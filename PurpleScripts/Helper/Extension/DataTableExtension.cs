using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public static class DataTableExtension
{
	public static List<T> ToList<T>(this DataTable dt)
	{
		try
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}
		catch
		{
			return null;
		}
	}

	public static T ToObject<T>(this DataRow dr)
	{
		try
		{
			return GetItem<T>(dr);
		}
		catch
		{
			return default(T);
		}
	}


	// PRIVATE ////////////////////////////

	private static T GetItem<T>(DataRow dr)
	{
		Type temp = typeof(T);
		T obj = Activator.CreateInstance<T>();

		foreach (DataColumn column in dr.Table.Columns)
		{
			foreach (PropertyInfo pro in temp.GetProperties())
			{
				try
				{
					if (pro.Name == column.ColumnName)
						pro.SetValue(obj, dr[column.ColumnName], null);
				}
				catch
				{
					continue;
				}
			}
		}
		return obj;
	}
}
