using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;

namespace PurpleDatabase
{
	public static class SQLGenerator
	{
		private static List<string> SQLHistory;
		private static string SQLQuery;

		private class SQLQueryItem
		{
			public enum TypeEnum {SELECT, UPDATE, DELETE};
			public enum SortEnum {NONE, ASC, DESC};

			// Variables
			public TypeEnum 		Type		= TypeEnum.SELECT;
			public List <string> 	Fields		= new List<string>();	

			public string 			Table		= string.Empty;

			public List <string> 	Filter		= null;

			public int 				Limit		= 0;
			public int 				Offset		= 0;

			public SortEnum			Sorting 	= SortEnum.NONE;


			// PRIVATE ////////////////////////////
			private string 			query		= String.Empty;
			
			private static string keyFrom 		= "FROM";
			private static string keyWhere 		= "WHERE";
			private static string keyLike 		= "LIKE";
			private static string keySet 		= "SET";
			private static string keyLimit 		= "LIMIT";
			private static string keyOffset 	= "OFFSET";
			private static string keyOrderBy 	= "ORDER BY";
			private static string keyStar 		= "*";
			private static string keyLikeSymbol	= "%";
			private static string keyEnd 		= ";";
			private static string keySpace 		= " ";


			// MAIN ////////////////////////////
			public string build()
			{
				// TODO: smarter

				query += Type.ToString () + keySpace + string.Join(", ", Fields.ToArray());
				query += keySpace + keyFrom + keySpace + Table;
				query += keyEnd;

				return query.Trim();
			}


			// SETUP - HELPER ////////////////////////////
			public void SetSorting(string LoadOption)
			{
				Sorting = (SortEnum) Enum.Parse(typeof(SortEnum), LoadOption, true);
			}
			public void SetSorting(SortEnum LoadOption)
			{
				Sorting = LoadOption;
			}

		}
		

		public static string Select(string select = "*", string from = "", 
					string where = "", int limit = 0, int offset = 0, string sorting = "") {

			SQLQueryItem sqi = new SQLQueryItem ();
			sqi.Fields.Add (select);
			sqi.Table = from;

			//TODO: where

			sqi.Limit = limit;
			sqi.Offset = offset;

			if (!String.IsNullOrEmpty (sorting))
				sqi.SetSorting (sorting); 

			return sqi.build ();
		}

		public static DataTable Fetch(this string query)
		{
			return PurpleDatabase.SelectQuery (query);
		}




		
		/*
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
		*/
		
		
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





		/*
		public static DataTable Fetch(string table)
		{
			return new DataTable ();
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
		*/
	}
}
