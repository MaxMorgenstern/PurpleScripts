using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace PurpleDatabase
{
	public static class SQLGenerator
	{
		private static SQLQueryItem _SQLQuery;

		private static List<string> _SQLHistory = new List<string>();
		public static List<string> SQLHistory
		{
			get
			{
				return _SQLHistory;
			}
		}

		// TODO
		private static List<string> _SQLQueryHistory = new List<string>();
		public static List<string> SQLQueryHistory
		{
			get
			{
				return _SQLQueryHistory;
			}
		}

		// SELECT - MASTER
		// TODO: if !UNITY_5_0
		// No named arguments with Unity versions below 5
		public static string Select(string select = "*", string from = "",
		                            string where = "", int limit = 0, int offset = 0, string sorting = "")
		{
			// If more passed than select do a query reset
			if (!String.IsNullOrEmpty(from) || !String.IsNullOrEmpty(where) ||
			    limit != 0 || offset != 0 || !String.IsNullOrEmpty(sorting))
				_SQLQuery = new SQLQueryItem();

			_SQLQuery.Type = SQLQueryItem.TypeEnum.SELECT;

			AddSelect(select);

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
			foreach (string singleSelect in select)
			{
				if (!String.IsNullOrEmpty(singleSelect))
					_SQLQuery.SelectFields.Add(singleSelect);
			}
		}


		// INSERT - MASTER
		public static string Insert(string into, string[] values)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.INSERT_INTO;

			_SQLQuery.Table = into;

			return _SQLQuery.build();
		}


		// UPDATE - MASTER
		public static string Update(string set, string from = "", string where = "")
		{
			return Update (new string[]{set}, from, where);
		}

		public static string Update(string[] set, string from = "", string where = "")
		{
			// If more passed than select do a query reset
			if (!String.IsNullOrEmpty(from) || !String.IsNullOrEmpty(where))
				_SQLQuery = new SQLQueryItem();

			_SQLQuery.Type = SQLQueryItem.TypeEnum.UPDATE;

			UpdateSet (set);

			if (!String.IsNullOrEmpty(from))
				_SQLQuery.Table = from;
			
			if (!String.IsNullOrEmpty(where))
				_SQLQuery.set_filter(where);

			return  _SQLQuery.build();
		}

		public static void UpdateSet(string[] set)
		{
			foreach (string singleSet in set)
			{
				if (!String.IsNullOrEmpty(singleSet))
					_SQLQuery.set_set_field(singleSet);
			}
		}


		// DELETE - MASTER
		public static string Delete(string delete)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.DELETE;


			return _SQLQuery.build();
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
		public static void Single()
		{
			_SQLQuery.Limit = 1;
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

		public static void ASC(string SortField) {
			_SQLQuery.set_order_by(SortField, "ASC");
		}

		public static void DESC(string SortField) {
			_SQLQuery.set_order_by(SortField, "DESC");
		}


		/*
        public static void Like() {}
        */

		public static void EnableEscape() {
			_SQLQuery.enable_escape_symbol ();
		}

		public static void DisableEscape() {
			_SQLQuery.disable_escape_symbol ();
		}


		private class SQLQueryItem
		{
			public enum TypeEnum { SELECT, INSERT_INTO, UPDATE, DELETE };
			public enum SortEnum { NONE, ASC, DESC };

			// Variables
			public TypeEnum Type = TypeEnum.SELECT;
			public List<string> SelectFields = new List<string>();

			public Dictionary<string, string> SetFields 
				= new Dictionary<string, string>();

			public string Table = string.Empty;

			public Dictionary<string, string> FilterList 
				= new Dictionary<string, string>();

			public Dictionary<string, SortEnum> SortList
				= new Dictionary<string, SortEnum>();

			public int Limit = 0;
			public int Offset = 0;


			// PRIVATE ////////////////////////////
			private string _query = String.Empty;

			private static string keyFrom 			= "FROM";
			private static string keyWhere 			= "WHERE";
			private static string keyValues 		= "VALUES";
			// private static string keyLike        = "LIKE";
			private static string keySet 			= "SET";
			private static string keyLimit 			= "LIMIT";
			private static string keyOffset 		= "OFFSET";
			private static string keyOrderBy 		= "ORDER BY";
			private static string keyNULL	 		= "NULL";
			private static string keyStar 			= "*";
			// private static string keyLikeSymbol  = "%";
			private static string keyEnd 			= ";";
			private static string keySpace 			= " ";
			private static string keyPlaceholder 	= "_";
			private static string keyEscapeSymbol 	= "`";
			private static string keyEqualsSymbol 	= "=";

			
			private static string activeEscapeSymbol= "`";

			public void enable_escape_symbol()
			{
				activeEscapeSymbol = keyEscapeSymbol;
			}

			public void disable_escape_symbol()
			{
				activeEscapeSymbol = String.Empty;
			}

			// MAIN ////////////////////////////
			// Cheatsheet:
			/*
            Type        SelectFields                Table                                                           Filter                          SortList    Limit   Offset
             *
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

				Add(Regex.Replace(Type.ToString(), keyPlaceholder, keySpace, RegexOptions.IgnoreCase));

				if (Type == TypeEnum.SELECT)
				{
					build_select_fields();
					Add(keyFrom);
				}

				if (Type == TypeEnum.DELETE)
				{
					Add(keyFrom);
				}

				// TABLE // TODO - more than one table
				Add(activeEscapeSymbol+Table+activeEscapeSymbol);

				if (Type == TypeEnum.INSERT_INTO)
				{
					build_insert_value_fields();
				}

				if (Type == TypeEnum.UPDATE)
				{
					Add(keySet);
					build_set_fields();
				}


				if (Type != TypeEnum.INSERT_INTO)
				{
					if (FilterList.Count > 0)
					{
						Add(keyWhere);
						build_where_fields();
					}
				}

				if (Type == TypeEnum.SELECT)
				{
					if (SortList.Count > 0)
					{
						Add(keyOrderBy);
						build_order_by_fields();
					}

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
				}

				_query = _query.Trim() + keyEnd;

				_SQLQueryHistory.Add(_query);
				return _query;
			}


			// HELPER ////////////////////////////
			public void set_filter(string SortOption)
			{
				string[] split = SortOption.Split(new Char[] { '=' });
				for (int i = 0; i < split.Length; i += 2)
				{
					FilterList.Add((split[i]).Trim(), (split[i+1]).Trim());
				}
			}

			public void set_order_by(string SortOption)
			{
				string[] split = SortOption.Split(new Char[] { ' ', ',' });
				for (int i = 0; i < split.Length; i += 2)
				{
					SortList.Add((split[i]).Trim(), (SortEnum)Enum.Parse(typeof(SortEnum), split[i + 1], true));
				}
			}

			public void set_order_by(string SortField, string SortOrder)
			{
				SortList.Add(SortField.Trim(), (SortEnum)Enum.Parse(typeof(SortEnum), SortOrder, true));
			}

			public void set_set_field(string SetOption)
			{
				string[] split = SetOption.Split(new Char[] { '=' });
				for (int i = 0; i < split.Length; i += 2)
				{
					SetFields.Add((split[i]).Trim(), (split[i+1]).Trim());
				}
			}

			public void set_set_field(string Key, string Value)
			{
				SetFields.Add(Key.Trim(), Value.Trim());
			}


			// PRIVATE ////////////////////////////
			private string Add(string part)
			{
				_query += (keySpace + part).TrimEnd();
				return _query;
			}

			// BUILD SELECT FIELDS
			private void build_select_fields()
			{
				// TODO: is this working all the time?
				SelectFields.RemoveAll(x => x == keyStar);
				if (SelectFields.Count > 0)
				{
					string select_string = string.Join(activeEscapeSymbol + ", " + activeEscapeSymbol, SelectFields.ToArray());
					Add(activeEscapeSymbol + select_string + activeEscapeSymbol);
				}
				else
				{
					Add(keyStar);
				}
			}
			
			private void build_where_fields()
			{
				// TODO - Not only AND
				bool first = true;
				foreach (KeyValuePair<string, string> FilterElement in FilterList) {
					if(first)
					{
						first = false;
					}
					else
					{
						// TODO: OR ...
						Add ("AND");
					}
					Add(activeEscapeSymbol + FilterElement.Key + activeEscapeSymbol);
					Add(keyEqualsSymbol);
					Add(FilterElement.Value);
				}
			}
			
			private void build_insert_value_fields()
			{
				// TODO: (field1, field2)        VALUES (value1,value2);
				
				// (field1,field2);
				Add(keyValues);
				//  (value1,value2);
				
			}
			
			private void build_set_fields()
			{
				foreach (KeyValuePair<string, string> SetElement in SetFields) {
					Add(activeEscapeSymbol + SetElement.Key + activeEscapeSymbol);
					Add(keyEqualsSymbol);
					if(SetElement.Value.ToUpper() == keyNULL)
					{
						Add (keyNULL);
					}
					else
					{
						Add(activeEscapeSymbol + SetElement.Value + activeEscapeSymbol);
					}
				}
			}
			
			private void build_order_by_fields()
			{
				foreach (KeyValuePair<string, SortEnum> SortElement in SortList)
				{
					if (SortElement.Value != SortEnum.NONE)
					{
						Add(activeEscapeSymbol + SortElement.Key + activeEscapeSymbol);
						Add(SortElement.Value.ToString());
					}
				}
			}
		}
	}
}