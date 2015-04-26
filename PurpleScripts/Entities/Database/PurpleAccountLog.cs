using System;
using PurpleAttributes;

namespace Entities.Database
{
	public class PurpleAccountLog
	{
		public int id { get; set; }

		[Required]
		public int account_id { get; set; }

		[Required]
		public string log { get; set; }
		public DateTime timestamp { get; set; }
	}
}
