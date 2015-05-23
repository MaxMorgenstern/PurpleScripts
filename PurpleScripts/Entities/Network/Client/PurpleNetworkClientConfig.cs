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

		public bool			ConfigLoaded;

		// CONSTRUCTOR
		public ClientConfig ()
		{
			ServerHost 		= PurpleConfig.Network.Server.Host;
			ServerPort 		= PurpleConfig.Network.Server.Port;
			ServerPassword 	= PurpleConfig.Network.Server.Password;
			ConfigLoaded	= false;
		}

		public void Save()
		{
			Save (string.Empty);
		}

		public void Save(string Name)
		{
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			_guid = Guid.NewGuid ();
			PurpleStorage.PurpleStorage.Save("Entities.PurpleNetwork.Client.ClientConfig"+suffix, this);
		}

		public void Load()
		{
			Load (string.Empty);	
		}

		public void Load(string Name)
		{
			this.ConfigLoaded = false;
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			ClientConfig config
				= PurpleStorage.PurpleStorage.Load<ClientConfig> ("Entities.PurpleNetwork.Client.ClientConfig"+suffix);
			if (config == null || config.guid == Guid.Empty.ToString ())
				return;
			
			this.ConfigLoaded = true;
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
		
		public void Delete()
		{
			Delete (string.Empty);
		}

		public void Delete(string Name)
		{
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			PurpleStorage.PurpleStorage.DeleteFile ("Entities.PurpleNetwork.Client.ClientConfig"+suffix);
		}
	}
}
