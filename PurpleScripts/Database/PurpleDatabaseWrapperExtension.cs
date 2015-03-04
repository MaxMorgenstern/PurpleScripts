using UnityEngine;
using System.Collections;

// TODO:

namespace PurpleDatabase.Extension
{
	public static class SQLGeneratorExtension
	{
		public static string Select(this string s, string select = "*", string from = "",
									string where = "", int limit = 0, int offset = 0, string sorting = "")
		{
			return SQLGenerator.Select (select, from, where, limit, offset, sorting);
		}

		public static string Select(this string s, string[] select)
		{
			return SQLGenerator.Select (select);
		}

		public static string Insert(this string s, string into, string[] values)
		{
			return SQLGenerator.Insert(into, values);
		}

		public static string From(this string s, string table)
		{
			return SQLGenerator.From (table);
		}
	}
}

