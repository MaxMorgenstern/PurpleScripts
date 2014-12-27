using UnityEngine;
using System;
using System.Collections.Generic;

// This is just an idea to provide a client and (autoritative) server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO
// event handler
// login
// option autoritative
// Servertype: Account, Lobby, Game, Multi (Account, Lobby, Game), Monitoring

// TODO: i18n

namespace PurpleNetwork
{
	namespace Server
	{
		public class PurpleServer : MonoBehaviour
		{
			private static PurpleServer instance;
			private ServerConfig stdServerConfig;
			private static int stdServerdelay;
			private static List<int> stdNotificationIntervalList;

			private static string notificationMessage;
			private static string notificationDoneMessage;
			private static List <int> notificationIntervalList;
			private static string notificationPlaceholder;

			private static Dictionary<string, string> _language;


			// START UP /////////////////////////
			protected PurpleServer ()
			{
				stdServerConfig = new ServerConfig ();
				stdNotificationIntervalList = new List<int> (
					new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 30, 60, 300, 600, 900, 1800});

				try{
					// TODO
				} catch(Exception e){
					// TODO - fallback
					Debug.LogError("Can not read Purple Config! " + e.ToString());
				}

				// TODO: server config
				stdServerdelay = 10;
				notificationPlaceholder = "!!time!!";
				notificationMessage = "Server Shutdown in !!time!!!";
				notificationDoneMessage = "The Server will be restarted now!";

				//TODO:  Load language externally
				_language = new Dictionary<string, string>();
				_language.Add ("day", "day");
				_language.Add ("days", "days");
				_language.Add ("hour", "hour");
				_language.Add ("hours", "hours");
				_language.Add ("minute", "minute");
				_language.Add ("minutes", "minutes");
				_language.Add ("second", "second");
				_language.Add ("seconds", "seconds");
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

			public static void StopServer(int seconds)
			{
				Instance.stop_server (seconds);
			}


			public static void RestartServer()
			{
				Instance.restart_server ();
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
			private void stop_server()
			{
				stop_server (stdServerdelay);
			}

			private void stop_server(int seconds)
			{
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
						combine_notification_message(notificationMessage, (int)time_left));
				}
			}

			private void stop_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= stop_server_done;
				PurpleCountdown.CountdownRunEvent -= stop_server_run;
				PurpleNetwork.Broadcast ("server_broadcast", notificationDoneMessage);
				PurpleNetwork.StopLocalServer ();
			}


			// RESTART
			private void restart_server()
			{
				restart_server (stdServerdelay);
			}

			private void restart_server(int seconds)
			{
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
						combine_notification_message(notificationMessage, (int)time_left));
				}
			}

			private void restart_server_done()
			{
				PurpleCountdown.CountdownDoneEvent -= restart_server_done;
				PurpleCountdown.CountdownRunEvent -= restart_server_run;
				PurpleNetwork.Broadcast ("server_broadcast", notificationDoneMessage);
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

			private void set_notification_done_message(string message)
			{
				notificationDoneMessage = message;
			}
			
			private string get_notification__done_message()
			{
				return notificationDoneMessage;
			}

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
						((convertedTimeLeft[0] == 1) ? _language["day"] : _language["days"]) + " ";

				if(convertedTimeLeft[1] != 0)
					timeLeftString += convertedTimeLeft[1] + " " + 
						((convertedTimeLeft[1] == 1) ? _language["hour"] : _language["hours"]) + " ";

				if(convertedTimeLeft[2] != 0)
					timeLeftString += convertedTimeLeft[2] + " " + 
						((convertedTimeLeft[2] == 1) ? _language["minute"] : _language["minutes"]) + " ";

				if(convertedTimeLeft[3] != 0)
					timeLeftString += convertedTimeLeft[3] + " " + 
						((convertedTimeLeft[3] == 1) ? _language["second"] : _language["seconds"]) + " ";

				message.Replace(notificationPlaceholder, timeLeftString.Trim());
				return message;
			}

			private static int[] calculate_time_from_seconds(int timeLeft)
			{
				TimeSpan time = TimeSpan.FromSeconds( timeLeft );
				return new int[] { time.Days, time.Hours, time.Minutes, time.Seconds };
			}
		}
	}
}
