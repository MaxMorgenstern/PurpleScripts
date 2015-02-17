using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace PurpleDatabase
{
	public static class SQLGenerator
	{
		private static List<string> _SQLHistory = new List<string>();
		private static SQLQueryItem _SQLQuery;
		
		// SELECT - MASTER
		// TODO: if !UNITY_5_0
		// No named arguments with Unity versions below 5
		public static string Select(string select = "*", string from = "",
		                            string where = "", int limit = 0, int offset = 0, string sorting = "")
		{
			// If more passed than select do a reset
			if (!String.IsNullOrEmpty(from) || !String.IsNullOrEmpty(where) ||
			    limit != 0 || offset != 0 || !String.IsNullOrEmpty(sorting))
				_SQLQuery = new SQLQueryItem();

			_SQLQuery.Type = SQLQueryItem.TypeEnum.SELECT;

			AddSelect (select);

			if (!String.IsNullOrEmpty(from))
				_SQLQuery.Table = from;
			
			if (!String.IsNullOrEmpty(where))
				_SQLQuery.set_filter(where);
			
			if (limit != 0)
				_SQLQuery.Limit = limit;
			
			if (offset != 0)
				_SQLQuery.Offset = offset;
			
			if (!String.IsNullOrEmpty(sorting))
				_SQLQuery.set_order_by(sorting);
			
			return _SQLQuery.build();
		}

		public static void AddSelect(string select)
		{
			AddSelect(select.Split(new Char[] { ' ', ',' }));

		}
		public static void AddSelect(string[] select)
		{
			foreach(string singleSelect in select)
			{
				if(!String.IsNullOrEmpty(singleSelect))
					_SQLQuery.SelectFields.Add(singleSelect);
			}
		}

		
		// UPDATE - MASTER
		public static string Update(string update)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.UPDATE;

			// TODO
			return String.Empty;
		}
		
		// DELETE - MASTER
		public static string Delete(string delete)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.DELETE;

			// TODO
			return String.Empty;
		}

		// INSERT - MASTER
		public static string Insert(string insert)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.INSERT_INTO;

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
		
		// BUILD QUERY
		public static string Get()
		{
			return _SQLQuery.build();
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
			public enum TypeEnum { SELECT, INSERT_INTO, UPDATE, DELETE };
			public enum SortEnum { NONE, ASC, DESC };
			
			// Variables
			public TypeEnum Type                = TypeEnum.SELECT;
			public List<string> SelectFields    = new List<string>();
			
			public string Table                 = string.Empty;
			
			public List<string> Filter          = new List<string>();
			
			public Dictionary<string, SortEnum> SortList
				= new Dictionary<string, SortEnum>();
			
			public int Limit                    = 0;
			public int Offset                   = 0;
			
			
			// PRIVATE ////////////////////////////
			private string _query = String.Empty;
			
			private static string keyFrom           = "FROM";
			private static string keyWhere          = "WHERE";
			private static string keyValues         = "VALUES";
			// private static string keyLike        = "LIKE";
			private static string keySet            = "SET";
			private static string keyLimit          = "LIMIT";
			private static string keyOffset         = "OFFSET";
			private static string keyOrderBy        = "ORDER BY";
			private static string keyStar           = "*";
			// private static string keyLikeSymbol  = "%";
			private static string keyEnd            = ";";
			private static string keySpace          = " ";
			private static string keyPlaceholder    = "_";
			private static string keyEscapeSymbol	= "`";
			
			
			// MAIN ////////////////////////////
			// Cheatsheet:
			/*
            SELECT      (field1, field2)   FROM     table_name                                                      WHERE some_column=some_value    ORDER BY X LIMIT Y OFFSET Z;
            INSERT INTO                             table_name                          VALUES (value1,value2);
            INSERT INTO                             table_name  (field1, field2)        VALUES (value1,value2);
            UPDATE                                  table_name          SET column1=value1,column2=value2           WHERE some_column=some_value;
            DELETE                         FROM     table_name                                                      WHERE some_column=some_value;
            */
			
			
			
			public string build()
			{
				// Reset query
				_query = String.Empty;

				// TYPE (SELECT / INSERT INTO...)
				Add(Regex.Replace(Type.ToString(), keyPlaceholder, keySpace, RegexOptions.IgnoreCase));
				
				if (Type == TypeEnum.SELECT)
				{
					build_select_fields();	// SELECT FIELDS
					Add(keyFrom);       	// FROM
				}
				if (Type == TypeEnum.DELETE)
				{
					Add(keyFrom);       	// FROM
				}
				
				// TABLE
				Add(Table);
				
				if (Type == TypeEnum.INSERT_INTO)
				{
					build_select_fields();	// SELECT FIELDS
					Add(keyValues);     	// VALUES
					// TODO: VALUE PART
				}
				
				if (Type == TypeEnum.UPDATE)
				{
					Add(keySet);			// SET
					// TODO: UPDATE PART
				}
				
				
				
				if (Type != TypeEnum.INSERT_INTO)
				{
					// WHERE
					if (Filter.Count > 0)
					{
						Add(keyWhere);
						// TODO - Not only AND
						Add(string.Join("AND ", Filter.ToArray()));
					}
				}
				
				if (Type == TypeEnum.SELECT)
				{
					// ORDER BY
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
					
					// LIMIT
					if (Limit != 0)
					{
						Add(keyLimit);
						Add(Limit.ToString());
					}
					
					// OFFSET
					if (Offset != 0)
					{
						Add(keyOffset);
						Add(Offset.ToString());
					}
				}

				return _query.Trim() + keyEnd;
			}
			
			// BUILD SELECT FIELDS
			public void build_select_fields()
			{
				// TODO: what if
				SelectFields.RemoveAll(x => x == keyStar);
				if (SelectFields.Count > 0)
				{
					string select_string = string.Join(keyEscapeSymbol+", "+keyEscapeSymbol, SelectFields.ToArray());
					Add(keyEscapeSymbol + select_string + keyEscapeSymbol);
				}
				else
				{
					Add(keyStar);
				}
			}
			
			public void build_insertvalue_fields()
			{
				
			}
			
			public void build_updateset_fields()
			{
				
			}
			
			// HELPER ////////////////////////////
			public void set_filter(string SortOption)
			{
				Filter.Add(SortOption);
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
				_query += (keySpace + part).TrimEnd();
				return _query;
			}
		}
	}
}