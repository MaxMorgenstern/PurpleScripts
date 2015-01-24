using System;

namespace PurpleNetwork.Server
{
	public enum ServerTypes { Account, Lobby, Game, Multi, Monitoring };
	// Account: Pure account data, handles login and sends the player to a lobby server
	// Lobby: List with games, Account Managements
	// Game: The actual Game server that runs the game instance
	// Multi: All above
	// Monitoring: A Monitoring Server that checks all of the above
	
	public class ServerConfig
	{
		public ServerTypes 	ServerType;
		public Guid			ServerID;
		
		public string 		ServerHost;
		public string 		ServerName;
		public string 		ServerPassword;
		public int 			ServerMaxClients;
		public int 			ServerPort;
		
		public bool 		SpamPrevention;
		public bool 		SpamResponse;
		
		public string 		DatabaseHost;
		public string 		DatabaseName;
		public string 		DatabaseUser;
		public string 		DatabasePassword;
		public int 			DatabasePort;

		// CONSTRUCTOR
		public ServerConfig ()
		{
			ServerID = System.Guid.NewGuid ();

			ServerHost 		= "localhost";
			ServerName 		= "PurpleServer";
			ServerType 		= ServerTypes.Multi;
			
			ServerPassword 	= String.Empty;
			ServerMaxClients= 32;
			ServerPort 		= 25001;

			SpamPrevention 	= true;
			SpamResponse 	= false;

			DatabaseHost 	= "localhost";
			DatabaseName 	= "PurpleDatabase";
			DatabaseUser 	= "root";
			DatabasePassword= String.Empty;
			DatabasePort 	= 3306;
		}
		
		public void SetType(string serverType)
		{
			SetType(parse_server_type (serverType));
		}
		
		public void SetType(ServerTypes serverType)
		{
			ServerType = serverType;
		}
		
		// PRIVATE HELPER /////////////////////////
		private ServerTypes parse_server_type(string serverType)
		{
			return (ServerTypes) Enum.Parse(typeof(ServerTypes), serverType, true);
		}
	}

	// Class Extension /////////////////////////
	public enum ServerStates { Online, Offline, Unknown };

	public class ServerReference : ServerConfig
	{
		public DateTime 	ReferenceLastSeen;
		public DateTime 	ReferenceFirstSeen;
		public int 			ReferencePing;

		public int 			ServerConnectedClients;
		public int 			ServerPriority;
		public ServerStates	ServerState;

		public ServerReference()
		{
			ReferenceLastSeen = DateTime.MinValue;
			ReferenceFirstSeen = DateTime.MinValue;
			ReferencePing = -1;

			ServerConnectedClients = 0;
			ServerPriority = 5;
			ServerState = ServerStates.Unknown;
		}
	}
}