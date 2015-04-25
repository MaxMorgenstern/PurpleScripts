using System;
using System.Collections;
using UnityEngine;
using Entities.PurpleNetwork.Client;

// This is just an idea to provide a client and autoritative server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

namespace PurpleNetwork.Client
{
	public class PurpleClient : MonoBehaviour
	{
		private static PurpleClient instance;
		private static ClientConfig currentClientConfig;


		// START UP /////////////////////////
		protected PurpleClient ()
		{
			currentClientConfig = new ClientConfig ();
			currentClientConfig.Load ();
		}


		// SINGLETON /////////////////////////
		private static PurpleClient Instance
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

		public static ClientConfig CurrentConfig
		{
			get
			{
				return Instance.get_currnet_client_config();
			}
		}


		// PUBLIC FUNCTIONS /////////////////////////
		public static void Connect()
		{
			Instance.connect_to_server ();
		}

		public static void Connect(ClientConfig configObject)
		{
			Instance.connect_to_server (configObject);
		}

		public static void SwitchServer(string hostname, string password, int port)
		{
			Instance.switch_server (hostname, password, port);
		}

		public static void SwitchServer(ClientConfig configObject)
		{
			Instance.switch_server (configObject);
		}

		public static void Disconnect()
		{
			Instance.disconnect_from_server ();
		}



		// PRIVATE FUNCTIONS /////////////////////////
		private void connect_to_server()
		{
			connect_to_server (currentClientConfig);
		}

		private void connect_to_server(ClientConfig configObject)
		{
			currentClientConfig = configObject;
			Handler.Base.register_base_handler ();

			PurpleNetwork.ConnectToServer (currentClientConfig.ServerHost, currentClientConfig.ServerPassword,
			                               currentClientConfig.ServerPort);
		}

		// TODO
		// Handler.Base.register_base_handler (); on switch
		private void switch_server(string hostname, string password, int port)
		{
			currentClientConfig.ServerHost = hostname;
			currentClientConfig.ServerPassword = password;
			currentClientConfig.ServerPort = port;
			switch_server (currentClientConfig);
		}

		private void switch_server(ClientConfig configObject)
		{
			currentClientConfig = configObject;
			PurpleNetwork.SwitchServer (currentClientConfig.ServerHost, currentClientConfig.ServerPassword,
			                            currentClientConfig.ServerPort);
		}

		private void disconnect_from_server()
		{
			PurpleNetwork.DisconnectFromServer ();
		}

		private ClientConfig get_currnet_client_config()
		{
			return currentClientConfig;
		}
	}
}
