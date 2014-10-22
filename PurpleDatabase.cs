#if UNITY_EDITOR_WIN
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

// TODO: Work in progress

public class PurpleDatabase : MonoBehaviour {
	
	private string serverIP = PurpleConfig.Database.IP;
	private string serverDatabase = PurpleConfig.Database.Name;
	private string serverUser = PurpleConfig.Database.User;
	private string serverPassword = PurpleConfig.Database.Password;

	private string connectionString;
	private MySqlConnection connection;
	
	void Start () {
		Initialize ();
		SelectStatement();
	}





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

		if (OpenConnection() == true)
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
			CloseConnection();

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
		
		if (OpenConnection () == true) {
			MySqlCommand cmd = new MySqlCommand (query, connection);

			cmd.ExecuteNonQuery ();			
			CloseConnection ();
		}
	}

	//Update statement
	private void UpdateStatement()
	{
		string query = ""; // Update query

		if (OpenConnection() == true)
		{
			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandText = query;
			cmd.Connection = connection;

			cmd.ExecuteNonQuery();
			CloseConnection();
		}
	}
	
	//Delete statement
	private void DeleteStatement()
	{
		string query = ""; // Delete query
		
		if (OpenConnection() == true)
		{
			MySqlCommand cmd = new MySqlCommand(query, connection);

			cmd.ExecuteNonQuery();
			CloseConnection();
		}
	}
	
	/************************************/
	
	// initialize database 
	private void Initialize(){
		if(connection == null || connection.State != ConnectionState.Open){
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
				Debug.Log(ex.ToString());
			}
		}
	}

	private bool OpenConnection()
	{
		try
		{
			connection.Open();
			return true;
		}
		catch (MySqlException ex)
		{
			//0: Cannot connect to server.
			//1045: Invalid user name and/or password.
			switch (ex.Number)
			{
			case 0:
				Debug.Log("Cannot connect to server.  Contact administrator");
				break;
				
			case 1045:
				Debug.Log("Invalid username/password, please try again");
				break;
			}
			return false;
		}
	}
	
	//Close connection
	private bool CloseConnection()
	{
		try
		{
			connection.Close();
			return true;
		}
		catch (MySqlException ex)
		{
			Debug.Log(ex.Message);
			return false;
		}
	}
}
#endif