using System;
using PurpleNetwork;
using PurpleDatabase;
using PurpleDatabase.Extension;
using Entities.Database;

namespace Entities.PurpleNetwork.Server
{
	public class ServerConfig
	{
		public const string 	CONFIG_FILE_PREFIX 	= "Entities.PurpleNetwork.Server.ServerConfig";
		public const string 	SERVER_TABLE 	= "server";

		public ServerTypes 	ServerType;
		protected Guid 		_guid;

		public int 			ServerID;
		public string 		ServerGUID
		{
			get
			{
				return _guid.ToString();
			}

			set
			{
				_guid = new Guid(value);
			}
		}

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

		public bool			ConfigLoaded;

		private PurpleServer serverReference;

		// CONSTRUCTOR
		public ServerConfig ()
		{
			Load ();
			if(!ConfigLoaded)
			{
				Reset ();
			}
		}

		public void SetType(string serverType)
		{
			SetType(parse_server_type (serverType));
		}

		public void SetType(ServerTypes serverType)
		{
			ServerType = serverType;
		}

		public void Reset ()
		{
			ServerType 		= parse_server_type (PurpleConfig.Network.Server.Type);
			_guid 			= Guid.NewGuid ();
			ServerID 		= -1;

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

			ConfigLoaded = false;

			get_server_reference ();
			if(serverReference == null)
			{
				create_server_reference ();
				get_server_reference ();
			}
		}

		public void Save()
		{
			Save (string.Empty);
		}

		public void Save(string Name)
		{
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			if(ServerID <= 0)
			{
				get_server_reference ();
				if(serverReference == null)
				{
					create_server_reference ();
					get_server_reference ();
				}
				ServerID = serverReference.id;
			}
			else
			{
				update_server_reference ();
			}
			PurpleStorage.PurpleStorage.Save(CONFIG_FILE_PREFIX+suffix, this);
		}

		public void Load()
		{
			Load (string.Empty);	
		}

		public void Load(string Name)
		{
			
			this.ConfigLoaded = false;
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			ServerConfig config
				= PurpleStorage.PurpleStorage.Load<ServerConfig> (CONFIG_FILE_PREFIX+suffix);
			if (config == null || config.ServerGUID == Guid.Empty.ToString ())
				return;
			
			this.ConfigLoaded 		= true;
			this.ServerType 		= config.ServerType;
			this.ServerID 			= config.ServerID;
			this.ServerGUID 		= config.ServerGUID;

			this.ServerHost 		= config.ServerHost;
			this.ServerName 		= config.ServerName;
			this.ServerPort 		= config.ServerPort;

			this.ServerPassword 	= config.ServerPassword;
			this.ServerMaxClients 	= config.ServerMaxClients;
			this.ServerAllowMonitoring = config.ServerAllowMonitoring;

			this.SanityTest 		= config.SanityTest;
			this.SanityAction 		= config.SanityAction;
			this.SanityPeriodical	= config.SanityPeriodical;

			this.ClientAuthentificationTimeout = config.ClientAuthentificationTimeout;

			this.SpamPrevention 	= config.SpamPrevention;
			this.SpamResponse 		= config.SpamResponse;

			this.DatabaseHost 		= config.DatabaseHost;
			this.DatabaseName 		= config.DatabaseName;
			this.DatabasePort 		= config.DatabasePort;
			this.DatabaseUser 		= config.DatabaseUser;
			this.DatabasePassword 	= config.DatabasePassword;
		}

		public void Delete()
		{
			Delete (string.Empty);
		}

		public void Delete(string Name)
		{
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			PurpleStorage.PurpleStorage.DeleteFile (CONFIG_FILE_PREFIX+suffix);
		}


		// PRIVATE HELPER /////////////////////////
		private ServerTypes parse_server_type(string serverType)
		{
			return (ServerTypes) Enum.Parse(typeof(ServerTypes), serverType, true);
		}


		private void get_server_reference()
		{
			get_server_reference (ServerGUID);
		}

		private void get_server_reference(string guid)
		{
			serverReference = SQLGenerator.New ().Select ("*").From(SERVER_TABLE)
				.Where ("guid="+guid).FetchSingle ().ToObject<PurpleServer> ();
			ServerID = serverReference.id;
		}

		private bool create_server_reference()
		{			
			update_server_reference_helper ();

			int result = serverReference.ToSQLInsert ().Execute ();
			return (result==1) ? true : false;
		}

		private bool update_server_reference()
		{
			get_server_reference ();
			update_server_reference_helper ();

			int result = serverReference.ToSQLUpdate ().Execute ();
			return (result==1) ? true : false;
		}

		private void update_server_reference_helper()
		{
			serverReference.guid = ServerGUID;
			serverReference.host = ServerHost;
			serverReference.max_player = ServerMaxClients;
			serverReference.name = ServerName;
			serverReference.port = ServerPort;
			serverReference.type = ServerType.ToString ();
		}
	}
}
