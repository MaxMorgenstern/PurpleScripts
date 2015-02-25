using System;

namespace PurpleNetwork.Server
{
	public class ServerConfig
	{
		public ServerTypes 	ServerType;
		public Guid			ServerID;
		
		public string 		ServerHost;
		public string 		ServerName;
		public int 			ServerPort;

		public string 		ServerPassword;
		public int 			ServerMaxClients;
		public bool 		ServerAllowMonitoring;

		public int			ClientAuthentificationTimeout;
		
		public bool 		SpamPrevention;
		public bool 		SpamResponse;
		
		public string 		DatabaseHost;
		public string 		DatabaseName;
		public int 			DatabasePort;
		public string 		DatabaseUser;
		public string 		DatabasePassword;

		// CONSTRUCTOR
		public ServerConfig ()
		{
			ServerType 		= ServerTypes.Multi;
			ServerID 		= System.Guid.NewGuid ();

			ServerHost 		= "localhost";
			ServerName 		= "PurpleServer";
			ServerPort 		= 25001;
			
			ServerPassword 	= String.Empty;
			ServerMaxClients= 32;
			ServerAllowMonitoring = true;

			ClientAuthentificationTimeout = 20;

			SpamPrevention 	= true;
			SpamResponse 	= false;

			DatabaseHost 	= "localhost";
			DatabaseName 	= "PurpleDatabase";
			DatabasePort 	= 3306;
			DatabaseUser 	= "root";
			DatabasePassword= String.Empty;
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
}
