using System;
using _PMBasic = Entities.PurpleMessages;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Client.Calls
{
	public class Account
	{
		public static void ValidateUsername()
		{
			// client_validate_username
			// _PMBasic.Data
		}

		public static void RegisterAccount()
		{
			// client_register
			// _PMClient.CreateAccount
		}

		public static void DisableAccount()
		{
			// client_disable
			// _PMClient.Authentication
		}
	}
}

