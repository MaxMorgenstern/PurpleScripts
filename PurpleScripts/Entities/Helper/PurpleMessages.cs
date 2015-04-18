using UnityEngine;
using System;
using System.Collections.Generic;

namespace Entities.PurpleMessages
{
	// BASE MESSAGE CLASS ////////////////////////////
	public class PurpleMessage
	{
		public Guid guid;
		public DateTime timestamp;

		// CONSTRUCTOR
		public PurpleMessage()
		{
			guid = System.Guid.NewGuid ();
			timestamp = DateTime.Now;
		}
	}

	// EMPTY MESSAGE
	public class Empty : PurpleMessage
	{
	}

	// EMPTY MESSAGE
	public class Boolean : PurpleMessage
	{
		public bool value;
	}

	// EXAMPLE MESSAGE
	public class Example : PurpleMessage
	{
		public string example;

		public Example()
		{
			example = "Hello, World!";
		}
	}

	// BASIC DATA
	public class Data : PurpleMessage
	{
		public string data;
		public bool validate;
	}


	// PLAYER MESSAGES ////////////////////////////
	namespace User
	{
		public class PurpleUserMessage : PurpleMessage
		{
			public NetworkPlayer player;
			public List<string> error;
		}

		// USER ACTION
		public class Action : PurpleUserMessage
		{
			public string playerName;
			public string actionName;
		}

		// USER LOGIN
		public class Authentication : PurpleUserMessage
		{
			public string playerName;
			public string playerPassword;
			public string playerToken;
			public bool playerAuthenticated;
		}

		// CREATE ACCOUNT
		public class CreateAccount : PurpleUserMessage
		{
			public string playerUsername;
			public string playerPassword;
			public string playerEmail;
			public string playerFirstName;
			public string playerLastName;
			public string playerGender;
			public DateTime playerBirthday;
			public string playerCountry;
			public string playerLanguage;
			public string playerComment;
		}

		// CREATE CHARACTER
		public class CreateCharacter : PurpleUserMessage
		{
			public string characterName;
			/// TODO
		}

		// CREATE Game
		public class CreateGame : PurpleUserMessage
		{
			public string gameName;
			// TODO
		}
	}


	// SERVER RELATED MESSAGES ////////////////////////////
	namespace Server
	{
		/*
		// SERVER Credentials
		public class Credentials : PurpleMessage
		{
			public string name;
			public string hostname;
			public string ip;
			public string password;
			public int port;
			public int player;
			public int maxPlayer;
		}

		// SERVER STATUS
		public class Status : PurpleMessage
		{
			public DateTime time;
			public DateTime uptime;
			public string version;

			// CONSTRUCTOR
			public Status()
			{
				time = DateTime.Now;
				PurpleVersion pv = new PurpleVersion();
				version = pv.GetCurrent ();
			}
		}
		*/

		// PING MESSAGE
		public class Ping : PurpleMessage
		{
			public DateTime triggerTime;
			public DateTime bounceTime;
		}

		// SERVER MESSAGES
		public class Message : PurpleMessage
		{
			public string message;
		}

		// SPAM PREVENTION
		public class SpamPrevention : PurpleMessage
		{
			public int requestsInTime;
			public TimeSpan requestTimeSpan;
		}

		// DISCONNECT MESSAGE
		public class Disconnect : PurpleMessage
		{
			public int status;
			public string message;
		}
	}

}
