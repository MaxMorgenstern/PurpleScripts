using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Entities.Database;
using Entities.PurpleNetwork;
using PurpleDatabase.Extension;
using PurpleNetwork.Server;

namespace PurpleDatabase.Helper
{
	public class AccountHelper : MonoBehaviour
	{
		private static string accountTable 			= "account";
		private static List<string> errorList;

		public static bool ValidateAuthentication(string identifier, string password_or_token)
		{
			PurpleAccount userData = get_user_reference (identifier);
			if(userData == null)
				return false;

			if(!string.IsNullOrEmpty(password_or_token))
			{
				PurplePassword pp = new PurplePassword();
				if(pp.ValidatePassword(password_or_token, userData.password))
				{
					update_user_last_seen(userData);
					return true;
				}

				if(!String.IsNullOrEmpty(userData.token) && password_or_token == userData.token)
				{
					if(PurpleHash.GetTokenDate(userData.token)
					   .AddDays(PurpleConfig.Account.User.Token.DaysValid) >= DateTime.Now)
					{
						update_user_last_seen(userData);
						return true;
					}
				}
			}
			return false;
		}

		public static bool Register(PurpleAccount account, string password)
		{
			if(!IsUniqueUsername(account.username))
				return false;

			if (PurpleAccountValidator.ValidatePasswordStrength (password)
			    	&& PurpleAccountValidator.ValidateUsername (account.username))
			{
				return create_database_user(account, password);
			}
			return false;
		}

		public static bool IsUniqueUsername(string username)
		{
			PurpleAccount userData = get_database_user(username);
			if(userData != null)
				return false;
			return true;
		}

		public static bool Disable(string identifier, string password, NetworkPlayer? np = null)
		{
			if(!ValidateAuthentication (identifier, password))
				return false;

			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			userData.active = false;

			if(np != null)
			{
				return update_user_reference (userData, np, false);
			}
			else
			{
				return update_user_reference (userData, false);
			}
		}

		public static bool Login(string identifier, string password)
		{
			string token = string.Empty;
			return Login (identifier, password, out token);
		}

		public static bool Login(string identifier, string password, out string token)
		{
			return Login (identifier, password, null, out token);
		}

		public static bool Login(string identifier, string password, NetworkPlayer? np, out string token)
		{
			token = String.Empty;
			if(!ValidateAuthentication (identifier, password))
				return false;

			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			userData.token = PurpleHash.Token ();
			token = userData.token;
			userData.token_created = DateTime.Now;
			userData.last_login = DateTime.Now;

			PurpleNetworkUser pnu = PurpleServer.UserList
				.Find (x => x.UserName == identifier || x.UserGUID.Equals(identifier));
			if(pnu != null)
				userData.last_login_ip = pnu.UserReference.ipAddress;

			if(np != null)
			{
				return update_user_reference (userData, np, true);
			}
			else
			{
				return update_user_reference (userData, true);
			}
		}

		public static bool Logout(string identifier, string password_or_token)
		{
			if(!ValidateAuthentication (identifier, password_or_token))
				return false;

			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			userData.token = null;
			userData.token_created = DateTime.MinValue;
			return update_user_reference (userData, false);
		}

		public static string GenerateToken(string identifier, string password_or_token, NetworkPlayer? np = null)
		{
			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return string.Empty;
			return GenerateToken (userData, password_or_token, np);
		}

		public static string GenerateToken(PurpleAccount account, string password_or_token, NetworkPlayer? np = null)
		{
			if(!ValidateAuthentication (account.username, password_or_token))
				return string.Empty;

			account.token = PurpleHash.Token ();
			account.token_created = DateTime.Now;

			if(np != null)
			{
				if(update_user_reference (account, np, true))
					return account.token;
			}
			else
			{
				if(update_user_reference (account, true))
					return account.token;
			}
			return string.Empty;
		}


		// Warning / Log ////////////////////////////

		public static bool AddWarning(string reporter, string password_or_token, string identifier, string comment, int level)
		{
			return AddWarning (reporter, password_or_token, identifier, comment, level, false);
		}

		public static bool AddWarning(string reporter, string password_or_token, string identifier, string comment, int level, bool notifyUser)
		{
			if(!ValidateAuthentication (reporter, password_or_token))
				return false;

			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			bool result = add_database_user_warning (userData.id, level, comment);
			if(result && notifyUser)
				PurpleMailGenerator.SendMail(PurpleConfig.Mail.Template.Warning, userData, comment);
			return result;
		}

		public static bool AddLog(string identifier, string comment)
		{
			PurpleAccount userData = get_user_reference (identifier);
			if(userData == null)
				return false;

			return add_database_user_log (userData.id, comment);
		}

		public static List<string> GetErrorList()
		{
			return errorList;
		}

		// PRIVATE ////////////////////////////

		private static PurpleAccount get_user_reference(string identifier)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList
				.Find (x => x.UserName == identifier || x.UserGUID.Equals(identifier));
			if(pnu != null)
			{
				return pnu.GetAsDataTable ().ToList<PurpleAccount>().FirstOrDefault();
			}
			else
			{
				return get_database_user (identifier);
			}
		}

		private static PurpleAccount get_database_user(string identifier)
		{
			return SQLGenerator.New ().Select ("*").From (accountTable)
				.Where ("username=" + identifier).Where ("active=1")
				.Where ("guid=" + identifier, "OR").Where ("active=1")
					.FetchSingle ().ToObject<PurpleAccount> ();
		}


		private static bool update_user_reference(PurpleAccount user, NetworkPlayer? np, bool authenticated)
		{
			if(np == null)
				return false;
			PurpleNetworkUser pnu = PurpleServer.UserList
				.Find (x => x.UserReference == np || x.UserName == user.username || x.UserGUID.Equals(user.guid));
			return update_user_reference(user, pnu, authenticated);
		}

		private static bool update_user_reference(PurpleAccount user, bool authenticated)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList
				.Find (x => x.UserName == user.username || x.UserGUID.Equals(user.guid));
			return update_user_reference(user, pnu, authenticated);
		}

		private static bool update_user_reference(PurpleAccount user, PurpleNetworkUser pnu, bool authenticated)
		{
			if(pnu != null)
			{
				pnu.UserGUID 			= new Guid(user.guid);
				pnu.UserID 				= user.id;
				pnu.UserType 			= (PurpleNetwork.UserTypes) Enum.Parse(
											typeof(PurpleNetwork.UserTypes), user.account_type, true);
				pnu.UserName			= user.username;
				pnu.UserToken			= user.token;
				pnu.UserTokenCreated	= user.token_created;

				pnu.UserAuthenticated	= authenticated;
			}
			user.last_seen 				= DateTime.Now;
			return update_database_user (user);
		}

		private static bool update_user_last_seen(PurpleAccount user)
		{
			return SQLGenerator.New ().Update("last_seen = now()", accountTable, "guid="+user.guid).Execute() == 1;
		}

		private static bool update_database_user(PurpleAccount user)
		{
			if(PurpleAttributes.Validator.Validate (user, out errorList))
			{
				int result = user.ToSQLUpdate ().Execute ();
				return (result==1) ? true : false;
			}
			return false;
		}

		private static bool create_database_user(PurpleAccount user, string password)
		{
			PurplePassword PuPa = new PurplePassword ();
			user.password = PuPa.CreateHash(password);
			user.NewGuid();
			user.account_created = DateTime.Now;
			user.account_type = "User";
			user.active = false;

			if(PurpleAttributes.Validator.Validate (user, out errorList))
			{
				int result = user.ToSQLInsert ().Execute ();
				if(result == 1)
					PurpleMailGenerator.SendMail(PurpleConfig.Mail.Template.Register, user);
				return (result==1) ? true : false;
			}
			return false;
		}


		private static bool add_database_user_warning(int account_id, int warning_level, string comment)
		{
			PurpleAccountWarnings paw = new PurpleAccountWarnings ();
			paw.account_id = account_id;
			paw.warning_level = warning_level;
			paw.comment = comment;

			if(PurpleAttributes.Validator.Validate (paw, out errorList))
			{
				int result = paw.ToSQLInsert ().Execute ();
				return (result==1) ? true : false;
			}
			return false;
		}

		private static bool add_database_user_log(int account_id, string comment)
		{
			PurpleAccountLog pal = new PurpleAccountLog ();
			pal.account_id = account_id;
			pal.log = comment;
			if(PurpleAttributes.Validator.Validate (pal, out errorList))
			{
				int result = pal.ToSQLInsert ().Execute ();
				return (result==1) ? true : false;
			}
			return false;
		}

	}
}
