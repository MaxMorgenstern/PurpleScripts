using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using PurpleNetwork.Server;


namespace PurpleNetwork
{
	public class PurpleNetworkTester : MonoBehaviour
	{
		private static PurpleNetworkTester instance;

		private static string formerIP;
		private static int formerPort;

		private static bool testDone;
		private static ConnectionTesterStatus testResult;

		private static pingData pingObject;

		private static ServerReference currentServerReference;
		private static string currentIP;
		private static int currentPort;


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
		protected PurpleNetworkTester ()
		{
			formerIP = currentIP = Network.connectionTesterIP;
			formerPort = currentPort = Network.connectionTesterPort;

			testDone = true;
			testResult = ConnectionTesterStatus.Undetermined;

			currentServerReference = null;
			pingObject = new pingData ();
		}


		// SINGLETON /////////////////////////
		private static PurpleNetworkTester Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleNetworkTester");
					instance     			= gameObject.AddComponent<PurpleNetworkTester> ();
				}
				return instance;
			}
		}


		// PUBLIC FUNCTIONS /////////////////////////
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
				PurpleCountdown.CountdownRunEvent += test_connection;
				PurpleCountdown.CountdownDoneEvent += reset_connection;
				PurpleCountdown.Countdown (timeout);
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
			Network.connectionTesterIP = currentIP = ipAddress;
			
			formerPort = Network.connectionTesterPort;
			Network.connectionTesterPort = currentPort = port;
		}

		private void test_connection()
		{
			testResult = Network.TestConnection();
			if(testResult != ConnectionTesterStatus.Undetermined)
			{
				PurpleCountdown.CancelCountdown();
				reset_connection();
			}
		}

		private void reset_connection()
		{
			PurpleCountdown.CountdownRunEvent -= test_connection;
			PurpleCountdown.CountdownDoneEvent -= reset_connection;

			testDone = true;

			Network.connectionTesterIP = currentIP = formerIP;
			Network.connectionTesterPort = currentPort = formerPort;
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

	}
}
