using UnityEngine;
using System.Collections;
using PurpleDatabase.Extension;
using PurpleNetwork.Server;
using System.Data;
using System.Linq;
using System;

namespace PurpleDatabase.Helper
{
	public class AccountHelper : MonoBehaviour
	{
		private static string accountTable 			= "account";
		private static string accountWarningsTable 	= "account_warnings";
		private static string accountLogTable 		= "account_log";

		public static bool ValidateAuthentication(string identifier, string password="", string token="")
		{
			DataTable userData = get_user (identifier);
			if(userData.Rows.Count == 0)
				return false;

			if(!string.IsNullOrEmpty(password))
			{
				PurplePassword pp = new PurplePassword();
				if(pp.ValidatePassword(password, userData.Rows[0]["password"].ToString()))
				{
					return true;
				}
			}
			else if (!string.IsNullOrEmpty(token))
			{
				if(token == userData.Rows[0]["token"].ToString())
				{
					return true;
				}
			}

			return false;
		}

		// TODO
		public static bool Register(string todo)
		{
			// TODO: usernanme regex + password regex
			/*
			Username
			password
			email
			firstname
			lastname
			gender
			birthday
			country code
			language
			 */
			return false;
		}

		// TODO
		public static bool Delete(string todo)
		{
			return false;
		}


		public static bool Login(string username, string password)
		{
			string token = string.Empty;
			return Login (username, password, out token);
		}

		public static bool Login(string username, string password, out string token)
		{
			// TODO
			token = String.Empty;

			DataTable userData = get_user (username);
			if(userData.Rows.Count == 0)
				return false;

			PurplePassword pp = new PurplePassword();
			if(pp.ValidatePassword(password, userData.Rows[0]["password"].ToString()))
			{
				// TODO: token
				return true;
			}

			// TODO: generate token set and update

			token = userData.Rows[0]["token"].ToString();
			return false;
		}
		
		public static bool Logout(string username)
		{
			/*
			string query = SQLGenerator.Update ("token=null", "Account",  "username = `"+username+"`");
			Debug.Log (query); // TODO - test query
			int result = PurpleDatabase.ExecuteQuery (query);
			return (result==1) ? true : false;
			*/
			return false;
		}
	
		public static bool AddWarning(string username, string comment, int level)
		{
			DataTable userData = get_user (username);
			if(userData.Rows.Count == 0)
				return false;
			
			int result = add_database_user_warning ((int)userData.Rows [0] ["id"], level, comment);
			return (result == 1) ? true : false;
		}

		public static bool AddLog(string username, string comment)
		{
			DataTable userData = get_user (username);
			if(userData.Rows.Count == 0)
				return false;
			
			int result = add_database_user_log ((int)userData.Rows [0] ["id"], comment);
			return (result == 1) ? true : false;
		}






		// PRIVATE ////////////////////////////

		// TODO: guid
		private static DataTable get_user(string identifier)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList.Find (x => x.UserName == identifier || x.UserGUID.Equals(identifier));
			if(pnu != null)
			{
				return pnu.GetAsDataTable ();
			}
			else
			{
				return get_database_user (identifier);
			}
		}

		private static DataTable get_database_user(string identifier)
		{
			return SQLGenerator.New ().Select ("id, username, password, token, token_created").From (accountTable)
				.Where ("username=" + identifier).Where ("active=1")
					.Where ("GUID=" + identifier, "OR").Where ("active=1").Single ().Fetch ();
		}

		private static bool update_database_user()
		{
			// TODO
			return true;
		}

		private static bool create_database_user()
		{
			// TODO
			SQLGenerator.Insert ("account", "GUID, username, password, email, account_created, first_name, last_name, gender, birthday, country_code, language_code, account_type, active")
				.Values ("");
			/*
// Create Account
INSERT INTO `account` (`GUID`, `username`, `password`, `email`, `email_verification`, `account_created`, `timestamp`, `first_name`, `last_name`, `gender`, `birthday`, `country_code`, `language_code`, `last_seen`, `last_login`, `last_login_ip`, `account_type`, `token`, `token_created`, `comment`, `active`)
VALUES ('INSERT GUID HERE', 'INSERTUSERNAME', 'PASSWORDHASH', 'MAILADDRESS', NULL, now(), now(), 'INSERTNAME', 'INSERTNAME2', 1, '1990-02-02', 'de', 'de', now(), NULL, NULL, 1, NULL, NULL, NULL, '1');
		 */
			return true;
		}
		
		private static int disable_database_user(int account_id)
		{
			// UPDATE `account` SET `active` = '0' WHERE `id` = '2';
			return SQLGenerator.New ().Update ("active = 0", "account", "id=" + account_id).Execute ();
		}
		
		private static int enable_database_user(int account_id)
		{
			// UPDATE `account` SET `active` = '1' WHERE `id` = '2';
			return SQLGenerator.New ().Update ("active = 1", "account", "id=" + account_id).Execute ();
		}
		
		
		private static int update_database_user_token(int account_id, string token)
		{
			// UPDATE `account` SET `token` = 'SETTOKEN', `token_created` = now() WHERE `id` = '2';
			return SQLGenerator.New ().Update ("token=" + token, "account", "id=" + account_id).Update ("token_created = now()").Execute ();
		}


		private static int add_database_user_warning(int account_id, int warning_level, string comment)
		{
			// INSERT INTO `account_warnings` (`account_id`, `warning_level`, `comment`, `timestamp`) VALUES ('2', '1', 'Reason for warning', now());
			return SQLGenerator.New ().Insert("account_warnings", "account_id, warning_level, comment, timestamp").Values(account_id+", "+warning_level+", "+comment+", now()").Execute();
		}

		private static int add_database_user_log(int account_id, string comment)
		{
			// INSERT INTO `account_log` (`account_id`, `log`, `timestamp`) VALUES ('1', 'Logdata', now());
			return SQLGenerator.New ().Insert ("account_log", "account_id, log, timestamp").Values (account_id+", "+comment+", now()").Execute();
		} 


	}
}
