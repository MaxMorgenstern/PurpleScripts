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
		/*
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



// -- ////////////////////////////
		public static DataTable SelectQuery(string query)
		{
			int rowCount = 0, colCount = 0;
			return Instance.sql_read_statement (query, out rowCount, out colCount);
		}
		public static DataTable SelectQuery(string query, out int rowCount)
		{
			int colCount = 0;
			return Instance.sql_read_statement (query, out rowCount, out colCount);
		}
		public static DataTable SelectQuery(string query, out int rowCount, out int colCount)
		{
			return Instance.sql_read_statement (query, out rowCount, out colCount);
		}

		public static DataRow SelectQuerySingle(string query)
		{
			int colCount;
			DataColumnCollection colums;
			return SelectQuerySingle (query, colCount, colums);
		}
		public static DataRow SelectQuerySingle(string query, out DataColumnCollection colums)
		{
			int colCount;
			return SelectQuerySingle (query, colCount, colums);
		}
		public static DataRow SelectQuerySingle(string query, out int colCount)
		{
			DataColumnCollection colums;
			return SelectQuerySingle (query, colCount, colums);
		}
		public static DataRow SelectQuerySingle(string query, out int colCount, out DataColumnCollection colums)
		{
			int rowCount;
			DataTable dt = Instance.sql_read_statement (query, out rowCount, out colCount);

			colums = dt.Columns;
			if(rowCount > 0)
				return dt.Rows[0];
			return null;
		}

// -- ////////////////////////////




		private DataTable sql_read_statement(string query, out int rowCount, out int colCount)
		{
			colCount = 0;
			rowCount = 0;
			DataTable dt = new DataTable();
			dt.Clear();

			if (open_connection () == true) {
				MySqlDataReader reader = null;
				try
				{
					MySqlCommand cmd = new MySqlCommand (query, connection);
					reader = cmd.ExecuteReader ();

					if(reader.HasRows)
					{
						colCount = reader.FieldCount;
						bool first_run = true;

						while (reader.Read()) 
						{
							rowCount++;
							DataRow dt_row = dt.NewRow();

							for(int i=0;i<reader.FieldCount;i++)
							{
								if(first_run)
								{
									dt.Columns.Add(reader.GetName(i));
									//dt_row = dt.NewRow();
								}
								dt_row[i] = reader[i];
							}

							dt.Rows.Add(dt_row);

							first_run = false;
						}
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
			return dt;
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
				if(connection != null)
				{
					connection.Open();
					return true;
				}
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
			}
			return false;
		}

		// close DB connection
		private bool close_connection()
		{
			try
			{
				if(connection != null)
				{
					connection.Close();
					return true;
				}
			}
			catch (MySqlException ex)
			{
				Debug.Log(ex.Message);
				connection = null;
			}
			return false;
		}
	}
}
