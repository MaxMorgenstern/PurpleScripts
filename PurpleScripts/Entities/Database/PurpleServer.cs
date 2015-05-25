using System;
using PurpleAttributes;

namespace Entities.Database
{
	public class PurpleServer
	{
		public int id { get; set; }

		[Required]
		public string name { get; set; }

		[Required]
		public string host { get; set; }

		[Required]
		public int port { get; set; }
		public int currnet_player { get; set; }

		[Required]
		public int max_player { get; set; }

		[Required]
		public string type { get; set; }
		public string local_ip { get; set; }
		public string global_ip { get; set; }
		public DateTime timestamp { get; set; }
	}
}
