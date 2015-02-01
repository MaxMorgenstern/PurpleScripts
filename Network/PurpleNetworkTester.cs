using UnityEngine;
using System.Collections;
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

		private static ServerReference currentServerReference;
		private static string currentIP;
		private static int currentPort;


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
	}
}
