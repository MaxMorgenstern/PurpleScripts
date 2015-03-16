using System;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Entities.Database
{
	public class PurpleAccount
	{
		public int id { get; set; }
		protected Guid _guid;
		public string guid { 
			get
			{
				return _guid.ToString();
			}

			set
			{
				_guid = new Guid(value);
			}
		}
		public string username { get; set; }
		public string password { get; set; }
		public string email { get; set; }
		public DateTime email_verification { get; set; }
		public DateTime account_created { get; set; }
		public DateTime timestamp { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string gender { get; set; }
		public DateTime birthday { get; set; }
		public string country_code { get; set; }
		public string language_code { get; set; }
		public DateTime last_seen { get; set; }
		public DateTime last_login { get; set; }
		public string last_login_ip { get; set; }
		public string account_type { get; set; }
		public string token { get; set; }
		public DateTime token_created { get; set; }
		public string comment { get; set; }
		public bool active { get; set; } 
	}
}
