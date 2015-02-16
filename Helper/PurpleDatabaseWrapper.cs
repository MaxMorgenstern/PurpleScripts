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
		
		// SELECT - MASTER
		public static string Select(string select = "*", string from = "",
		                            string where = "", int limit = 0, int offset = 0, string sorting = "")
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Fields.Add(select);
			
			if (!String.IsNullOrEmpty(from))
				_SQLQuery.Table = from;

			if(!String.IsNullOrEmpty(where))
				_SQLQuery.set_filter(where);

			if(limit != 0)
				_SQLQuery.Limit = limit;

			if(offset != 0)
				_SQLQuery.Offset = offset;
			
			if (!String.IsNullOrEmpty(sorting))
				_SQLQuery.set_order_by(sorting);
			
			return _SQLQuery.build();
		}
		
		// UPDATE - MASTER
		public static string Update()
		{
			// TODO
			return String.Empty;
		}
		
		// DELETE - MASTER
		public static string Delete()
		{
			// TODO
			return String.Empty;
		}
		
		
		// SINGLE OPERATIONS /////////////
		
		// NEW QUERY
		public static void New()
		{
			_SQLQuery = new SQLQueryItem();
		}
		
		public static void Reset()
		{
			_SQLQuery = new SQLQueryItem();
		}

		
		// FROM
		public static void From(string table)
		{
			_SQLQuery.Table = table;
		}
		
		// WHERE
		public static void Where(string logic)
		{
			_SQLQuery.set_filter(logic);
		}
		
		// LIMIT - OFFSET
		public static void Limit(int limit = 0, int offset = 0)
		{
			_SQLQuery.Limit = limit;
			_SQLQuery.Offset = offset;
		}
		
		// ORDER BY
		public static void OrderBy(string SortOption)
		{
			_SQLQuery.set_order_by(SortOption);
		}
		
		public static void OrderBy(string SortField, string SortOrder)
		{
			_SQLQuery.set_order_by(SortField, SortOrder);
		}
		
		
		// EXECUTE QUERY
		public static DataTable Execute(this string query)
		{
			_SQLHistory.Add(query);
			return PurpleDatabase.SelectQuery(query);
		}
		
		public static DataTable Fetch(this string query)
		{
			_SQLHistory.Add(query);
			return PurpleDatabase.SelectQuery(query);
		}
		
		
		
		
		
		/*
        public static void Like() {}
        public static void ASC() {}
        public static void DESC() {}
        public static void Set() {}
        */
		
		
		private class SQLQueryItem
		{
			public enum TypeEnum { SELECT, UPDATE, DELETE };
			public enum SortEnum { NONE, ASC, DESC };
			
			// Variables
			public TypeEnum Type 			= TypeEnum.SELECT;
			public List<string> Fields 		= new List<string>();
			
			public string Table 			= string.Empty;
			
			public List<string> Filter 		= new List<string>();
			
			public Dictionary<string, SortEnum> SortList
							= new Dictionary<string, SortEnum>();
			
			public int Limit 				= 0;
			public int Offset				= 0;
			
			
			// PRIVATE ////////////////////////////
			private string query 					= String.Empty;
			
			private static string keyFrom 			= "FROM";
			private static string keyWhere 			= "WHERE";
			// private static string keyLike 		= "LIKE";
			// private static string keySet 		= "SET";
			private static string keyLimit 			= "LIMIT";
			private static string keyOffset 		= "OFFSET";
			private static string keyOrderBy 		= "ORDER BY";
			private static string keyStar 			= "*";
			// private static string keyLikeSymbol	= "%";
			private static string keyEnd 			= ";";
			private static string keySpace			 = " ";
			
			
			// MAIN ////////////////////////////
			// Cheatsheet:
			/*
                SELECT * FROM table_name;
                INSERT INTO table_name 
                    VALUES (value1,value2,value3,...);
                UPDATE table_name
                    SET column1=value1,column2=value2,...
                    WHERE some_column=some_value;
                DELETE FROM table_name
                    WHERE some_column=some_value;
            */
			
			public string build()
			{
				// TYPE
				Add(Type.ToString());
				Fields.RemoveAll(x => x == keyStar);
				if (Fields.Count > 0)
				{
					Add(string.Join(", ", Fields.ToArray()));
				}
				else
				{
					Add(keyStar);
				}
				
				// TABLE
				Add(keyFrom);
				Add(Table);
				
				// FILTER / WHERE
				if (Filter.Count > 0)
				{
					Add(keyWhere);
					// TODO - Not only AND
					Add(string.Join("AND ", Fields.ToArray()));
				}
				
				// SORTING
				if (SortList.Count > 0)
				{
					Add(keyOrderBy);
					
					foreach (KeyValuePair<string, SortEnum> SortElement in SortList)
					{
						if (SortElement.Value != SortEnum.NONE)
						{
							Add(SortElement.Key);
							Add(SortElement.Value.ToString());
						}
					}
				}
				
				// LIMITATION
				if (Limit != 0)
				{
					Add(keyLimit);
					Add(Limit.ToString());
				}
				
				if (Offset != 0)
				{
					Add(keyOffset);
					Add(Offset.ToString());
				}
				
				return query.Trim() + keyEnd;
			}
			
			
			// HELPER ////////////////////////////
			public void set_filter(string SortOption)
			{
				Filter.Add (SortOption);
			}

				
			public void set_order_by(string SortOption)
			{
				string[] split = SortOption.Split(new Char[] { ' ', ',' });
				for (int i = 0; i < split.Length; i += 2)
				{
					SortList.Add(split[i], (SortEnum)Enum.Parse(typeof(SortEnum), split[i + 1], true));
				}
			}
			
			public void set_order_by(string SortField, string SortOrder)
			{
				SortList.Add(SortField, (SortEnum)Enum.Parse(typeof(SortEnum), SortOrder, true));
			}
			
			
			// PRIVATE ////////////////////////////
			private string Add(string part)
			{
				query += (keySpace + part).TrimEnd();
				return query;
			}
		}
	}
}