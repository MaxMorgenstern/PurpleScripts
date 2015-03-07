using UnityEngine;
using System.Collections;

namespace PurpleDatabase.Extension
{
	public static class SQLGeneratorExtension
	{
		// SELECT - MASTER
		public static string Select(this string s, string select = "*", string from = "",
		                            string where = "", int limit = 0, int offset = 0, string sorting = "")
		{
			return SQLGenerator.Select (select, from, where, limit, offset, sorting);
		}

		public static string Select(this string s, string[] select)
		{
			return SQLGenerator.Select (select);
		}

		// INSERT - MASTER
		public static string Insert(this string s, string into, string value)
		{
			return SQLGenerator.Insert(into, value);
		}

		public static string Insert(this string s, string into, string[] value)
		{
			return SQLGenerator.Insert(into, value);
		}

		// UPDATE - MASTER
		public static string Update(this string s, string set, string table = "", string where = "")
		{
			return SQLGenerator.Update (set, table, where);
		}

		public static string Update(this string s, string[] set, string table = "", string where = "")
		{
			return SQLGenerator.Update (set, table, where);
		}

		// DELETE - MASTER
		public static string Delete(this string s, string table, string where = "", int limit = 0)
		{
			return SQLGenerator.Delete(table, where, limit);
		}


		// SINGLE OPERATIONS /////////////

		// FROM
		public static string From(this string s, string table)
		{
			return SQLGenerator.From (table);
		}

		public static string Table(this string s, string table)
		{
			return SQLGenerator.From (table);
		}

		// WHERE
		public static string Where(this string s, string logic)
		{
			return SQLGenerator.Where (logic);
		}

		public static string Where(this string s, string logic, string conjunction)
		{
			return SQLGenerator.Where (logic, conjunction);
		}

		// LIKE
		public static string Like(this string s, string field, string like, string conjunction = "AND")
		{
			return SQLGenerator.Like (field, like, conjunction);
		}

		// WHERE IN
		public static string In(this string s, string field, string inQuery, bool filterQuery = true, string conjunction = "AND")
		{
			return SQLGenerator.In (field, inQuery, filterQuery, conjunction);
		}

		// INSERT VALUES
		public static string Values(this string s, string value)
		{
			return SQLGenerator.Values (value);
		}

		public static string Values(this string s, string[] value)
		{
			return SQLGenerator.Values (value);
		}

		// LIMIT - OFFSET
		public static string Limit(this string s, int limit = 0, int offset = 0)
		{
			return SQLGenerator.Limit (limit, offset);
		}

		public static string Single(this string s)
		{
			return SQLGenerator.Single ();
		}

		// ORDER BY
		public static string OrderBy(this string s, string SortOption)
		{
			return SQLGenerator.OrderBy (SortOption);
		}

		public static string OrderBy(this string s, string SortField, string SortOrder)
		{
			return SQLGenerator.OrderBy (SortField, SortOrder);
		}

		// ASC - DESC
		public static string ASC(this string s, string SortField)
		{
			return SQLGenerator.ASC (SortField);
		}

		public static string DESC(this string s, string SortField)
		{
			return SQLGenerator.DESC (SortField);
		}

		// BUILD QUERY
		public static string Get(this string s)
		{
			return SQLGenerator.Get ();
		}

		public static string Build(this string s)
		{
			return SQLGenerator.Build ();
		}
	}
}
