using UnityEngine;
namespace PurpleNetwork
{
	public enum UserTypes { User, Moderator, GameMaster, Monitoring };
	// User: Standard user
	// Moderator: User with additional rights
	// GameMaster: Administrator user
	// Monitoring: User instance of monitoring server
	public enum ServerStates { Online, Offline, Unknown };
	public enum ServerTypes { Account, Lobby, Game, Multi, Monitoring };
	// Account: Pure account data, handles login and sends the player to a lobby server
	// Lobby: List with games, Account Managements
	// Game: The actual Game server that runs the game instance
	// Multi: All above
	// Monitoring: A Monitoring Server that checks all of the above


	// DELEGATES FOR CALLBACK AND EVENTS
	public delegate void PurpleNetCallback(string converted_object, NetworkPlayer network_info); // Listener
	public delegate void PurpleNetworkEvent(object passed_object, NetworkPlayer network_info); // network event

	public delegate void PurpleNetworkClientEvent(object passed_object); // network event


	public class Constants
	{
		public const string 	SERVER_ID_STRING 	= "-1";
		public const int 		SERVER_ID_INT 		= -1;
	}
}
