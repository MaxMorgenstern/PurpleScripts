using System;
namespace PurpleConfig
{
	// Networking
	public static class Network {
		public static string IP { get { return ItemIds.getConfigString ("Network.IP"); } }
		public static string Host { get { return ItemIds.getConfigString ("Network.Host"); } }
		public static int Port { get { return ItemIds.getConfigInt ("Network.Port"); } }
		public static int MaxPlayer { get { return ItemIds.getConfigInt ("Network.MaxPlayer"); } }
		public static string Password { get { return ItemIds.getConfigString ("Network.Password"); } }
		public static int Pause { get { return ItemIds.getConfigInt ("Network.Pause"); } }

		public static class MasterServer {
			public static string GameName { get { return ItemIds.getConfigString ("Network.MasterServer.GameName"); } }
			public static string GameType { get { return ItemIds.getConfigString ("Network.MasterServer.GameType"); } }
			public static string ServerURL { get { return ItemIds.getConfigString ("Network.MasterServer.URL"); } }
		}
	}

	// Database
	public static class Database {
		public static string IP { get { return ItemIds.getConfigString ("Database.IP"); } }
		public static string Name { get { return ItemIds.getConfigString ("Database.Name"); } }
		public static string User { get { return ItemIds.getConfigString ("Database.User"); } }
		public static string Password { get { return ItemIds.getConfigString ("Database.Password"); } }
	}

	// Password generation/validation
	public static class Password {
		public static int SaltByteSize { get { return ItemIds.getConfigInt ("Password.SaltByteSize"); } }
		public static int HashByteSize { get { return ItemIds.getConfigInt ("Password.HashByteSize"); } }
		public static int IterationIndex { get { return ItemIds.getConfigInt ("Password.IterationIndex"); } }
		public static int SaltIndex { get { return ItemIds.getConfigInt ("Password.SaltIndex"); } }

		public static class PBKDF2 {
			public static int Iterations { get { return ItemIds.getConfigInt ("Password.PBKDF2.Iterations"); } }
			public static int Index { get { return ItemIds.getConfigInt ("Password.PBKDF2.Index"); } }
		}
	}
}
