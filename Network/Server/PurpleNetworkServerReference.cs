using System;

namespace PurpleNetwork.Server
{
	// Class Extension /////////////////////////
	public class ServerReference : ServerConfig
	{
		public DateTime 	ReferenceLastSeen;
		public DateTime 	ReferenceFirstSeen;
		public int 			ReferencePing;
		public string		ReferencePingNote;

		public int			TesterTimeout;
		public string		TesterState;

		public string 		ServerNote;
		public int 			ServerConnectedClients;
		public int 			ServerPriority;
		public ServerStates	ServerState;

		public ServerReference()
		{
			ReferenceLastSeen = DateTime.MinValue;
			ReferenceFirstSeen = DateTime.MinValue;
			ReferencePing = -1;
			ReferencePingNote = String.Empty;

			TesterTimeout = 20;
			TesterState = String.Empty;
			ServerNote = String.Empty;

			ServerConnectedClients = 0;
			ServerPriority = 5;
			ServerState = ServerStates.Unknown;
		}
	}
}
