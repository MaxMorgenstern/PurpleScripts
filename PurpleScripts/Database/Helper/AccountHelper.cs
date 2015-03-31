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
		private static string accountWarningsTable 	= "account_warnings";
		private static string accountLogTable 		= "account_log";

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
					return true;
				}

				if(!String.IsNullOrEmpty(userData.token) && password_or_token == userData.token)
				{
					if(PurpleHash.GetTokenDate(userData.token)
					   .AddDays(PurpleConfig.Account.User.Token.DaysValid) >= DateTime.Now)
					{
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
				PurplePassword PuPa = new PurplePassword ();
				account.password = PuPa.CreateHash(password);
				account.NewGuid();
				account.account_created = DateTime.Now;
				account.account_type = "User";
				account.active = false;

				// TODO: check if all data is set
				return create_database_user(account);
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

		public static bool Disable(string identifier, string password)
		{
			if(!ValidateAuthentication (identifier, password))
				return false;

			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			userData.active = false;

			return update_user_reference (userData, false);
		}

		public static bool Login(string identifier, string password)
		{
			string token = string.Empty;
			return Login (identifier, password, out token);
		}

		public static bool Login(string identifier, string password, out string token)
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

			return update_user_reference (userData, true);
		}

		public static bool Logout(string identifier)
		{
			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return false;

			userData.token = null;
			userData.token_created = DateTime.MinValue;
			return update_user_reference (userData, false);
		}

		public static string GenerateToken(string identifier, string password_or_token)
		{
			PurpleAccount userData = get_database_user (identifier);
			if(userData == null)
				return string.Empty;
			return GenerateToken (userData, password_or_token);
		}

		public static string GenerateToken(PurpleAccount account, string password_or_token)
		{
			if(!ValidateAuthentication (account.username, password_or_token))
				return string.Empty;
			
			account.token = PurpleHash.Token ();
			account.token_created = DateTime.Now;
			
			if(update_user_reference (account, true))
				return account.token;
			return string.Empty;
		}


		// Warning / Log ////////////////////////////

		public static bool AddWarning(string identifier, string comment, int level)
		{
			return AddWarning (identifier, comment, level, false);
		}

		public static bool AddWarning(string identifier, string comment, int level, bool notifyUser)
		{
			PurpleAccount userData = get_user_reference (identifier);
			if(userData == null)
				return false;
			
			int result = add_database_user_warning (userData.id, level, comment);
			if(result == 1 && notifyUser)
				PurpleMailGenerator.SendMail(PurpleConfig.Mail.Template.Warning, userData, comment);
			return (result == 1) ? true : false;
		}

		public static bool AddLog(string identifier, string comment)
		{
			PurpleAccount userData = get_user_reference (identifier);
			if(userData == null)
				return false;
			
			int result = add_database_user_log (userData.id, comment);
			return (result == 1) ? true : false;
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
		
		private static bool update_user_reference(PurpleAccount user, bool authenticated)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList
				.Find (x => x.UserName == user.username || x.UserGUID.Equals(user.guid));
			if(pnu != null)
			{
				pnu.UserGUID 			= new Guid(user.guid);
				pnu.UserID 				= user.id;
				pnu.UserType 			= (PurpleNetwork.UserTypes) Enum.Parse(
											typeof(PurpleNetwork.UserTypes), user.account_type, true);
				pnu.UserConnectedTime	= user.last_login;
				pnu.UserName			= user.username;
				pnu.UserToken			= user.token;
				pnu.UserTokenCreated	= user.token_created;

				pnu.UserAuthenticated	= authenticated;
			}
			// TODO - new user ref ???
			return update_database_user (user);
		}

		private static bool update_database_user(PurpleAccount user)
		{
			int result = user.ToSQLUpdate ().Execute ();
			return (result==1) ? true : false;
		}

		private static bool create_database_user(PurpleAccount user)
		{
			int result = user.ToSQLInsert ().Execute ();
			if(result == 1)
				PurpleMailGenerator.SendMail(PurpleConfig.Mail.Template.Register, user);
			return (result==1) ? true : false;
		}






		private static int add_database_user_warning(int account_id, int warning_level, string comment)
		{
			return SQLGenerator.New ().Insert(accountWarningsTable, "account_id, warning_level, comment, timestamp")
				.Values(account_id+", "+warning_level+", "+comment+", now()").Execute();
		}

		private static int add_database_user_log(int account_id, string comment)
		{
			return SQLGenerator.New ().Insert (accountLogTable, "account_id, log, timestamp")
				.Values (account_id+", "+comment+", now()").Execute();
		} 

	}
}
