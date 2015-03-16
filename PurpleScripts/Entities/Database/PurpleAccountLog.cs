using System;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Entities.Database
{
	public class PurpleAccountLog
	{
		public int id { get; set; }
		public int account_id { get; set; }
		public string log { get; set; }
		public DateTime timestamp { get; set; }
	}
}
