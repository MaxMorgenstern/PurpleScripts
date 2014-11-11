using UnityEngine;
using System.Collections;

// This is just an idea to provide a client and autoritative server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO: A lot
// TODO: Split in Client and Server file


namespace PurpleNetwork
{
	public class PurpleServer : MonoBehaviour
	{
		// Game List - or game instance list
		// CreateGame(name, password, player, game, version, options, public);

		// event handler

		// interval update

		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
	}
	
	public class PurpleClient : MonoBehaviour
	{
		// ConnectToAccountServer(version)
		// ConnectToLobbyServer(version)
		// JoinRandomGame();
		// JoinGame(name, password);
		// GetData() GetRoomData() GetLobbyData()
		
		// GetGameList()
		// GetGameList(filter)

		// Login()
		// LoginWithFacebook()
		// LoginWithGoogle()

		// Chat()

		// event handler
		
		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
	}
}
