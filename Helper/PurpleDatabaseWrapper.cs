using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;

namespace PurpleDatabase
{
	public static class SQLGenerator
	{
		private static List<string> _SQLHistory = new List<string>();
		private static SQLQueryItem _SQLQuery;

		private class SQLQueryItem
		{
			public enum TypeEnum {SELECT, UPDATE, DELETE};
			public enum SortEnum {NONE, ASC, DESC};

			// Variables
			public TypeEnum 		Type		= TypeEnum.SELECT;
			public List <string> 	Fields		= new List<string>();	

			public string 			Table		= string.Empty;

			public List <string> 	Filter		= null;
			
			public Dictionary<string, SortEnum> SortList 
				= new Dictionary<string, SortEnum> ();

			public int 				Limit		= 0;
			public int 				Offset		= 0;


			// PRIVATE ////////////////////////////
			private string 			query		= String.Empty;
			
			private static string keyFrom 		= "FROM";
			private static string keyWhere 		= "WHERE";
			// private static string keyLike 		= "LIKE";
			// private static string keySet 		= "SET";
			private static string keyLimit 		= "LIMIT";
			private static string keyOffset 	= "OFFSET";
			private static string keyOrderBy 	= "ORDER BY";
			private static string keyStar 		= "*";
			// private static string keyLikeSymbol	= "%";
			private static string keyEnd 		= ";";
			private static string keySpace 		= " ";


			// MAIN ////////////////////////////
			public string build()
			{
				// TYPE
				Add (Type.ToString ());
				Fields.RemoveAll(x => x == keyStar);
				if(Fields.Count > 0)
				{
					Add (string.Join(", ", Fields.ToArray()));
				}
				else
				{
					Add (keyStar);
				}

				// TABLE
				Add (keyFrom);
				Add (Table);

				// FILTER
				if(Filter != null)
				{
					Add (keyWhere);
					// TODO
				}

				// SORTING
				if(SortList.Count > 0)
				{
					Add (keyOrderBy);

					foreach (KeyValuePair<string, SortEnum> SortElement in SortList)
					{
						if(SortElement.Value != SortEnum.NONE)
						{
							Add (SortElement.Key);
							Add (SortElement.Value.ToString());
						}
					}
				}

				// LIMITATION
				if(Limit != 0)
				{
					Add (keyLimit);
					Add (Limit.ToString());
				}
				
				if(Offset != 0)
				{
					Add (keyOffset);
					Add (Offset.ToString());
				}

				return query.Trim () + keyEnd;
			}


			// HELPER ////////////////////////////
			public void SetSorting(string SortOption)
			{
				string [] split = SortOption.Split(new Char [] {' ', ',' });
				for(int i=0; i<split.Length; i+=2)
				{
					SortList.Add(split[i], (SortEnum) Enum.Parse(typeof(SortEnum), split[i+1], true));
				}
			}

			public void SetSorting(string SortField, string SortOrder)
			{
				SortList.Add(SortField, (SortEnum) Enum.Parse(typeof(SortEnum), SortOrder, true));
			}


			// PRIVATE ////////////////////////////
			private string Add(string part)
			{
				query += (keySpace + part).TrimEnd ();
				return query;
			}
		}
		

		public static string Select(string select = "*", string from = "", 
					string where = "", int limit = 0, int offset = 0, string sorting = "") {

			_SQLQuery = new SQLQueryItem ();
			_SQLQuery.Fields.Add (select);
			_SQLQuery.Table = from;

			//TODO: where

			_SQLQuery.Limit = limit;
			_SQLQuery.Offset = offset;

			if (!String.IsNullOrEmpty (sorting))
				_SQLQuery.SetSorting (sorting); 

			return _SQLQuery.build ();
		}
		
		public static void Sort(string SortOption)
		{
			_SQLQuery.SetSorting (SortOption);
		}

		public static void Sort(string SortField, string SortOrder)
		{
			_SQLQuery.SetSorting (SortField, SortOrder);	
		}

		public static DataTable Fetch(this string query)
		{
			_SQLHistory.Add (query);
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
