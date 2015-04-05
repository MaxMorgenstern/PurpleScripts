using System;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Entities.Database
{
	public class PurpleServerLog
	{
		public int id { get; set; }
		public string name { get; set; }
		public string host { get; set; }
		public int port { get; set; }
		public int max_player { get; set; }
		public string type { get; set; }
		public string local_ip { get; set; }
		public string global_ip { get; set; }
		public string comment { get; set; }
		public DateTime timestamp { get; set; }
	}
}
