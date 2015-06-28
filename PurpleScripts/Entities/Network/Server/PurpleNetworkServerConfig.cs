using System;
using PurpleNetwork;
using Entities.Database;
using PurpleDatabase.Helper;

namespace Entities.PurpleNetwork.Server
{
	public class ServerConfig
	{
		public const string 	CONFIG_FILE_PREFIX 	= "Entities.PurpleNetwork.Server.ServerConfig";

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

		public PurpleServer serverReference;

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

			ServerGUID 		= PurpleConfig.Network.Server.GUID;
			if(string.IsNullOrEmpty (ServerGUID) || ServerGUID.Equals (new Guid ().ToString()))
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

			serverReference = PurpleServerHelper.GetServerReference (ServerGUID);
			if(serverReference != null)
				ServerID = serverReference.id;
			if(serverReference == null)
			{
				serverReference = new PurpleServer ();
				PurpleServerHelper.CreateServerReference (this, 0, string.Empty, string.Empty);
				serverReference = PurpleServerHelper.GetServerReference (ServerGUID);
			}
			if(serverReference != null)
				ServerID = serverReference.id;
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
				serverReference = PurpleServerHelper.GetServerReference (ServerGUID);
				if(serverReference == null)
				{
					serverReference = new PurpleServer ();
					PurpleServerHelper.CreateServerReference (this, 0, string.Empty, string.Empty);
					serverReference = PurpleServerHelper.GetServerReference (ServerGUID);
				}
				ServerID = serverReference.id;
			}
			else
			{
				PurpleServerHelper.UpdateServerReference (this, 0, string.Empty, string.Empty);
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

			this.serverReference = PurpleServerHelper.GetServerReference (this.ServerGUID);
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

	}
}
