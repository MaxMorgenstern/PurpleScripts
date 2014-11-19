using UnityEngine;
using System;
using System.Collections;

// This is just an idea to provide a client and autoritative server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO: A lot

/*
 * Server: 
 * 	Start, Stop - Account Server - Game Server (with Lobby)
 * 	Listener
 * 	Login
 */

namespace PurpleNetwork
{

	public class PurpleServer : MonoBehaviour
	{
		public class Config
		{
			public string todo;
		}

		public enum ServerType { Account, /*Lobby,*/ Game, Multi, Monitoring };
		private Config stdServerConfig;

		private static PurpleServer instance;


		// START UP /////////////////////////
		protected PurpleServer ()
		{
			stdServerConfig = new Config ();
			// TODO
		}


		public static void LaunchServer()
		{
			LaunchServer (ServerType.Multi);
		}

		public static void LaunchServer(string type)
		{
			LaunchServer(Instance.parse_server_type (type));
		}

		public static void LaunchServer(ServerType type)
		{
			// TODO
		}

		public static void Setup()
		{
			// TODO: do server setup
			// perhaps return an server setup option object
		}


		public static void StopServer()
		{
			StopServer (0);
		}

		public static void StopServer(float time)
		{
			// TODO: float is time in seconds
		}


		public static void RestartServer()
		{
			RestartServer (0);
		}
		
		public static void RestartServer(float time)
		{
			// TODO: float is time in seconds
		}
		
		
		
		private ServerType parse_server_type(string type)
		{
			return (ServerType) Enum.Parse(typeof(ServerType), type, true);
		}

		// Game List - or game instance list
		// CreateGame(name, password, player, game, version, options, public);

		// event handler

		// interval update

		// option autoritative




		// SINGLETON /////////////////////////
		public static PurpleServer Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleServerManager");
					instance     			= gameObject.AddComponent<PurpleServer> ();
				}
				return instance;
			}
		}
	}
}
