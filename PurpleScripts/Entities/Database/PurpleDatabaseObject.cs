using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PurpleDatabase;
using PurpleDatabase.Extension;

namespace Entities.Database
{
	public static class PurpleDatabaseObject
	{
		// PurpleAccount ////////////////////////////

		public static string ToSQLInsert(this PurpleAccount data)
		{
			return to_sql_insert (data);
		}

		public static string ToSQLUpdate(this PurpleAccount data)
		{
			return to_sql_update (data, data.id);
		}

		public static string ToSQLDelete(this PurpleAccount data)
		{
			return to_sql_delete(data, data.id);
		}


		// PurpleAccountLog ////////////////////////////

		public static string ToSQLInsert(this PurpleAccountLog data)
		{
			return to_sql_insert (data);
		}

		public static string ToSQLUpdate(this PurpleAccountLog data)
		{
			return to_sql_update (data, data.id);
		}

		public static string ToSQLDelete(this PurpleAccountLog data)
		{
			return to_sql_delete(data, data.id);
		}


		// PurpleAccountWarnings ////////////////////////////

		public static string ToSQLInsert(this PurpleAccountWarnings data)
		{
			return to_sql_insert (data);
		}

		public static string ToSQLUpdate(this PurpleAccountWarnings data)
		{
			return to_sql_update (data, data.id);
		}

		public static string ToSQLDelete(this PurpleAccountWarnings data)
		{
			return to_sql_delete(data, data.id);
		}









		// PRIVATE /////////////

		private static string to_sql_select<T>(T data, int id, string guid)
		{
			return SQLGenerator.New ().Select ("*", get_table_name (data))
				.Where ("id=" + id).Where ("guid=" + guid, "OR").Single();
		}

		private static string to_sql_insert<T>(T data)
		{
			Dictionary<string, string> dict = convert_to_dictionary (data);
			return SQLGenerator.New ().Insert (get_table_name (data),
			                                   dictionary_to_insertarray(dict)).Single();
		}

		private static string to_sql_update<T>(T data, int id)
		{
			Dictionary<string, string> dict = convert_to_dictionary (data);
			return SQLGenerator.New ().Update (dictionary_to_updatearray (dict),
			                                   get_table_name (data)).Where ("id=" + id).Single();
		}

		private static string to_sql_delete<T>(T data, int id)
		{
			return SQLGenerator.New ().Delete(get_table_name (data)).Where ("id=" + id).Single();
		}


		// PRIVATE HELPER /////////////

		private static string[] dictionary_to_updatearray(Dictionary<string, string> dict)
		{
			List<string> returnvalue = new List<string> ();

			foreach(KeyValuePair<string, string> entry in dict)
			{
				if(entry.Key != "id")
					returnvalue.Add(entry.Key + "=" + entry.Value);
			}
			return returnvalue.ToArray();
		}

		private static string[] dictionary_to_insertarray(Dictionary<string, string> dict)
		{
			List<string> returnvalue = new List<string> ();

			foreach(KeyValuePair<string, string> entry in dict)
			{
				if(entry.Key != "id")
					returnvalue.Add(entry.Key + "=" + entry.Value);
			}
			return new string[] { string.Join(", ", returnvalue.ToArray()) };
		}


		private static string get_table_name<T>(T data)
		{
			string tableName = Regex.Replace (data.GetType().Name, "Purple", String.Empty);
			tableName = Regex.Replace (tableName, "(\\B[A-Z])", "_$1").ToLower();

			return PurpleConfig.Database.Prefix + tableName;
		}


		private static Dictionary<string, string> convert_to_dictionary<T>(T data)
		{
			string nullDate = "0001-01-01 00:00:00";
			string nullGUID = new Guid ().ToString();
			string nullValue = "NULL";

			Dictionary<string, string> dict = new Dictionary<string, string> ();
			
			foreach (PropertyInfo singleProperty in typeof(T).GetProperties()) 
			{ 
				try {
					string propertyValue = string.Empty;
					switch (Type.GetTypeCode(singleProperty.PropertyType))
					{
					case TypeCode.DateTime:
						DateTime dt = (DateTime)singleProperty.GetValue(data, null);
						DateTime dtNull = Convert.ToDateTime(nullDate);         
						
						if(!dt.Equals(dtNull) && !dt.Equals(DateTime.MinValue))
							propertyValue = dt.ToString("yyyy-MM-dd HH:mm:ss");
						break;
						
					case TypeCode.Boolean:
						bool value = Convert.ToBoolean(singleProperty.GetValue(data, null));
						propertyValue = (value) ? "1" : "0";
						break;
						
					default:
						object tmpPropertyValue = singleProperty.GetValue(data, null);
						if(tmpPropertyValue != null && tmpPropertyValue.ToString() != nullGUID)
							propertyValue = tmpPropertyValue.ToString();
						break;
					}
					
					if(!String.IsNullOrEmpty(propertyValue))
					{
						dict.Add(singleProperty.Name, propertyValue);
					}
					else
					{
						dict.Add(singleProperty.Name, nullValue);
					}
				} catch {
					continue;
				}
			}
			return dict;
		}

	}
}
