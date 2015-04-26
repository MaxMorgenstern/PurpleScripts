using System;
using System.Data;
using PurpleNetwork;
using UnityEngine;

namespace Entities.PurpleNetwork
{
	public class PurpleNetworkUser
	{
		public NetworkPlayer 	UserReference;

		public Guid				UserGUID;
		public int				UserID;
		public UserTypes		UserType;
		public bool				UserAuthenticated;
		public DateTime			UserConnectedTime;

		public string			UserName;
		public string			UserToken;
		public DateTime			UserTokenCreated;


		// CONSTRUCTOR
		public PurpleNetworkUser ()
		{
			UserGUID 			= new Guid ();
			UserID				= -1;
			UserType 			= UserTypes.User;
			UserAuthenticated 	= false;
			UserConnectedTime 	= DateTime.Now;

			UserName			= String.Empty;
			UserToken 			= String.Empty;
		}

		public PurpleNetworkUser (NetworkPlayer player)
		{
			UserReference 		= player;

			UserGUID 			= new Guid ();
			UserID				= -1;
			UserType 			= UserTypes.User;
			UserAuthenticated 	= false;
			UserConnectedTime 	= DateTime.Now;

			UserName			= String.Empty;
			UserToken 			= String.Empty;
			UserTokenCreated	= DateTime.MinValue;
		}

		public DataTable GetAsDataTable()
		{
			DataTable table = new DataTable();
			table.Columns.Add("id", typeof(int));
			table.Columns.Add("guid", typeof(string));
			table.Columns.Add("username", typeof(string));
			table.Columns.Add("password", typeof(string));
			table.Columns.Add("token", typeof(string));
			table.Columns.Add("token_created", typeof(DateTime));

			table.Rows.Add(UserID, UserGUID.ToString(), UserName, string.Empty, UserToken, UserTokenCreated);
			return table;
		}
	}
}
