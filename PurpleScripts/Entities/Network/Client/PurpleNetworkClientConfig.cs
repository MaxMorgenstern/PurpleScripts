using System;

namespace Entities.PurpleNetwork.Client
{
	public class ClientConfig
	{
		public const string 	CONFIG_FILE_PREFIX 	= "Entities.PurpleNetwork.Client.ClientConfig";

		public string 		ServerHost;
		public int 			ServerPort;
		public string 		ServerPassword;

		public string 		ServerSwitchToken;
		protected Guid 		_guid;

		public string 		guid
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
			ServerSwitchToken = string.Empty;
		}

		public void ResetServer ()
		{
			ServerHost 		= PurpleConfig.Network.Server.Host;
			ServerPort 		= PurpleConfig.Network.Server.Port;
			ServerPassword 	= PurpleConfig.Network.Server.Password;
			ServerSwitchToken = string.Empty;
		}

		public void Save()
		{
			Save (string.Empty);
		}

		public void Save(string Name)
		{
			string suffix = (!string.IsNullOrEmpty (Name)) ? "." + Name : string.Empty;
			_guid = Guid.NewGuid ();
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
			ClientConfig config
				= PurpleStorage.PurpleStorage.Load<ClientConfig> (CONFIG_FILE_PREFIX+suffix);
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
			this.guid 				= config.guid;
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
	}
}
