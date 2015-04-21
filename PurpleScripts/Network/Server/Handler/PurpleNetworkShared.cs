using UnityEngine;
using Entities.PurpleNetwork;

namespace PurpleNetwork.Server.Handler
{
	public class Shared
	{
		public static PurpleNetworkUser get_network_player_reference(NetworkPlayer np)
		{
			return PurpleServer.UserList.Find (x => x.UserReference == np);
		}
	}

}

