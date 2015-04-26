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
			guid = Guid.NewGuid ();
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
			public List<string> error;
			public bool validate;
		}

		// USER ACTION
		public class Action : PurpleUserMessage
		{
			public string ClientName;
			public string ActionName;
		}

		// USER LOGIN
		public class Authentication : PurpleUserMessage
		{
			public string ClientName;
			public string ClientPassword;
			public string ClientToken;
			public bool ClientAuthenticated;
		}

		// CREATE ACCOUNT
		public class CreateAccount : PurpleUserMessage
		{
			public string ClientName;
			public string ClientPassword;
			public string ClientEmail;
			public string ClientFirstName;
			public string ClientLastName;
			public string ClientGender;
			public DateTime ClientBirthday;
			public string ClientCountry;
			public string ClientLanguage;
			public string ClientComment;
		}

		// CREATE CHARACTER
		public class CreateCharacter : PurpleUserMessage
		{
			public string CharacterName;
			// TODO
		}

		// CREATE Game
		public class CreateGame : PurpleUserMessage
		{
			public string GameName;
			// TODO
		}
	}

	// GAMEMASTER MESSAGES ////////////////////////////
	namespace Gamemaster
	{
		public class PurpleGamemasterMessage : PurpleMessage
		{
			public string gmUsername;
			public string gmPassword;
			public string gmToken;
			public bool validate;
		}

		public class Warning : PurpleGamemasterMessage
		{
			public string warningComment;
			public int warningLevel;
			public string warningUser;
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

		// VERSION MESSAGE
		public class Version : PurpleMessage
		{
			public string BuildVersion;
			public string ServerVersion;
			public string ClientVersion;
		}

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
