using System;

namespace PurpleNetwork
{
	namespace Server
	{
		public enum ServerType { Account, Lobby, Game, Multi, Monitoring };
		// Account: Pure account data, handles login and sends the player to a lobby server
		// Lobby: List with games, Account Managements
		// Game: The actual Game server that runs the game instance
		// Multi: All above
		// Monitoring: A Monitoring Server that checks all of the above
		
		public class ServerConfig
		{
			public ServerType 	ServerType;
			public Guid			ServerID;
			
			public string 		ServerHost;
			public string 		ServerName;
			public string 		ServerPassword;
			public int 			ServerMaxClients;
			public int 			ServerPort;
			
			public string 		DatabaseHost;
			public string 		DatabaseName;
			public string 		DatabaseUser;
			public string 		DatabasePassword;
			//public int 		DatabasePort;


			// CONSTRUCTOR
			public ServerConfig ()
			{
				ServerID = System.Guid.NewGuid ();

				ServerHost 		= "localhost";
				ServerName 		= "GameServer";
				ServerType 		= ServerType.Multi;
				
				ServerPassword 	= "";
				ServerMaxClients= 32;
				ServerPort 		= 25001;

				DatabaseHost 	= "localhost";
				DatabaseName 	= "PurpleDatabase";
				DatabaseUser 	= "root";
				DatabasePassword= "";
			}
			
			public void SetType(string serverType)
			{
				SetType(parse_server_type (serverType));
			}
			
			public void SetType(ServerType serverType)
			{
				ServerType = serverType;
			}
			
			// PRIVATE HELPER /////////////////////////
			private ServerType parse_server_type(string serverType)
			{
				return (ServerType) Enum.Parse(typeof(ServerType), serverType, true);
			}
		}
	}
}