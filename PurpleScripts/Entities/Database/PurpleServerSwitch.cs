using System;
using PurpleAttributes;

namespace Entities.Database
{
	public class PurpleServerSwitch
	{
		public int id { get; set; }

		[Required]
		public int from_server { get; set; }

		[Required]
		public int to_server { get; set; }

		[Required]
		public int account_id { get; set; }

		[Required]
		public string token { get; set; }

		public DateTime timestamp { get; set; }
	}
}
