using UnityEngine;
using System;
using System.Collections;

// This is just an idea to provide a client and autoritative server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO: A lot
// TODO: Split in Client and Server file

/*
 * Server: 
 * 	Start, Stop - Account Server - Game Server (with Lobby)
 * 	Listener
 * 	Login
 * 
 * Clent:
 * 	Connect, Disconnect
 * 	...
 * 
 */

namespace PurpleNetwork
{
	public class PurpleServer : MonoBehaviour
	{
		public enum ServerType { Account, /*Lobby,*/ Game, Multi, Monitoring };

		private static PurpleServer instance;


		// START UP /////////////////////////
		protected PurpleServer ()
		{
			//
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


	///////////////////////////////////////
	// SPLIT HERE /////////////////////////
	///////////////////////////////////////


	public class PurpleClient : MonoBehaviour
	{
		private static PurpleClient instance;

		
		// START UP /////////////////////////
		protected PurpleClient ()
		{
			//
		}


		// ConnectToAccountServer(version)
		// ConnectToLobbyServer(version)
		// JoinRandomGame();
		// JoinGame(name, password);
		// GetData() GetRoomData() GetLobbyData()
		
		// GetGameList()
		// GetGameList(filter)

		// Login()
		// LoginWithFacebook()
		// LoginWithGoogle()

		// Chat()

		// event handler
		


		// SINGLETON /////////////////////////
		public static PurpleClient Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleClientManager");
					instance     			= gameObject.AddComponent<PurpleClient> ();
				}
				return instance;
			}
		}
	}
}
