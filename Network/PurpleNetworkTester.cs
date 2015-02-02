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
					instance.init();
				}
				return instance;
			}
		}


		// PUBLIC FUNCTIONS /////////////////////////
		public static string Run(string ip, int port)
		{
			return Instance.run_test (ip, port, 30).ToString();
		}

		public static string Run(ServerReference reference)
		{
			return Instance.run_test (reference).ToString();
		}

		public static string Run(string ip, int port, int timeout)
		{
			return Instance.run_test (ip, port, timeout).ToString();
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
		private ConnectionTesterStatus run_test(ServerReference reference)
		{
			if(testDone)
			{
				currentServerReference = reference;
			}
			return run_test (currentServerReference.ServerHost, currentServerReference.ServerPort, currentServerReference.TesterTimeout);
		}

		private ConnectionTesterStatus run_test(string ip, int port, int timeout)
		{
			if(testDone)
			{
				testDone = false;
				init_connection (ip, port);
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
		private void init_connection(string ip, int port)
		{
			formerIP = Network.connectionTesterIP;
			Network.connectionTesterIP = currentIP = ip;
			
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

		private void init()
		{
			pingObject = new pingData ();
		}	



		public static int Ping(string host)
		{
			return Instance.ping(host);
		}


		private int ping(IPAddress host)
		{
			if(pingObject.IP != host)
			{
				pingObject.IP = host;
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

		/*
		private string ping(string host)
		{
			string pingMessage = String.Empty;			
			IPAddress address = Dns.GetHostEntry(host).AddressList.First();
			System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping ();
			
			PingOptions pingOptions = new PingOptions ();
			pingOptions.DontFragment = true;
			
			byte[] buffer = new byte[32];
			
			for (int i = 0; i < 4; i++)
			{
				try
				{
					PingReply pingReply = ping.Send(address, 1000, buffer, pingOptions);
					if (!(pingReply == null))
					{
						switch (pingReply.Status)
						{
						case IPStatus.Success:
							pingMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
							break;
						case IPStatus.TimedOut:
							pingMessage = "Connection has timed out...";
							break;
						default:
							pingMessage = string.Format("Ping failed: {0}", pingReply.Status.ToString());
							break;
						}
					}
					else
					{
						pingMessage = "Connection failed for an unknown reason...";
					}
				}
				catch (PingException ex)
				{
					pingMessage = string.Format("Connection Error: {0}", ex.Message);
				}
				catch (Exception ex)
				{
					pingMessage = string.Format("Connection Error: {0}", ex.Message);
				}
			}
			return pingMessage;
		}
		*/



	}
}
