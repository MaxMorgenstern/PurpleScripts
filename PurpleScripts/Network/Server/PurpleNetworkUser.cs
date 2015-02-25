using UnityEngine;
using System.Collections;
using System;

namespace PurpleNetwork.Server
{
	public class PurpleNetworkUser
	{
		public NetworkPlayer 	UserReference;

		public Guid				UserID;
		public UserTypes		UserType;
		public bool				UserAuthenticated;
		public DateTime			UserConnectedTime;

		public string			UserName;
		public string			UserToken;


		// CONSTRUCTOR
		public PurpleNetworkUser ()
		{
			UserID 				= new Guid ();
			UserType 			= UserTypes.User;
			UserAuthenticated 	= false;
			UserConnectedTime 	= DateTime.Now;
			
			UserName			= String.Empty;
			UserToken 			= String.Empty;
		}

		public PurpleNetworkUser (NetworkPlayer player)
		{
			UserReference 		= player;

			UserID 				= new Guid ();
			UserType 			= UserTypes.User;
			UserAuthenticated 	= false;
			UserConnectedTime 	= DateTime.Now;

			UserName			= String.Empty;
			UserToken 			= String.Empty;
		}
	}
}
