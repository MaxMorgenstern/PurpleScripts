using UnityEngine;
using Entities.Database;
using PurpleDatabase.Extension;
using PNS = PurpleNetwork.Server.PurpleServer;

namespace PurpleDatabase.Helper
{
	public class PurpleServerHelper : MonoBehaviour
	{
		public const string 	SERVER_TABLE 	= "server";
		private static PurpleServer serverReference = new PurpleServer();


		public static PurpleServer GetServerReference()
		{
			return GetServerReference (PNS.CurrentConfig.ServerGUID);
		}

		public static PurpleServer GetServerReference(string guid)
		{
			return SQLGenerator.New ().Select ("*").From(SERVER_TABLE)
				.Where ("guid="+guid).FetchSingle ().ToObject<PurpleServer> ();
		}


		public static bool CreateServerReference()
		{
			return CreateServerReference (PNS.CurrentConfig,
				Network.connections.Length, Network.player.externalIP,
				Network.player.ipAddress);
		}

		public static bool CreateServerReference(Entities.PurpleNetwork.Server.ServerConfig conf, 
				int current_player, string ip, string local_ip)
		{			
			serverReference = new PurpleServer ();
			update_server_reference_helper (conf, current_player, ip, local_ip);

			if (GetServerReference (serverReference.guid) != null)
				return false;

			int result = serverReference.ToSQLInsert ().Execute ();
			return (result==1) ? true : false;
		}


		public static bool UpdateServerReference()
		{
			return UpdateServerReference (PNS.CurrentConfig,
				Network.connections.Length, Network.player.externalIP,
				Network.player.ipAddress);
		}

		public static bool UpdateServerReference(Entities.PurpleNetwork.Server.ServerConfig conf, 
				int current_player, string ip, string local_ip)
		{
			serverReference = GetServerReference ();
			update_server_reference_helper (conf, current_player, ip, local_ip);

			int result = serverReference.ToSQLUpdate ().Execute ();
			return (result==1) ? true : false;
		}


		// PRIVATE /////////////////////////

		private static void update_server_reference_helper(Entities.PurpleNetwork.Server.ServerConfig conf, 
				int current_player, string ip, string local_ip)
		{
			if ( current_player > 0)
				serverReference.currnet_player = current_player;
			if (!string.IsNullOrEmpty (ip))
				serverReference.global_ip = ip;
			if (!string.IsNullOrEmpty (local_ip))
				serverReference.local_ip = local_ip;
			
			if (!string.IsNullOrEmpty (conf.ServerGUID))
				serverReference.guid = conf.ServerGUID;
			
			serverReference.host = conf.ServerHost;
			serverReference.max_player = conf.ServerMaxClients;
			serverReference.name = conf.ServerName;
			serverReference.port = conf.ServerPort;
			serverReference.type = conf.ServerType.ToString ();
		}
	}
}
