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

// Game List - or game instance list
// CreateGame(name, password, player, game, version, options, public);

// event handler

// interval update

// option autoritative

using System.Collections.Generic;

namespace PurpleNetwork
{
	namespace Server
	{
		public enum ServerType { Account, Lobby, Game, Monitoring };

		public class ServerConfig
		{
			public string name;
			public ServerType type;

			public string password;
			public int maxUser;
			public int port;

			// CONSTRUCTOR
			public ServerConfig ()
			{
				name = "GameServer";
				type = ServerType.Game;
				
				password = "purple";
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
			private static int stdServerdelay;
			
			private static string notificationMessage;
			private static List <int> intervalList;


			// START UP /////////////////////////
			protected PurpleServer ()
			{
				stdServerConfig = new ServerConfig ();
				stdServerdelay = 10;
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


			public static void StopServer()
			{
				StopServer (stdServerdelay);
			}
			
			public static void StopServer(int seconds)
			{
				Instance.stop_server (seconds);
			}


			public static void RestartServer()
			{
				RestartServer (stdServerdelay);
			}
			
			public static void RestartServer(int seconds)
			{
				Instance.restart_server (seconds);
			}

			
			public static void SetNotificationInterval(int interval)
			{
				List<int> intervalList = new List<int> ();
				intervalList.Add (interval);
				Instance.set_notification_interval (intervalList);
			}

			public static void SetNotificationMessage(string message)
			{
				Instance.set_notification_message (message);
			}
						
			public static string GetNotificationMessage()
			{
				return Instance.get_notification_message ();
			}

			
			public static void SetNotificationInterval(List <int> interval)
			{
				Instance.set_notification_interval (interval);
			}

			public static List <int> GetNotificationInterval()
			{
				return Instance.get_notification_interval ();
			}


			
			// PRIVATE FUNCTIONS /////////////////////////
			// START
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


			// STOP
			private void stop_server(int seconds)
			{
				PurpleCountdown.CountdownDoneEvent += stop_server_done;
				PurpleCountdown.CountdownRunEvent += stop_server_run;
				PurpleCountdown.Countdown (seconds);
			}

			private void stop_server_run()
			{
				float time_left = PurpleCountdown.CountdownTimeLeft ();
				// TODO - check time left
				PurpleNetwork.Broadcast ("server_broadcast", 
						combine_notification_message(notificationMessage, (int)PurpleCountdown.CountdownTimeLeft()));
			}

			private void stop_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= stop_server_done;
				PurpleCountdown.CountdownRunEvent -= stop_server_run;
				PurpleNetwork.StopLocalServer ();
			}


			// RESTART
			private void restart_server(int seconds)
			{
				PurpleCountdown.CountdownDoneEvent += restart_server_done;
				PurpleCountdown.CountdownRunEvent += restart_server_run;
				PurpleCountdown.Countdown (seconds);
			}
			
			private void restart_server_run()
			{
				float time_left = PurpleCountdown.CountdownTimeLeft ();
				// TODO - check time left
				PurpleNetwork.Broadcast ("server_broadcast", 
				                         combine_notification_message(notificationMessage, (int)PurpleCountdown.CountdownTimeLeft()));
			}

			private void restart_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= restart_server_done;
				PurpleCountdown.CountdownRunEvent -= restart_server_run;
				PurpleNetwork.RestartLocalServer ();
			}


			// SET NOTIFICATION MESSAGE
			private void set_notification_message(string message)
			{
				notificationMessage = message;
			}

			private string get_notification_message()
			{
				return notificationMessage;
			}

			
			// SET NOTIFICATION INTERVAL
			private void set_notification_interval(List <int> interval)
			{
				intervalList = interval;
			}


			// GET NOTIFICATION INTERVAL
			private List <int> get_notification_interval()
			{
				return intervalList;
			}
		

			
			// HELPER ////////////////////
			private string combine_notification_message(string message, int time)
			{
				// TODO - keyword
				return message;
			}
		}
	}
}
