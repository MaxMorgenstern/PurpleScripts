using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PurpleDatabase
{
	public class PurpleDatabaseWrapper : MonoBehaviour
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



		protected PurpleDatabaseWrapper ()
		{
			PurpleDatabase.Initialize ();
		}

		private void build_query()
		{
		}



		// new query
		public void New()
		{
			
		}

		// execute query
		public void Execute()
		{
			
		}

		// sql query
		public void SQL()
		{
			
		}
		
		
		public void Select()
		{
			
		}
		
		public void From()
		{
			
		}
		
		public void Where()
		{
			
		}

		public void Like()
		{
			
		}
		
		public void Limit()
		{
			
		}
		
		public void Offset()
		{
			
		}

		public void OrderBy()
		{
			
		}

		public void ASC()
		{
			
		}

		public void DESC()
		{
			
		}
		


		public void Update()
		{
			
		}
		
		public void Set()
		{
			
		}
		
		
		public void Delete()
		{
			
		}

	}
}
