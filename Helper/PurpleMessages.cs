using UnityEngine;
using System;
using System.Xml.Serialization;

// TODO: create besser classes

namespace PurpleMessages
{
	// BASE MESSAGE CLASS ////////////////////////////
	public class PurpleMessage
	{
		public Guid guid;

		// CONSTRUCTOR
		public PurpleMessage()
		{
			guid = System.Guid.NewGuid ();
		}
	}

	// EMPTY MESSAGE //////////////////////////// 
	public class Empty : PurpleMessage
	{
	}

	// EXAMPLE MESSAGE //////////////////////////// 
	public class Example : PurpleMessage
	{
		public string test;
	}

	// PLAYER MESSAGE //////////////////////////// 
	public class Player : PurpleMessage
	{
		public NetworkPlayer player;
		public string player_name;
	}


	// SERVER RELATED MESSAGES //////////////////////////// 
	namespace Server
	{
		// SERVER STATUS //////////////////////////// 
		public class Data : PurpleMessage
		{
			public string serverName;
			public string serverHostname;
			public string serverPassword;
		}

		// SERVER STATUS //////////////////////////// 
		public class Status : PurpleMessage
		{
			public DateTime serverTime;
			public DateTime serverUptime;
			public String serverVersion;

			// CONSTRUCTOR
			public Status()
			{
				serverTime = DateTime.Now;
				serverVersion = "0.0.1.1";
			}
		}

		// PING MESSAGE //////////////////////////// 
		public class Ping : PurpleMessage
		{
			public DateTime triggerTime;
			public DateTime localTime;
			
			public Ping()
			{
				localTime = DateTime.Now;
			}
		}
	}

}
