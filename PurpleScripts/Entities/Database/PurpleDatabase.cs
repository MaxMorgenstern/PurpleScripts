using System;
using PurpleAttributes;
using PurpleDatabase;

namespace Entities.Database
{
	public class PurpleDatabase
	{
		public int id { get; set; }

		[Required]
		public string version { get; set; }
		public DateTime valid_from { get; set; }
		public DateTime timestamp { get; set; }

		public static string CurrentVersion()
		{
			PurpleDatabase pd = new PurpleDatabase ().ToSQLSelectMax ("valid_from").FetchSingle ().ToObject<PurpleDatabase> ();
			return pd.version;
		}

		public static DateTime CurrentVersionDate()
		{
			PurpleDatabase pd = new PurpleDatabase ().ToSQLSelectMax ("valid_from").FetchSingle ().ToObject<PurpleDatabase> ();
			return pd.valid_from;
		}
	}
}
