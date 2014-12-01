/**
 * If you get an error like:
 * 		Internal compiler error. ... Could not load file or assembly 'System.Drawing,
 * 		Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.
 * change the API compatibility Level from ".NET 2.0 Subset" to ".NET 2.0"!
 *
 * Also see: http://answers.unity3d.com/questions/379212/how-to-solve-the-error-type-or-namespace-systemdat.html
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

#if !UNITY_WEBPLAYER
#endif


// TODO: Work in progress
namespace PurpleDatabase
{
	public class PurpleDatabase : MonoBehaviour {

		private static string serverIP;
		private static string serverDatabase;
		private static string serverUser;
		private static string serverPassword;

		private static string connectionString;
		private static MySqlConnection connection;
		
		private static PurpleDatabase instance;

		protected PurpleDatabase () {
			try{
				serverIP = PurpleConfig.Database.IP;
				serverDatabase = PurpleConfig.Database.Name;
				serverUser = PurpleConfig.Database.User;
				serverPassword = PurpleConfig.Database.Password;
			} catch(Exception e){
				Debug.LogError("Can not read Purple Config! " + e.ToString());
			}
		}


		// SINGLETON /////////////////////////
		public static PurpleDatabase Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleDatabase");
					instance     			= gameObject.AddComponent<PurpleDatabase> ();
					instance.initialize();
				}
				return instance;
			}
		}

		// SETUP ////////////////////////////
		public static void Setup (string host, string database, string user, string password)
		{
			Instance.purple_setup (host, database, user, password);
		}


		public static void Initialize()
		{
			Instance.open_connection ();
			Instance.close_connection ();
		}

		
		// PRIVATE ////////////////////////////

		// SETUP ////////////////////////////
		private void purple_setup(string host, string database, string user, string password)
		{
			serverIP = host;
			serverDatabase = database;
			serverUser = user;
			serverPassword = password;
			Instance.initialize (true);
		}


		// HELPER ////////////////////////////

		public static string my_SQL_escape(string str)
		{
			return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
			                     delegate(Match match)
			                     {
				string v = match.Value;
				switch (v)
				{
				case "\x00":	 	// ASCII NUL (0x00) character
					return "\\0";
				case "\b":		 	// BACKSPACE character
					return "\\b";
				case "\n":		 	// NEWLINE (linefeed) character
					return "\\n";
				case "\r":		 	// CARRIAGE RETURN character
					return "\\r";
				case "\t":			// TAB
					return "\\t";
				case "\u001A":	 	// Ctrl-Z
					return "\\Z";
				default:
					return "\\" + v;
				}
			});
		}

		
		
	
		/*
		// move to network manager
		private bool checkLogin(string username, string password){

			string pwDatabaseHash = "dummy";

			PurplePassword pm = new PurplePassword ();
			bool pwValid = pm.ValidatePassword (password, pwDatabaseHash);
			return pwValid;
		}


		// TODO - make all statements dynamic
		//Select statement
		public List< string >[] SelectStatement()
		{
			string query = "SELECT * FROM `one`"; // select statement

			//Create a list to store the result
			List< string >[] list = new List< string >[3];
			list[0] = new List< string >();
			list[1] = new List< string >();
			list[2] = new List< string >();

			if (open_connection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader dataReader = cmd.ExecuteReader();

				//Read the data and store them in the list
				while (dataReader.Read())
				{
					list[0].Add(dataReader["id"] + "");
					list[1].Add(dataReader["name"] + "");
					list[2].Add(dataReader["counter"] + "");
					Debug.Log (dataReader["id"] + " " + dataReader["name"] + " " + dataReader["counter"]);
				}

				dataReader.Close();
				close_connection();

				return list;
			}
			else
			{
				return list;
			}
		}

		// Insert statement
		private void InsertStatement()
		{
			string query = ""; // Insert query

			if (open_connection () == true) {
				MySqlCommand cmd = new MySqlCommand (query, connection);

				cmd.ExecuteNonQuery ();
				close_connection ();
			}
		}

		//Update statement
		private void UpdateStatement()
		{
			string query = ""; // Update query

			if (open_connection() == true)
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.CommandText = query;
				cmd.Connection = connection;

				cmd.ExecuteNonQuery();
				close_connection();
			}
		}

		//Delete statement
		private void DeleteStatement()
		{
			string query = ""; // Delete query

			if (open_connection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);

				cmd.ExecuteNonQuery();
				close_connection();
			}
		}
		*/


		// CREATE, INSERT, UPDATE, and DELETE
		private void sql_write_statement(string query)
		{
			if (open_connection () == true) {
				try
				{
					MySqlCommand cmd = new MySqlCommand (query, connection);
					cmd.ExecuteNonQuery ();
				}
				catch (Exception ex)
				{
					Debug.LogError("Can not execute query. " + ex.ToString());
				}
				close_connection ();
			}
		}

		// SELECT
		private void sql_read_statement(string query)
		{
			if (open_connection () == true) {
				MySqlDataReader reader = null;
				try
				{
					MySqlCommand cmd = new MySqlCommand (query, connection);
					reader = cmd.ExecuteReader ();
					while (reader.Read()) 
					{
						// TODO				row 0						row 1
						//Debug.Log(reader.GetInt32(0) + ": "  + reader.GetString(1));
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Can not execute query. " + ex.ToString());
				} 
				finally 
				{
					if (reader != null) 
					{
						reader.Close();
					}
				}
				close_connection ();
			}
		}



		// BASICS /////////////////////////

		// initialize database
		private void initialize()
		{
			initialize (false);
		}
		
		private void initialize(bool force)
		{
			if(connection == null || force){
				try
				{
					connectionString = "Server="+serverIP+";" +
						"Database="+serverDatabase+";" +
						"User ID="+serverUser+";" +
						"Pooling=false;" +
						"Password="+serverPassword+";";
					connection = new MySqlConnection (connectionString);
				}
				catch (Exception ex)
				{
					connection = null;
					Debug.Log(ex.ToString());
				}
			}
		}

		// open db connection
		private bool open_connection()
		{
			try
			{
				connection.Open();
				return true;
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						Debug.Log ("Cannot connect to server.");
						Debug.LogWarning (ex);
						break;

					case 1042:
						Debug.Log ("Unable to connect to any of the specified MySQL hosts.");
						Debug.LogWarning (ex);
						break;

					case 1045:
						Debug.Log("Invalid username/password.");
						Debug.LogWarning (ex);
						break;
					
					default:
						Debug.Log (ex.Message);
						Debug.Log (ex.Number);
						break;
				}
				connection = null;
				return false;
			}
		}

		// close DB connection
		private bool close_connection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				Debug.Log(ex.Message);
				connection = null;
				return false;
			}
		}
	}
}
