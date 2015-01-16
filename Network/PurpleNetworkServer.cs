using System;
using System.Collections.Generic;
using UnityEngine;

// This is just an idea to provide a client and (autoritative) server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO
// login
// options in general
// option autoritative

namespace PurpleNetwork
{
	namespace Server
	{
		public class PurpleServer : MonoBehaviour
		{
			private static PurpleServer instance;
			private static ServerConfig stdServerConfig;
			private static int stdServerdelay;
			private static List<int> stdNotificationIntervalList;
			
			private static ServerConfig currentServerConfig;
			
			private static string restartNotificationMessage;
			private static string restartNotificationDoneMessage;
			private static string shutdownNotificationMessage;
			private static string shutdownNotificationDoneMessage;
			private static List <int> notificationIntervalList;
			private static string notificationPlaceholder;


			// START UP /////////////////////////
			protected PurpleServer ()
			{
				stdServerConfig = new ServerConfig ();
				currentServerConfig = stdServerConfig;
				stdNotificationIntervalList = new List<int> (
					new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 30, 60, 300, 600, 900, 1800});

				try{
					notificationPlaceholder = PurpleConfig.Network.Message.Placeholder;
					stdServerdelay = PurpleConfig.Network.Server.Delay;

					shutdownNotificationMessage = PurpleI18n.Get("SHUTDOWN");
					shutdownNotificationDoneMessage = PurpleI18n.Get("SHUTDOWNNOW");
					restartNotificationMessage = PurpleI18n.Get("RESTART");
					restartNotificationDoneMessage = PurpleI18n.Get("RESTARTNOW");
				} catch(Exception e){
					notificationPlaceholder = "!!time!!";
					stdServerdelay = 10;

					shutdownNotificationMessage = "Server shutdown in !!time!!!";
					shutdownNotificationDoneMessage = "The server will be shut down now!";
					restartNotificationMessage = "Server restart in !!time!!!";
					restartNotificationDoneMessage = "The server will be restarted now!";

					Debug.LogError("Can not read Purple Config! " + e.ToString());
				}
			}


			// SINGLETON /////////////////////////
			private static PurpleServer Instance
			{
				get
				{
					if (instance == null)
					{
						GameObject gameObject 	= new GameObject ("PurpleServerManager");
						instance     			= gameObject.AddComponent<PurpleServer> ();
						instance.set_notification_interval();
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
				Instance.stop_server ();
			}

			public static void StopServer(string message, string doneMessage)
			{
				Instance.stop_server (message, doneMessage);
			}
			
			public static void StopServer(int seconds)
			{
				Instance.stop_server (seconds);
			}

			public static void StopServer(int seconds, string message, string doneMessage)
			{
				Instance.stop_server (seconds, message, doneMessage);
			}

			
			public static void RestartServer()
			{
				Instance.restart_server ();
			}

			public static void RestartServer(string message, string doneMessage)
			{
				Instance.restart_server (message, doneMessage);
			}
			
			public static void RestartServer(int seconds)
			{
				Instance.restart_server (seconds);
			}

			public static void RestartServer(int seconds, string message, string doneMessage)
			{
				Instance.restart_server (seconds, message, doneMessage);
			}


			public static void SetNotificationInterval(int interval)
			{
				List<int> intervalList = new List<int> ();
				intervalList.Add (interval);
				Instance.set_notification_interval (intervalList);
			}

			public static void SetNotificationKeyword(string keyword)
			{
				Instance.set_notification_keyword (keyword);
			}

			public static void SetNotificationInterval()
			{
				Instance.set_notification_interval ();
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
				launch_server (currentServerConfig);
			}

			private void launch_server(ServerConfig config)
			{
				currentServerConfig = config;
				int player = config.ServerMaxClients;
				string password = config.ServerPassword;
				int port = config.ServerPort;
				PurpleNetwork.LaunchLocalServer(player, password, port);

				switch (config.ServerType)
				{
					case ServerTypes.Account:
						Handler.RegisterAccountListener();
						break;
					
					case ServerTypes.Game:
						Handler.RegisterGameListener();
						break;
						
					case ServerTypes.Lobby:
						Handler.RegisterLobbyListener();
						break;
						
					case ServerTypes.Monitoring:
						Handler.RegisterLMonitoringListener();
						break;
						
					case ServerTypes.Multi:
						Handler.RegisterMultiListener();
						break;
				}
			}


			// STOP
			private void stop_server()
			{
				stop_server (stdServerdelay);
			}

			private void stop_server(string message, string doneMessage)
			{
				stop_server (stdServerdelay, message, doneMessage);
			}

			private void stop_server(int seconds)
			{
				stop_server (seconds, shutdownNotificationMessage, shutdownNotificationDoneMessage);
			}

			private void stop_server(int seconds, string message, string doneMessage)
			{
				shutdownNotificationMessage = message;
				shutdownNotificationDoneMessage = doneMessage;

				PurpleCountdown.CountdownDoneEvent += stop_server_done;
				PurpleCountdown.CountdownRunEvent += stop_server_run;
				PurpleCountdown.Countdown (seconds);
			}

			private void stop_server_run()
			{
				float time_left = PurpleCountdown.CountdownTimeLeft ();

				if(notificationIntervalList.Contains((int)time_left))
				{
					PurpleNetwork.Broadcast ("server_broadcast",
						create_broadcast_message(combine_notification_message(shutdownNotificationMessage, (int)time_left)));
				}
			}

			private void stop_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= stop_server_done;
				PurpleCountdown.CountdownRunEvent -= stop_server_run;
				PurpleNetwork.Broadcast ("server_broadcast", create_broadcast_message(shutdownNotificationDoneMessage));
				PurpleNetwork.StopLocalServer ();
			}


			// RESTART
			private void restart_server()
			{
				restart_server (stdServerdelay);
			}

			private void restart_server(string message, string doneMessage)
			{
				restart_server (stdServerdelay, message, doneMessage);
			}

			private void restart_server(int seconds)
			{
				restart_server (seconds, restartNotificationMessage, restartNotificationDoneMessage);
			}

			private void restart_server(int seconds, string message, string doneMessage)
			{
				restartNotificationMessage = message;
				restartNotificationDoneMessage = doneMessage;

				PurpleCountdown.CountdownDoneEvent += restart_server_done;
				PurpleCountdown.CountdownRunEvent += restart_server_run;
				PurpleCountdown.Countdown (seconds);
			}

			private void restart_server_run()
			{
				float time_left = PurpleCountdown.CountdownTimeLeft ();

				if(notificationIntervalList.Contains((int)time_left))
				{
					PurpleNetwork.Broadcast ("server_broadcast",
						create_broadcast_message(combine_notification_message(restartNotificationMessage, (int)time_left)));
				}
			}

			private void restart_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= restart_server_done;
				PurpleCountdown.CountdownRunEvent -= restart_server_run;
				PurpleNetwork.Broadcast ("server_broadcast", create_broadcast_message(restartNotificationDoneMessage));
				PurpleNetwork.RestartLocalServer ();
			}

			// NOTIFICATION MESSAGE
			private void set_notification_keyword(string keyword)
			{
				notificationPlaceholder = keyword;
			}


			// SET NOTIFICATION INTERVAL
			private void set_notification_interval()
			{
				notificationIntervalList = stdNotificationIntervalList;
			}

			private void set_notification_interval(List <int> interval)
			{
				notificationIntervalList = interval;
			}

			// GET NOTIFICATION INTERVAL
			private List <int> get_notification_interval()
			{
				return notificationIntervalList;
			}


			// HELPER ////////////////////
			private string combine_notification_message(string message, int timeLeft)
			{
				int[] convertedTimeLeft = calculate_time_from_seconds (timeLeft);
				string timeLeftString = "";

				if(convertedTimeLeft[0] != 0)
					timeLeftString += convertedTimeLeft[0] + " " + 
						((convertedTimeLeft[0] == 1) ? PurpleI18n.Get ("day") : PurpleI18n.Get ("days")) + " ";
				if(convertedTimeLeft[1] != 0)
					timeLeftString += convertedTimeLeft[1] + " " + 
						((convertedTimeLeft[1] == 1) ? PurpleI18n.Get ("hour") : PurpleI18n.Get ("hours")) + " ";
				if(convertedTimeLeft[2] != 0)
					timeLeftString += convertedTimeLeft[2] + " " + 
						((convertedTimeLeft[2] == 1) ? PurpleI18n.Get ("minute") : PurpleI18n.Get ("minutes")) + " ";
				if(convertedTimeLeft[3] != 0)
					timeLeftString += convertedTimeLeft[3] + " " + 
						((convertedTimeLeft[3] == 1) ? PurpleI18n.Get ("second") : PurpleI18n.Get ("seconds")) + " ";

				return message.Replace(notificationPlaceholder, timeLeftString.Trim());
			}

			private int[] calculate_time_from_seconds(int timeLeft)
			{
				TimeSpan time = TimeSpan.FromSeconds( timeLeft );
				return new int[] { time.Days, time.Hours, time.Minutes, time.Seconds };
			}

			private PurpleMessages.Server.Message create_broadcast_message(string data)
			{
				PurpleMessages.Server.Message PSM = new PurpleMessages.Server.Message ();
				PSM.message = data;
				return PSM;
			}
		}
	}
}
