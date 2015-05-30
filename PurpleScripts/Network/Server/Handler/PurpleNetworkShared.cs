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

		public static string get_token_or_password(Entities.PurpleMessages.User.Authentication authObject)
		{
			if (!string.IsNullOrEmpty(authObject.ClientPassword))
				return authObject.ClientPassword;
			if (!string.IsNullOrEmpty(authObject.ClientToken))
				return authObject.ClientToken;
			return string.Empty;
		}
	}
}

