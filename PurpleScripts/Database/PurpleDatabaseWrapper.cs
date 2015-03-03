using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Text.RegularExpressions;

// TODO: SELECT * FROM xx WHERE y "IN (...)"

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

			add_select(select);

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

		public static string Select(string[] select)
		{
			add_select (select);
			return _SQLQuery.build();
		}


		// INSERT - MASTER
		public static string Insert(string into, string value)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.INSERT_INTO;

			_SQLQuery.Table = into;
			add_insert_column (value);

			return _SQLQuery.build();
		}

		public static string Insert(string into, string[] value)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.INSERT_INTO;

			_SQLQuery.Table = into;
			add_insert_value (value);

			return _SQLQuery.build();
		}


		// UPDATE - MASTER
		public static string Update(string set, string table = "", string where = "")
		{
			return Update (new string[]{set}, table, where);
		}

		public static string Update(string[] set, string table = "", string where = "")
		{
			// If more passed than select do a query reset
			if (!String.IsNullOrEmpty(table) || !String.IsNullOrEmpty(where))
				_SQLQuery = new SQLQueryItem();

			_SQLQuery.Type = SQLQueryItem.TypeEnum.UPDATE;

			update_set (set);

			if (!String.IsNullOrEmpty(table))
				_SQLQuery.Table = table;

			if (!String.IsNullOrEmpty(where))
				_SQLQuery.set_filter(where);

			return  _SQLQuery.build();
		}


		// DELETE - MASTER
		public static string Delete(string table, string where = "", int limit = 0)
		{
			_SQLQuery = new SQLQueryItem();
			_SQLQuery.Type = SQLQueryItem.TypeEnum.DELETE;

			_SQLQuery.Table = table;

			if (!String.IsNullOrEmpty (where))
				_SQLQuery.set_filter (where);

			if (limit != 0)
				_SQLQuery.Limit = limit;

			return _SQLQuery.build();
		}



		// SINGLE OPERATIONS /////////////

		// NEW QUERY
		public static string New()
		{
			_SQLQuery = new SQLQueryItem();
			return String.Empty;
		}

		public static void Reset()
		{
			_SQLQuery = new SQLQueryItem();
		}


		// FROM
		public static string From(string table)
		{
			_SQLQuery.Table = table;
			return _SQLQuery.build();
		}

		public static string Table(string table)
		{
			_SQLQuery.Table = table;
			return _SQLQuery.build();
		}

		// WHERE
		public static string Where(string logic)
		{
			_SQLQuery.set_filter(logic);
			return _SQLQuery.build();
		}
		public static string Where(string logic, string conjunction)
		{
			_SQLQuery.set_filter(logic, conjunction);
			return _SQLQuery.build();
		}

		public static string Like(string field, string like, string conjunction = "AND")
		{
			_SQLQuery.set_like_filter(field, like, conjunction);
			return _SQLQuery.build();
		}

		public static string Values(string value)
		{
			return Values (new string[] { value });
		}
		
		public static string Values(string[] value)
		{
			add_insert_value (value);
			return _SQLQuery.build();
		}


		// LIMIT - OFFSET
		public static string Limit(int limit = 0, int offset = 0)
		{
			_SQLQuery.Limit = limit;
			_SQLQuery.Offset = offset;
			return _SQLQuery.build();
		}

		public static string Single()
		{
			_SQLQuery.Limit = 1;
			return _SQLQuery.build();
		}

		// ORDER BY
		public static string OrderBy(string SortOption)
		{
			_SQLQuery.set_order_by(SortOption);
			return _SQLQuery.build();
		}

		public static string OrderBy(string SortField, string SortOrder)
		{
			_SQLQuery.set_order_by(SortField, SortOrder);
			return _SQLQuery.build();
		}

		public static string ASC(string SortField) {
			_SQLQuery.set_order_by(SortField, "ASC");
			return _SQLQuery.build();
		}

		public static string DESC(string SortField) {
			_SQLQuery.set_order_by(SortField, "DESC");
			return _SQLQuery.build();
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


		// ESCAPE STRINGS
		public static void EnableEscape() {
			_SQLQuery.enable_escape_symbol ();
		}

		public static void DisableEscape() {
			_SQLQuery.disable_escape_symbol ();
		}


		// PRIVATE FUNCTIONS /////////////////////////
		private static void add_select(string select)
		{
			add_select(select.Split(new Char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));

		}
		private static void add_select(string[] select)
		{
			foreach (string singleSelect in select)
			{
				if (!String.IsNullOrEmpty(singleSelect))
					_SQLQuery.SelectFields.Add(singleSelect);
			}
		}

		private static void update_set(string[] set)
		{
			foreach (string singleSet in set)
			{
				if (!String.IsNullOrEmpty(singleSet))
					_SQLQuery.set_set_field(singleSet);
			}
		}

		private static void add_insert_column (string value)
		{
			string[] column = value.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string singleColumn in column)
			{
				_SQLQuery.InsertColumn.Add(singleColumn.Trim ());
			}
		}
		
		private static void add_insert_value (string[] value)
		{
			foreach (string singleValue in value)
			{
				string[] data = singleValue.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				
				List<string> tmpList = new List<string>();
				foreach (string singleData in data)
				{
					if (singleData.IndexOf("=") != -1)
					{
						string[] dataParts = singleData.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						
						if(dataParts.Length > 2)
							return;
						
						if(!_SQLQuery.InsertColumn.Contains(dataParts[0].Trim ()))
							_SQLQuery.InsertColumn.Add(dataParts[0].Trim ());
						
						tmpList.Add(dataParts[1]);
					}
					else
					{
						tmpList.Add(singleData);
					}
				}
				_SQLQuery.InsertValues.Add(tmpList);
			}
		}


		// PRIVATE INNER CLASS /////////////////////////
		private class SQLQueryItem
		{
			public enum TypeEnum { SELECT, INSERT_INTO, UPDATE, DELETE };
			public enum SortEnum { NONE, ASC, DESC };
			public enum ConjunctionEnum { AND, OR };

			// Variables
			public TypeEnum Type = TypeEnum.SELECT;
			public List<string> SelectFields 		= new List<string>();

			public Dictionary<string, string> SetFields
				= new Dictionary<string, string>();

			public string Table 					= string.Empty;

			public List<SQLQueryField> FilterList 	= new List<SQLQueryField>();

			public List<string> InsertColumn 		= new List<string>();
			public List<List<string>> InsertValues 	= new List<List<string>>();

			public Dictionary<string, SortEnum> SortList
				= new Dictionary<string, SortEnum>();

			public int Limit 						= 0;
			public int Offset 						= 0;


			// PRIVATE ////////////////////////////
			private string _query = String.Empty;


			private static string keyFrom 			= "FROM";
			private static string keyWhere 			= "WHERE";
			private static string keyValues 		= "VALUES";
			private static string keyLike       	= "LIKE";
			private static string keySet 			= "SET";
			private static string keyLimit 			= "LIMIT";
			private static string keyOffset 		= "OFFSET";
			private static string keyOrderBy 		= "ORDER BY";
			private static string keyNULL	 		= "NULL";
			private static string keyStar 			= "*";
			private static string keyEnd 			= ";";
			private static string keySpace 			= " ";
			private static string keyPlaceholder 	= "_";
			private static string keyEscapeSymbol 	= "`";
			private static string keyStringEscapeSymbol= "'";
			private static string keyEqualsSymbol 	= "=";
			private static string keyDelimiter 		= ",";
			private static string keyBracketOpen 	= "(";
			private static string keyBracketClose 	= ")";

			private static string activeEscapeSymbol= "`";

			private static char[] _trimSymbols 		= new char[] { ' ', activeEscapeSymbol[0], keyStringEscapeSymbol[0] };
			private static string[] _splitChar 		= new string[] { "<=", ">=", "=", ">", "<", keyLike };

			public class SQLQueryField
			{
				public string key;
				public string value;
				public string operation;

				public ConjunctionEnum conjunction = ConjunctionEnum.AND;
			}

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
				Add(AddEscapeSymbol(Table));

				if (Type == TypeEnum.INSERT_INTO)
				{
					build_insert_value_fields();
				}

				if (Type == TypeEnum.UPDATE)
				{
					Add(keySet);
					build_set_fields();

					if (Limit != 0)
					{
						Add(keyLimit);
						Add(Limit.ToString());
					}
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

				if (Type == TypeEnum.DELETE)
				{
					if (Limit != 0)
					{
						Add(keyLimit);
						Add(Limit.ToString());
					}
				}

				_query = _query.Trim() + keyEnd;

				_SQLQueryHistory.Add(_query);
				return _query;
			}



			// HELPER ////////////////////////////
			public void set_like_filter(string field, string like, string conjunction)
			{
				set_filter (field + keySpace + keyLike + keySpace + like, conjunction);
			}

			public void set_filter(string SortOption)
			{
				string conjunction = (SortOption.IndexOf("OR") != -1) ? "OR" : "AND";

				SortOption = SortOption.Replace ("AND ", String.Empty);
				SortOption = SortOption.Replace ("OR ", String.Empty);

				set_filter (SortOption, conjunction);
			}

			public void set_filter(string SortOption, string Conjunction)
			{
				if(String.IsNullOrEmpty(Conjunction))
					Conjunction = "AND";

				string operation = String.Empty;
				if (SortOption.IndexOf(keyLike) != -1)
				{
					operation = keyLike;
				}
				else
				{
					Regex reg = new Regex ("[^<>=]");
					operation = reg.Replace (SortOption, "");
				}

				string[] split = SortOption.Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < split.Length; i += 2)
				{
					SQLQueryField queryField = new SQLQueryField();
					queryField.key = (split[i]).Trim();
					queryField.value = AddStringEscapeSymbol(split[i+1]);
					queryField.operation = operation;

					queryField.conjunction = (ConjunctionEnum)Enum.Parse(typeof(ConjunctionEnum), Conjunction, true);

					FilterList.Add(queryField);
				}
			}

			public void set_order_by(string SortOption)
			{
				string[] split = SortOption.Split(new Char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
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
				string[] split = SetOption.Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < split.Length; i += 2)
				{
					SetFields.Add((split[i]).Trim(), (split[i+1]).Trim());
				}
			}

			public void set_set_field(string Key, string Value)
			{
				SetFields.Add(Key.Trim(), Value.Trim());
			}


			public string AddEscapeSymbol(string str)
			{
				return activeEscapeSymbol + str.Trim(_trimSymbols) + activeEscapeSymbol;
			}

			public string AddStringEscapeSymbol(string str)
			{
				double value = double.MinValue;
				try
				{
					if (double.TryParse (str, out value))
					{
						return str.Trim ();
					}
				} catch { }

				str = str.Trim (_trimSymbols);
				if(object.Equals(str.ToUpper(), keyNULL))
				{
					return keyNULL;
				}

				return keyStringEscapeSymbol + str + keyStringEscapeSymbol;
			}


			// PRIVATE ////////////////////////////
			private string Add(string part)
			{
				_query += (keySpace + part).TrimEnd();
				return _query;
			}

			private string Add(string part, bool addSpace)
			{
				if(addSpace)
					return Add (part);

				_query += (part).TrimEnd();
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
					Add(AddEscapeSymbol(select_string));
				}
				else
				{
					Add(keyStar);
				}
			}

			private void build_where_fields()
			{
				bool first = true;
				foreach (SQLQueryField FilterElement in FilterList) {
					if(first)
					{
						first = false;
					}
					else
					{
						Add (FilterElement.conjunction.ToString());
					}
					Add(AddEscapeSymbol(FilterElement.key));
					Add(FilterElement.operation);
					Add(FilterElement.value);
				}
			}

			private void build_insert_value_fields()
			{
				string columnString = string.Join(activeEscapeSymbol + ", " + activeEscapeSymbol, InsertColumn.ToArray());
				if(!String.IsNullOrEmpty(columnString))
				{
					Add (keyBracketOpen+AddEscapeSymbol(columnString)+keyBracketClose);
				}

				Add(keyValues);

				bool first = true;
				foreach (List<string> valueList in InsertValues)
				{
					if(first)
					{
						first = false;
					}
					else
					{
						Add (keyDelimiter, false);
					}

					string valueString = string.Empty;
					foreach(string s in valueList)
					{
						valueString += " " +AddStringEscapeSymbol(s) + ",";
					}
					valueString = valueString.Trim(new char[] {',', ' '});

					if(!String.IsNullOrEmpty(valueString))
					{
						Add(keyBracketOpen+(valueString)+keyBracketClose);
					}
				}
			}

			private void build_set_fields()
			{
				bool first = true;
				foreach (KeyValuePair<string, string> SetElement in SetFields) {
					if(first)
					{
						first = false;
					}
					else
					{
						Add (keyDelimiter, false);
					}
					Add(AddEscapeSymbol(SetElement.Key));
					Add(keyEqualsSymbol);
					Add(AddStringEscapeSymbol(SetElement.Value));
				}
			}

			private void build_order_by_fields()
			{
				foreach (KeyValuePair<string, SortEnum> SortElement in SortList)
				{
					if (SortElement.Value != SortEnum.NONE)
					{
						Add(AddEscapeSymbol(SortElement.Key));
						Add(SortElement.Value.ToString());
					}
				}
			}
		}
	}
}