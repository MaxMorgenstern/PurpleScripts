using System;
using PurpleNetwork;

namespace Entities.PurpleNetwork.Server
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

		public bool			SanityTest;
		public string		SanityAction;
		public int			SanityPeriodical;

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
			ServerType 		= parse_server_type (PurpleConfig.Network.Server.Type);
			ServerID 		= System.Guid.NewGuid ();

			ServerHost 		= PurpleConfig.Network.Server.Host;
			ServerName 		= PurpleConfig.Network.Server.Name;
			ServerPort 		= PurpleConfig.Network.Server.Port;

			ServerPassword 	= PurpleConfig.Network.Server.Password;
			ServerMaxClients = PurpleConfig.Network.Server.Clients.Max;
			ServerAllowMonitoring = PurpleConfig.Network.Server.AllowMonitoring;

			SanityTest 		= PurpleConfig.Network.Server.Sanity.Test;
			SanityAction	= PurpleConfig.Network.Server.Sanity.Action;
			SanityPeriodical= PurpleConfig.Network.Server.Sanity.Periodical;

			ClientAuthentificationTimeout = PurpleConfig.Network.Server.Clients.AuthentificationTimeout;

			SpamPrevention 	= PurpleConfig.Network.Server.Spam.Prevention;
			SpamResponse 	= PurpleConfig.Network.Server.Spam.Response;

			DatabaseHost 	= PurpleConfig.Database.IP;
			DatabaseName 	= PurpleConfig.Database.Name;
			DatabasePort 	= PurpleConfig.Database.Port;
			DatabaseUser 	= PurpleConfig.Database.User;
			DatabasePassword= PurpleConfig.Database.Password;
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
