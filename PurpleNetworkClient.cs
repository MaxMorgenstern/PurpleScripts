using UnityEngine;
using System;
using System.Collections;

// This is just an idea to provide a client and autoritative server class
// This class is not optimized for games with a lot of server interaction but for
// 		games that are turn based or need less network traffic

// TODO: A lot

/*
 * Clent:
 * 	Connect, Disconnect
 * 	...
 */

namespace PurpleNetwork
{
	public class PurpleClient : MonoBehaviour
	{
		private static PurpleClient instance;

		
		// START UP /////////////////////////
		protected PurpleClient ()
		{
			//
		}


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
		


		// SINGLETON /////////////////////////
		public static PurpleClient Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleClientManager");
					instance     			= gameObject.AddComponent<PurpleClient> ();
				}
				return instance;
			}
		}
	}
}
