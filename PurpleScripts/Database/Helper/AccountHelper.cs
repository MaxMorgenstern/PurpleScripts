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

		public static bool ValidateAuthentication(string username, string password="", string token="")
		{
			DataTable userData = get_user (username);
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
	

		
		// PRIVATE ////////////////////////////

		private static DataTable get_user(string username)
		{
			PurpleNetworkUser pnu = PurpleServer.UserList.Find (x => x.UserName == username);
			if(pnu != null)
			{
				return pnu.GetAsDataTable ();
			}
			else
			{
				return get_user_from_database (username);
			}
		}

		private static DataTable get_user_from_database(string username)
		{
			return SQLGenerator.New ().Select ("username, password, token, token_created").From (accountTable).Where ("username=" + username).Where ("active=1").Single ().Fetch ();
		}

		private static bool update_user()
		{
			// TODO
			return true;
		}

		private static bool update_user_to_database()
		{
			// TODO
			return true;
		}
	}
}
