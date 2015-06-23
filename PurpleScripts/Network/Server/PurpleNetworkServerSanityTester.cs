using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using PurpleNetwork.Server;
using Entities.PurpleNetwork.Server;
using Entities.Database;
using PurpleDatabase;
using PNS = PurpleNetwork.Server.PurpleServer;


// TODO: Class for monitoring - split it out later on


namespace PurpleNetwork
{
	public class PurpleNetworkServerSanityTester : MonoBehaviour
	{
		private static PurpleNetworkServerSanityTester instance;

		private static string formerIP;
		private static int formerPort;

		private static bool testDone;
		private static ConnectionTesterStatus testResult;

		private static pingData pingObject;

		private static PurpleCountdown countdown;

		private static ServerReference currentServerReference;

		private static PurpleCountdown sanityCountdown;
		private static PurpleCountdown sanityCountdownDone;

		private static bool server_sanity_database;
		private static bool server_sanity_network_ip;
		private static bool server_sanity_network_reachable;

		private class pingData
		{
			public Ping ping;
			public int lastPing;
			public IPAddress IP;
			public string host;
			public bool done;

			public pingData()
			{
				ping = null;
				lastPing = -1;
				IP = null;
				host = String.Empty;
				done = true;
			}
		}

		// START UP /////////////////////////
		protected PurpleNetworkServerSanityTester ()
		{
			formerIP = Network.connectionTesterIP;
			formerPort = Network.connectionTesterPort;

			testDone = true;
			testResult = ConnectionTesterStatus.Undetermined;

			currentServerReference = null;
			pingObject = new pingData ();
		}


		// SINGLETON /////////////////////////
		private static PurpleNetworkServerSanityTester Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleNetworkTester");
					instance     			= gameObject.AddComponent<PurpleNetworkServerSanityTester> ();
				}
				return instance;
			}
		}


		// PUBLIC FUNCTIONS /////////////////////////
		// SERVER SANITY CHECK /////////////////////////
		public static void ServerSanityCheck()
		{
			Instance.server_sanity_check ();
		}


		// SIMPLE PING /////////////////////////
		public static int Ping(IPAddress ipAddress)
		{
			return Instance.ping(ipAddress);
		}

		public static int Ping(string host)
		{
			return Instance.ping(host);
		}


		// ADVANCED FUNCTIONS /////////////////////////
		public static string Run()
		{
			return Instance.run_test (30).ToString();
		}

		public static string Run(string ipAddress, int port)
		{
			return Instance.run_test (ipAddress, port, 30).ToString();
		}

		public static string Run(ServerReference reference)
		{
			return Instance.run_test (reference).ToString();
		}

		public static string Run(string ipAddress, int port, int timeout)
		{
			return Instance.run_test (ipAddress, port, timeout).ToString();
		}

		public static bool IsTestDone
		{
			get
			{
				return Instance.get_test_state();
			}
		}

		public static string TestResult
		{
			get
			{
				return Instance.get_test_result().ToString();
			}
		}

		public static bool TestDatabase(string ip, string externalIp)
		{
			return Instance.test_current_database_connection (ip, externalIp);
		}



		// PRIVATE FUNCTIONS /////////////////////////
		private int ping(IPAddress ipAddress)
		{
			if(pingObject.IP != ipAddress)
			{
				pingObject.IP = ipAddress;
			}
			return ping ();
		}

		private int ping(string host)
		{
			if(pingObject.host != host)
			{
				pingObject.host = host;
				pingObject.IP = Dns.GetHostEntry(host).AddressList.First();
			}
			return ping ();
		}

		private int ping()
		{
			if(pingObject.ping != null && pingObject.ping.isDone)
				pingObject.lastPing = pingObject.ping.time;

			if(pingObject.ping == null || pingObject.ping.isDone)
				pingObject.ping = new Ping (pingObject.IP.ToString());

			return pingObject.lastPing;
		}


		// ADVANCED FUNCTIONS /////////////////////////
		private ConnectionTesterStatus run_test(ServerReference reference)
		{
			if(testDone)
			{
				currentServerReference = reference;
			}
			return run_test (currentServerReference.ServerHost, currentServerReference.ServerPort, currentServerReference.TesterTimeout);
		}

		private ConnectionTesterStatus run_test(string ipAddress, int port, int timeout)
		{
			if(testDone)
			{
				testDone = false;
				init_connection (ipAddress, port);
				countdown = PurpleCountdown.NewInstance ("ConnectionTesterStatus");
				countdown.CountdownRunEvent += test_connection;
				countdown.CountdownDoneEvent += reset_connection;
				countdown.CountDown (timeout);
			}
			return testResult;
		}

		private ConnectionTesterStatus run_test(int timeout)
		{
			if(testDone)
			{
				testDone = false;
				countdown = PurpleCountdown.NewInstance ("ConnectionTesterStatus");
				countdown.CountdownRunEvent += test_connection;
				countdown.CountdownDoneEvent += reset_connection;
				countdown.CountDown (timeout);
			}
			return testResult;
		}


		private bool get_test_state()
		{
			return testDone;
		}

		private ConnectionTesterStatus get_test_result()
		{
			return testResult;
		}

		// PRIVATE HELPER /////////////////////////
		private void init_connection(string ipAddress, int port)
		{
			formerIP = Network.connectionTesterIP;
			Network.connectionTesterIP = ipAddress;

			formerPort = Network.connectionTesterPort;
			Network.connectionTesterPort = port;
		}

		private void test_connection()
		{
			testResult = Network.TestConnection();
			if(testResult != ConnectionTesterStatus.Undetermined)
			{
				countdown.CancelCountDown();
				reset_connection();
			}
		}

		private void reset_connection()
		{
			countdown.CountdownRunEvent -= test_connection;
			countdown.CountdownDoneEvent -= reset_connection;
			countdown.DestroyInstance ();

			testDone = true;

			if(!formerIP.StartsWith("0."))
				Network.connectionTesterIP = formerIP;
			Network.connectionTesterPort = formerPort;
		}



		// UNTESTED FUNCTIONS /////////////////////////
		private bool test_client_server_connecion(ConnectionTesterStatus type1, ConnectionTesterStatus type2)
		{
			if (type1 == ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted &&
				type2 == ConnectionTesterStatus.LimitedNATPunchthroughSymmetric)
				return false;
			else if (type1 == ConnectionTesterStatus.LimitedNATPunchthroughSymmetric &&
					 type2 == ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted)
				return false;
			else if (type1 == ConnectionTesterStatus.LimitedNATPunchthroughSymmetric &&
					 type2 == ConnectionTesterStatus.LimitedNATPunchthroughSymmetric)
				return false;
			return true;
		}


		// TESTER FUNCTIONS /////////////////////////
		// TODO: MOVE
		/*
		public bool Test(List <ServerReference> serverList)
		{
			// check all server availabilities
			bool returnValue = true;
			foreach(ServerReference sr in serverList)
			{
				ServerReference newSR = new ServerReference();
				bool pingReturn = Test(sr, out newSR);
				sr.ReferencePingNote = newSR.ReferencePingNote;
				sr.ServerState = newSR.ServerState;
				sr.ReferenceLastSeen = newSR.ReferenceLastSeen;

				if(returnValue)
					returnValue = pingReturn;
			}
			return returnValue;
		}
		*/
		/*
		public static ServerReference Test(ServerReference reference)
		{
			return Ping (reference.ServerHost);
		}

		public static ServerReference Test(ServerReference reference)
		{
			string pingMessage = String.Empty;
			ServerReference newRefernece = reference;
			bool pingReturn = Ping(reference.ServerHost, out pingMessage);

			newRefernece.ReferencePingNote = pingMessage;
			if(pingReturn)
			{
				newRefernece.ServerState = ServerStates.Online;
				newRefernece.ReferenceLastSeen = DateTime.Now;
			}
			else
			{
				newRefernece.ServerState = ServerStates.Offline;
			}
			return newRefernece;
		}

		public bool Test(ServerReference reference, out string pingMessage)
		{
			return Ping (reference.ServerHost, out pingMessage);
		}
		*/


		private void server_sanity_check()
		{
			server_sanity_database = false;
			server_sanity_network_ip = false;
			server_sanity_network_reachable = false;

			int testTime = 30;

			PurpleDebug.Log("ServerSanityCheck: Start...", 1);
			PurpleDebug.Log("ServerSanityCheck: Initialize Calls...");
			run_test (testTime-2);
			string ipAddress = Network.player.ipAddress;
			string externalIP = Network.player.externalIP;

			PurpleDebug.Log("ServerSanityCheck: Start Database Check...");
			if(test_current_database_connection (ipAddress, externalIP))
			{
				PurpleDebug.Log("ServerSanityCheck: Database OK");
				server_sanity_database = true;
			}
			else
			{
				PurpleDebug.LogError("ServerSanityCheck: Database ERROR", 1);
			}

			PurpleDebug.Log("ServerSanityCheck: Start Network Check...");
			PurpleDebug.Log("ServerSanityCheck: Local IP Address " + ipAddress);
			PurpleDebug.Log("ServerSanityCheck: External IP Address " + externalIP);
			if(!string.IsNullOrEmpty(externalIP)
			   && !externalIP.StartsWith("0.")
			   && !externalIP.StartsWith("127.")
			   && !externalIP.StartsWith("192.")
			   && !externalIP.Contains("UNASSIGNED_SYSTEM_ADDRESS"))
			{
				server_sanity_network_ip = true;
			}

			sanityCountdown = PurpleCountdown.NewInstance ("SanityCheck");
			sanityCountdown.TriggerEvent += server_sanity_check_periodical;
			sanityCountdown.Trigger (5, testTime/10);

			sanityCountdownDone = PurpleCountdown.NewInstance ("SanityCheckKill");
			sanityCountdownDone.CountdownDoneEvent += server_sanity_check_done;
			sanityCountdownDone.CountDown (testTime);
		}

		private void server_sanity_check_periodical()
		{
			if(testDone)
			{
				PurpleDebug.Log("ServerSanityCheck: Network Testresult " + testResult.ToString());
				if(testResult != ConnectionTesterStatus.Error &&
				   testResult != ConnectionTesterStatus.Undetermined)
				{
					server_sanity_network_reachable = true;
				}
				if (!server_sanity_network_ip)
				{
					string externalIP = Network.player.externalIP;
					PurpleDebug.Log("ServerSanityCheck: Re-Test External IP Address " + externalIP);
					if (!string.IsNullOrEmpty(externalIP)
					   && !externalIP.StartsWith("0.")
					   && !externalIP.StartsWith("127.")
					   && !externalIP.StartsWith("192.")
					   && !externalIP.Contains("UNASSIGNED_SYSTEM_ADDRESS"))
					{
						server_sanity_network_ip = true;
					}
					else
					{
						PurpleDebug.Log("ServerSanityCheck: No external IP, check for connected player. Connected: " 
							+ Network.connections.Length);
						if (Network.connections.Length > 0)
						{
							server_sanity_network_ip = true;
						}
					}
				}

				server_sanity_check_done();
			}
			else
			{
				PurpleDebug.Log("ServerSanityCheck: Network Testresult still running...");
			}
		}

		private void server_sanity_check_done()
		{
			sanityCountdown.TriggerEvent -= server_sanity_check_periodical;
			sanityCountdown.CancelTrigger();
			sanityCountdown.DestroyInstance();

			sanityCountdownDone.CountdownDoneEvent -= server_sanity_check_done;
			sanityCountdownDone.CancelCountDown();
			sanityCountdownDone.DestroyInstance();

			if (server_sanity_database && server_sanity_network_ip && server_sanity_network_reachable)
			{
				PurpleDebug.Log("ServerSanityCheck: Success!", 1);
			}
			else if (server_sanity_database && server_sanity_network_reachable)
			{
				PurpleDebug.LogWarning("ServerSanityCheck: Partly Success! No external IP found.", 1);
			}
			else
			{
				PurpleDebug.LogError("ServerSanityCheck: ERROR!", 1);
				if(!server_sanity_database)
					PurpleDebug.LogError("ServerSanityCheck: Database unreachable!", 1);

				if(!server_sanity_network_ip)
					PurpleDebug.LogError("ServerSanityCheck: No external IP!", 1);

				if(!server_sanity_network_reachable)
					PurpleDebug.LogError("ServerSanityCheck: Network Error!", 1);

				PurpleDebug.LogError("ServerSanityCheck: Shut down server...", 1);
				if(PNS.CurrentConfig.SanityTest)
				{
					switch (PNS.CurrentConfig.SanityAction.ToLower())
					{
					case "shutdown":
						PNS.StopServer(5);
						break;

					case "restart":
						PNS.RestartServer(5);
						break;

					case "warn":
						PurpleDebug.LogError("ServerSanityCheck: Shutdown/Restart recommended!", 1);
						break;
					}
				}
			}
		}

		private bool test_current_database_connection(string ip, string externalIp)
		{
			Entities.Database.PurpleServerLog psl = new Entities.Database.PurpleServerLog ();
			psl.name = PNS.CurrentConfig.ServerName;
			psl.port = PNS.CurrentConfig.ServerPort;
			psl.max_player = PNS.CurrentConfig.ServerMaxClients;
			psl.host = PNS.CurrentConfig.ServerHost;
			psl.comment = "PurpleNetworkServerSanityTester: Test";
			psl.type = PNS.CurrentConfig.ServerType.ToString();
			psl.local_ip = ip;
			psl.global_ip = externalIp;
			psl.server_id = PNS.CurrentConfig.ServerID;

			int insertResult = psl.ToSQLInsert ().Execute ();
			if (insertResult == 1 && PurpleDatabase.PurpleDatabase.LastInsertedId() > 0)
				return true;
			return false;
		}
	}
}
