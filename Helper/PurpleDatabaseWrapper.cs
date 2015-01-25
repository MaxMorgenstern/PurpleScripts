using System.Collections;
using System.Collections.Generic;

namespace PurpleDatabase
{
	public class Query
	{
		private static List<string> SQLHistory;
		private static string SQLQuery;

		private class SQLQueryItem
		{
			public enum OptionEnum {SELECT, UPDATE, DELETE};
			public enum SortEnum {ASC, DESC};
			
			public string keyFrom 		= "FROM";
			public string keyWhere 		= "WHERE";
			public string keyLike 		= "LIKE";
			public string keySet 		= "SET";
			public string keyLimit 		= "LIMIT";
			public string keyOffset 	= "OFFSET";
			public string keyOrderBy 	= "ORDER BY";
			public string keyStar 		= "*";
			public string keyLikeSymbol	= "%";
			public string keyEnd 		= ";";

			public OptionEnum 		Option;

			public List <string> 	Fields;
			public string 			Table;
			public List <string> 	Logic;

			public int 				Limit;
			public int 				Offset;

			public SortEnum			Sorting;

		}



		Hashtable args = new Hashtable();
		object this[string key]
		{
			get
			{
				return args[key];
			}
			set 
			{
				args[key] = value;
			}
		}


		// TODO - lists for operations...
		// OPTION
		// FIELDS
		// TABLE
		// LOGIC
		// LIMITATION
		// SORTING

		/*

		TableNameFromUserInput

		string mysqlstring = @"Unescaped but with at";

		Idea:
		dbConnection.select("age").from("user").where("name = 'Max'").execute();	SELECT age FROM user WHERE name = 'Max';
		dbConnection.from("account").execute();										SELECT * FROM account;
		dbConnection.sql("SELECT * FROM dummy LIMIT 5;");
		 */



		protected Query ()
		{
			PurpleDatabase.Initialize ();
		}

		private void build_query()
		{
		}



		// new query
		public static void New()
		{
			
		}

		// execute query
		public static void Execute()
		{
			
		}

		// sql query
		public static void SQL()
		{
			
		}
		
		
		public static void Select()
		{
			
		}
		
		public static void From()
		{
			
		}
		
		public static void Where()
		{
			
		}

		public static void Like()
		{
			
		}
		
		public static void Limit()
		{
			
		}
		
		public static void Offset()
		{
			
		}

		public static void OrderBy()
		{
			
		}

		public static void ASC()
		{
			
		}

		public static void DESC()
		{
			
		}
		


		public static void Update()
		{
			
		}
		
		public static void Set()
		{
			
		}
		
		
		public static void Delete()
		{
			
		}

	}
}
