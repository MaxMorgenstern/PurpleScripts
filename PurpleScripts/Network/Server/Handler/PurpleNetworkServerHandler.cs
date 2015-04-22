using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleNetwork.Server.Handler
{
	public class Handler
	{
		public static void RegisterAccountListener()
		{
			Base.register_base_handler ();
			GameMaster.register_gamemaster_handler ();

			Account.register_account_handler ();
		}

		public static void RegisterLobbyListener()
		{
			Base.register_base_handler ();
			GameMaster.register_gamemaster_handler ();

			Lobby.register_lobby_handler ();
		}

		public static void RegisterGameListener()
		{
			Base.register_base_handler ();
			GameMaster.register_gamemaster_handler ();

			Game.register_game_handler ();
		}

		public static void RegisterMultiListener()
		{
			Base.register_base_handler ();
			GameMaster.register_gamemaster_handler ();

			Account.register_account_handler ();
			Lobby.register_lobby_handler ();
			Game.register_game_handler ();
		}

		public static void RegisterLMonitoringListener()
		{
			Base.register_base_handler ();
			GameMaster.register_gamemaster_handler ();

			Monitor.register_monitoring_handler ();
		}
	}
}
