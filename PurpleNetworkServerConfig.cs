using System;

namespace PurpleNetwork
{
	namespace Server
	{
		public enum ServerType { Account, Lobby, Game, Multi, Monitoring };
		
		public class ServerConfig
		{
			public string name;
			public ServerType type;
			
			public string password;
			public int maxUser;
			public int port;
			
			// CONSTRUCTOR
			public ServerConfig ()
			{
				name = "GameServer";
				type = ServerType.Multi;
				
				password = "purple";
				maxUser = 32;
				port = 1000;
			}
			
			public void SetType(string serverType)
			{
				SetType(parse_server_type (serverType));
			}
			
			public void SetType(ServerType serverType)
			{
				type = serverType;
			}
			
			// PRIVATE HELPER /////////////////////////
			private ServerType parse_server_type(string serverType)
			{
				return (ServerType) Enum.Parse(typeof(ServerType), serverType, true);
			}
		}
	}
}