using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Entities.PurpleNetwork;
using PurpleDatabase.Extension;
using PurpleNetwork.Server;
using Entities.Database;
using System.Collections.Generic;

// TODO: update reference - update database
// update PurpleNetworkUser

namespace PurpleDatabase.Helper
{
	public class AccountHelper : MonoBehaviour
	{
		private static string accountTable 			= "account";
		private static string accountWarningsTable 	= "account_warnings";
		private static string accountLogTable 		= "account_log";

		public static bool ValidateAuthentication(string identifier, string password="", string token="")
		{
			PurpleAccount userData = get_user (identifier);
			if(userData == null)
				return false;

			if(!string.IsNullOrEmpty(password))
			{
				PurplePassword pp = new PurplePassword();
				if(pp.ValidatePassword(password, userData.password))
				{
					return true;
				}
			}
			else if (!string.IsNullOrEmpty(token))
			{
				if(!String.IsNullOrEmpty(userData.token) && token == userData.token)
				{
					return true;
				}
			}
			return false;
		}

		// TODO - Test
		public static bool Register(PurpleAccount account)
		{
			if(!IsUniqueUsername(account.username))
				return false;

			if (PurpleAccountValidator.ValidatePasswordStrength (account.password) 
			    	&& PurpleAccountValidator.ValidateUsername (account.username))
			{
				int affectedRows = account.ToSQLInsert().Execute();
				return (affectedRows == 1) ? true : false;
			}
			return false;
		}

		// TODO - Test
		public static bool IsUniqueUsername(string username)
		{
			PurpleAccount userData = get_database_user(username).ToList<PurpleAccount>().FirstOrDefault();
			if(userData != null)
				return false;
			return true;
		}

		// TODO - Test
		public static bool Disable(string identifier, string password)
		{
			if(!ValidateAuthentication (identifier, password))
				return false;

			PurpleAccount userData = get_user (identifier);
			if(userData == null)
				return false;

			userData.active = false;
			// TODO
			return false;
			//return (affectedRows == 1) ? true : false;
		}

		// TODO - Test
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

			PurpleAccount userData = get_user (identifier);
			if(userData == null)
				return false;

			userData.token = PurpleHash.Token ();
			token = userData.token;
			userData.token_created = DateTime.Now;

			// TODO

			return true;
		}
		
		public static bool Logout(string identifier)
		{
			/*
			string query = SQLGenerator.Update ("token=null", "Account",  "username = `"+username+"`");
			Debug.Log (query); // TODO - test query
			int result = PurpleDatabase.ExecuteQuery (query);
			return (result==1) ? true : false;
			*/
			return false;
		}
	
		public static bool AddWarning(string identifier, string comment, int level)
		{
			PurpleAccount userData = get_user (identifier);
			if(userData == null)
				return false;
			
			int result = add_database_user_warning (userData.id, level, comment);
			return (result == 1) ? true : false;
		}

		public static bool AddLog(string identifier, string comment)
		{
			PurpleAccount userData = get_user (identifier);
			if(userData == null)
				return false;
			
			int result = add_database_user_log (userData.id, comment);
			return (result == 1) ? true : false;
		}






		// PRIVATE ////////////////////////////

		// TODO: guid
		private static PurpleAccount get_user(string identifier)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList.Find (x => x.UserName == identifier || x.UserGUID.Equals(identifier));
			if(pnu != null)
			{
				return pnu.GetAsDataTable ().ToList<PurpleAccount>().FirstOrDefault();
			}
			else
			{
				return get_database_user (identifier).ToList<PurpleAccount>().FirstOrDefault();
			}
		}

		private static DataTable get_database_user(string identifier)
		{
			return SQLGenerator.New ().Select ("id, guid, username, password, token, token_created").From (accountTable)
				.Where ("username=" + identifier).Where ("active=1")
					.Where ("guid=" + identifier, "OR").Where ("active=1").Single ().Fetch ();
		}








		private static bool update_network_user(PurpleAccount user)
		{
			return false;
		}

		// TODO - Test
		private static bool update_database_user(PurpleAccount user)
		{
			int result = user.ToSQLUpdate ().Execute ();
			return (result==1) ? true : false;
		}

		private static bool create_database_user(PurpleAccount user)
		{
			int result = user.ToSQLInsert ().Execute ();
			return (result==1) ? true : false;
		}






		private static int add_database_user_warning(int account_id, int warning_level, string comment)
		{
			return SQLGenerator.New ().Insert(accountWarningsTable, "account_id, warning_level, comment, timestamp").Values(account_id+", "+warning_level+", "+comment+", now()").Execute();
		}

		private static int add_database_user_log(int account_id, string comment)
		{
			return SQLGenerator.New ().Insert (accountLogTable, "account_id, log, timestamp").Values (account_id+", "+comment+", now()").Execute();
		} 

	}
}
