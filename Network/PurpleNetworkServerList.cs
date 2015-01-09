using System;
using System.Collections.Generic;

using PurpleStorage;

namespace PurpleNetwork
{
	namespace Server
	{
		public class Lists
		{
			private List <ServerReference> serverList;

			public bool Add(ServerReference reference)
			{
				serverList.Add (reference);
				return true;
			}

			public bool Remove(ServerReference reference)
			{
				return serverList.Remove (reference);
			}

			public List <ServerReference> Get()
			{
				return serverList;
			}

			// CONSTRUCTOR
			public Lists()
			{
				serverList = new List<ServerReference> ();
			}
		}

		public class ServerReference
		{
			public string serverName;
			public ServerType type;
			public string password;
			public int maxUser;

			public string ip;
			public int port;
			
			public DateTime firstSeen;
			public DateTime lastSeen;
		}
	}
}
