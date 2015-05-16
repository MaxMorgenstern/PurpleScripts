using System;
using Entities.PurpleNetwork.Client;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Client.Calls
{
	public class Base
	{
		public static void Authenticate(string Username, string Password, string Token)
		{
			authentication_call(Username, Password, Token, "client_authenticate");
		}

		public static void Authenticate(ClientConfig config)
		{
			authentication_call(config, "client_authenticate");
		}

		public static void GenerateToken(string Username, string Password, string Token)
		{
			authentication_call(Username, Password, Token, "client_generate_token");
		}

		public static void GenerateToken(ClientConfig config)
		{
			authentication_call(config, "client_generate_token");
		}

		public static void Logout(string Username, string Password, string Token)
		{
			authentication_call(Username, Password, Token, "client_logout");
		}

		public static void Logout(ClientConfig config)
		{
			authentication_call(config, "client_logout");
		}

		public static void Ping()
		{
			_PMServer.Ping pingObject = new _PMServer.Ping();
			pingObject.triggerTime = DateTime.Now;
			PurpleNetwork.ToServer ("client_ping", pingObject);
		}

		public static void GetVersion()
		{
			PurpleNetwork.ToServer ("client_get_version", null);
		}

		// PRIVATE /////////////////////////

		private static void authentication_call(string Username, string Password, string Token, string eventName)
		{
			_PMClient.Authentication authObject = new _PMClient.Authentication ();
			authObject.ClientName = Username;
			authObject.ClientPassword = Password;
			authObject.ClientToken = Token;
			authObject.ClientAuthenticated = false;

			PurpleNetwork.ToServer (eventName, authObject);
		}

		private static void authentication_call(ClientConfig config, string eventName)
		{
			_PMClient.Authentication authObject = new _PMClient.Authentication ();
			authObject.ClientName = config.ClientName;
			authObject.ClientPassword = config.ClientPassword;
			authObject.ClientToken = config.ClientToken;
			authObject.ClientAuthenticated = false;

			PurpleNetwork.ToServer (eventName, authObject);
		}
	}
}
