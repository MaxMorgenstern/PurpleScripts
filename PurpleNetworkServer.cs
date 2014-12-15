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
	namespace Server
	{
		public enum ServerType { Account, Lobby, Game, Monitoring };

		public class ServerConfig
		{
			public string name;
			public string password;
			public ServerType type;
			public int maxUser;
			public int port;

			// CONSTRUCTOR
			public ServerConfig ()
			{
				name = "GameServer";
				password = "purple";
				type = ServerType.Game;
				maxUser = 32;
				port = 1000;
			}
			public ServerConfig (string serverName, string serverPassword, ServerType serverType)
			{
				name = serverName;
				password = serverPassword;
				type = serverType;
				maxUser = 32;
				port = 1000;
			}

			public void SetType(string serverType)
			{
				SetType(parse_server_type (serverType));
			}

			public void SetType(ServerType serverType)
			{
				type = serverType;
			}

			// PRIVATE HELPER /////////////////////////
			private ServerType parse_server_type(string serverType)
			{
				return (ServerType) Enum.Parse(typeof(ServerType), serverType, true);
			}
		}



		public class PurpleServer : MonoBehaviour
		{

			private ServerConfig stdServerConfig;	

			private static PurpleServer instance;


			// START UP /////////////////////////
			protected PurpleServer ()
			{
				stdServerConfig = new ServerConfig ();
			}

			
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


			// PUBLIC FUNCTIONS /////////////////////////
			public static void LaunchServer()
			{
				Instance.launch_server ();
			}

			public static void LaunchServer(ServerConfig config)
			{
				Instance.launch_server (config);
			}



			
			private void launch_server()
			{
				launch_server (stdServerConfig);
			}

			private void launch_server(ServerConfig config)
			{
				int player = config.maxUser;
				string password = config.password;
				int port = config.port;
				PurpleNetwork.LaunchLocalServer(player, password, port);
			}

/*
			public static void LaunchServer(string type)
			{
				LaunchServer(Instance.parse_server_type (type));
			}

			public static void LaunchServer(ServerType type)
			{
				// TODO
			}
*/
			// LaunchLocalServer (int player, string localPassword, int localPort)

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
			




			// Game List - or game instance list
			// CreateGame(name, password, player, game, version, options, public);

			// event handler

			// interval update

			// option autoritative



		}
	}
}
