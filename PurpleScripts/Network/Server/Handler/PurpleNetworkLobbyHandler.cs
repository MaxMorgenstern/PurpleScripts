using System;
using System.Collections.Generic;
using UnityEngine;
using _PurpleMessages = Entities.PurpleMessages;

namespace PurpleNetwork.Server.Handler
{
	public class Lobby
	{
		public static void register_lobby_handler()
		{
			PurpleNetwork.AddListener("lobby_create_character", lobby_create_character_handler);
			PurpleNetwork.AddListener("lobby_create_game", lobby_create_game_handler);
		}


		// HANDLER /////////////////////////

		// LOBBY /////////////////////////
		public static void lobby_create_game_handler (string dataObject, NetworkPlayer np)
		{ /* TODO */ }

		public static void lobby_create_character_handler (string dataObject, NetworkPlayer np)
		{ /* TODO */ }

	}
}
