using System;

namespace Entities.PurpleNetwork.Client
{
	public class ClientConfig
	{
		public string 		ServerHost;
		public int 			ServerPort;	
		public string 		ServerPassword;

		protected Guid _guid;

		public string guid 
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

		public string 		ClientName;
		public string 		ClientPassword;
		public string 		ClientEmail;
		public string 		ClientToken;
		public DateTime 	ClientTokenCreated;

		public bool 		PlayerAuthenticated;

		// CONSTRUCTOR
		public ClientConfig ()
		{
			ServerHost 		= PurpleConfig.Network.Server.Host;
			ServerPort 		= PurpleConfig.Network.Server.Port;
			ServerPassword 	= PurpleConfig.Network.Server.Password;
		}

		public void Save()
		{
			PurpleStorage.PurpleStorage.Save("Entities.PurpleNetwork.Client.ClientConfig", this);
		}
		
		public void Load()
		{
			ClientConfig config 
				= PurpleStorage.PurpleStorage.Load<ClientConfig> ("Entities.PurpleNetwork.Client.ClientConfig");
			this.ServerHost 		= config.ServerHost;
			this.ServerPort 		= config.ServerPort;
			this.ServerPassword 	= config.ServerPassword;
			this.ClientName 		= config.ClientName;
			this.ClientPassword 	= config.ClientPassword;
			this.ClientEmail 		= config.ClientEmail;
			this.ClientToken 		= config.ClientToken;
			this.ClientTokenCreated = config.ClientTokenCreated;
			this.PlayerAuthenticated= config.PlayerAuthenticated;
			this.guid 				= config.guid;
		}
		


	}
}

