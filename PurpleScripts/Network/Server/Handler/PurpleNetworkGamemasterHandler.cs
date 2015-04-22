using System;
using System.Collections.Generic;
using UnityEngine;
using PurpleDatabase.Helper;
using _PMGamemaster = Entities.PurpleMessages.Gamemaster;

namespace PurpleNetwork.Server.Handler
{
	public class GameMaster : Shared
	{
		public static void register_gamemaster_handler()
		{
			PurpleNetwork.AddListener("gamemaster_add_warning", gamemaster_add_warning_handler);

			PurpleNetwork.DisconnectedFromPurpleServer += remove_gamemaster_handler;
		}


		// HANDLER /////////////////////////

		// GAME /////////////////////////
		public static void gamemaster_add_warning_handler (string dataObject, NetworkPlayer np)
		{
			Debug.Log ("Gamemaster add warning received: " + np.ToString ());
			_PMGamemaster.Warning accountWarning = PurpleSerializer.StringToObjectConverter<_PMGamemaster.Warning> (dataObject);
			string password_or_token = string.Empty;

			if(!string.IsNullOrEmpty(accountWarning.gmToken))
				password_or_token = accountWarning.gmToken;
			if(!string.IsNullOrEmpty(accountWarning.gmPassword))
				password_or_token = accountWarning.gmPassword;

			accountWarning.validate = AccountHelper.AddWarning (accountWarning.gmUsername, password_or_token,
			                          accountWarning.warningUser, accountWarning.warningComment,
			                          accountWarning.warningLevel);

			AccountHelper.AddLog(get_network_player_reference(np).UserName,
			                     "gamemaster_add_warning_handler " + accountWarning.gmUsername);
			PurpleNetwork.ToPlayer (np, "server_add_warning_result", accountWarning);
		}

		public static void remove_gamemaster_handler(object ob, NetworkPlayer np)
		{
			PurpleNetwork.RemoveListener("gamemaster_add_warning", gamemaster_add_warning_handler);
		}
	}
}
