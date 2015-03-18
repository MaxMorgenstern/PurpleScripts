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
		// TODO - SELECT: PurpleAccount + id

		public static string ToSQLInsert(this PurpleAccount data)  
		{
			Dictionary<string, string> dict = convert_to_dictionary (data);

			return SQLGenerator.New ().Insert (get_table_name (data), 
						dictionary_to_insertarray(dict));
		}
		
		public static string ToSQLUpdate(this PurpleAccount data) 
		{
			Dictionary<string, string> dict = convert_to_dictionary (data);

			return SQLGenerator.New ().Update (dictionary_to_updatearray (dict), 
						get_table_name (data)).Where ("id=" + data.id);
		}
		
		public static string ToSQLDelete(this PurpleAccount data) 
		{
			return SQLGenerator.New ().Delete(get_table_name (data))
						.Where ("id=" + data.id);
		}









		public static string ToSQLInsertTEST(this PurpleAccount data)  
		{
			return to_sql_insert (data);
		}

		public static string ToSQLUpdateTEST(this PurpleAccount data) 
		{
			return to_sql_update (data, data.id);
		}

		public static string ToSQLDeleteTEST(this PurpleAccount data) 
		{
			return to_sql_delete(data, data.id);
		}





		


		
		// PRIVATE /////////////

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
				returnvalue.Add(entry.Key + "=" + entry.Value);
			}
			return returnvalue.ToArray();
		}

		private static string[] dictionary_to_insertarray(Dictionary<string, string> dict)
		{
			List<string> returnvalue = new List<string> ();
			
			foreach(KeyValuePair<string, string> entry in dict)
			{
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


		private static Dictionary<string, string> convert_to_dictionary<T>(T account)
		{
			Type classtype = typeof(T); 
			Dictionary<string, string> dict = new Dictionary<string, string> ();
			foreach (PropertyInfo singleProperty in classtype.GetProperties())  
			{  
				try {
					dict.Add(singleProperty.Name, 
					         account.GetType().GetProperty(singleProperty.Name).GetValue(account, null).ToString());
				} catch {
					continue;
				}
			}
			return dict;
		}

	}
}
