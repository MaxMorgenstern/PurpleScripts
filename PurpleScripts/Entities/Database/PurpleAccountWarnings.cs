using System;
using System.Collections;
using System.Data;
using UnityEngine;
using PurpleAttributes;

namespace Entities.Database
{
	public class PurpleAccountWarnings
	{
		public int id { get; set; }

		[Required]
		public int account_id { get; set; }

		[Required]
		public int warning_level { get; set; }
		public string comment { get; set; }
		public DateTime timestamp { get; set; }
	}
}
