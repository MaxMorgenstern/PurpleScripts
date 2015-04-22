using System;
using System.Collections.Generic;
using UnityEngine;
using PurpleDatabase.Helper;
using _PMBasic = Entities.PurpleMessages;
using _PMClient = Entities.PurpleMessages.User;
using _PMServer = Entities.PurpleMessages.Server;

namespace PurpleNetwork.Server.Handler
{
	public class Account : Shared
	{
		public static void register_account_handler()
		{
			PurpleNetwork.AddListener("client_validate_username", client_validate_username_handler);
			PurpleNetwork.AddListener("client_register", client_register_handler);
			PurpleNetwork.AddListener("client_disable", client_disable_handler);

			PurpleNetwork.AddListener("client_create_character", client_create_character_handler);

			PurpleNetwork.DisconnectedFromPurpleServer += remove_account_handler;
		}


		// HANDLER /////////////////////////

		// ACCOUNT /////////////////////////
		public static void client_validate_username_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Username validation received: " + np.ToString ());
			_PMBasic.Data basicData = PurpleSerializer.StringToObjectConverter<_PMBasic.Data> (dataObject);
			basicData.validate = AccountHelper.IsUniqueUsername (basicData.data);
			PurpleNetwork.ToPlayer (np, "server_validate_username_result", basicData);
		}


		public static void client_register_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Registration received: " + np.ToString ());
			_PMClient.CreateAccount accountData = PurpleSerializer.StringToObjectConverter<_PMClient.CreateAccount> (dataObject);
			Entities.Database.PurpleAccount purpleAccount = new Entities.Database.PurpleAccount ();

			purpleAccount.birthday		= accountData.ClientBirthday;
			purpleAccount.country_code	= accountData.ClientCountry;
			purpleAccount.email 		= accountData.ClientEmail;
			purpleAccount.first_name 	= accountData.ClientFirstName;
			purpleAccount.gender 		= accountData.ClientGender;
			purpleAccount.language_code = accountData.ClientLanguage;
			purpleAccount.last_name 	= accountData.ClientLastName;
			purpleAccount.username 		= accountData.ClientName;

			// TODO: test
			accountData.validate = AccountHelper.Register (purpleAccount, accountData.ClientPassword);
			accountData.error = AccountHelper.GetErrorList();

			AccountHelper.AddLog(get_network_player_reference(np).UserName,
			                     "client_register_handler " + accountData.ClientName + " - " + accountData.validate.ToString());
			PurpleNetwork.ToPlayer (np, "server_register_result", accountData);
		}

		//TODO: test
		public static void client_disable_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Authentication received: " + np.ToString ());
			_PMClient.Authentication authObject = PurpleSerializer.StringToObjectConverter<_PMClient.Authentication> (dataObject);
			_PMBasic.Boolean returnData = new _PMBasic.Boolean ();
			returnData.value = AccountHelper.Disable (authObject.ClientName, authObject.ClientPassword, np);

			AccountHelper.AddLog(get_network_player_reference(np).UserName,
			                     "client_disable_handler " + authObject.ClientName + " - " + returnData.value.ToString());
			PurpleNetwork.ToPlayer (np, "server_disable_result", returnData);
		}

		// TODO: implement
		public static void client_create_character_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Create Character received: " + np.ToString () + " | " + dataObject);
			if(np.ToString() == Constants.SERVER_ID_STRING && Network.isServer) return;
			// TODO: convert data object to account object - purple messages
		}


		public static void remove_account_handler(object ob, NetworkPlayer np)
		{
			PurpleNetwork.RemoveListener("client_validate_username", client_validate_username_handler);
			PurpleNetwork.RemoveListener("client_register", client_register_handler);
			PurpleNetwork.RemoveListener("client_disable", client_disable_handler);

			PurpleNetwork.RemoveListener("client_create_character", client_create_character_handler);

			PurpleNetwork.DisconnectedFromPurpleServer -= remove_account_handler;
		}
	}
}
