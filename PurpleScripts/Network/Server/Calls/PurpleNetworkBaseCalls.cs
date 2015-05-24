using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;
using UnityEngine;

namespace PurpleNetwork.Server.Calls
{
	public class Base
	{
		public static void SwitchServer(NetworkPlayer np, string hostname, string password, int port, string token)
		{
			_PMServer.SwitchMessage switchObject = new _PMServer.SwitchMessage ();
			switchObject.Hostname = hostname;
			switchObject.Password = password;
			switchObject.Port = port;
			switchObject.SwitchToken = token;
			PurpleNetwork.ToPlayer (np, "server_switch", switchObject);
		}
	}
}
