using System;
using System.Collections;
using System.Data;
using UnityEngine;
using PurpleAttributes;

namespace Entities.Database
{
	public class PurpleAccount
	{
		public int id { get; set; }
		protected Guid _guid;

		[Required]
		public string guid {
			get
			{
				return _guid.ToString();
			}

			set
			{
				if (String.IsNullOrEmpty(value))
				{
					NewGuid();
				}
				else
				{
					_guid = new Guid(value);
				}
			}
		}

		[Required]
		public string username { get; set; }

		[Required]
		public string password { get; set; }

		[Required]
		[Expression(ExpressionType.EMail)]
		public string email { get; set; }
		public DateTime email_verification { get; set; }
		public DateTime account_created { get; set; }
		public DateTime timestamp { get; set; }

		[Required]
		public string first_name { get; set; }

		[Required]
		public string last_name { get; set; }

		[Required]
		public string gender { get; set; }
		public DateTime birthday { get; set; }

		[Required]
		[Scope(1,6)]
		public string country_code { get; set; }

		[Required]
		[Scope(1,6)]
		public string language_code { get; set; }
		public DateTime last_seen { get; set; }
		public DateTime last_login { get; set; }
		public string last_login_ip { get; set; }

		[Required]
		public string account_type { get; set; }
		public string token { get; set; }
		public DateTime token_created { get; set; }
		public string comment { get; set; }
		public bool active { get; set; }

		public void NewGuid()
		{
			_guid = Guid.NewGuid ();
		}
	}
}
