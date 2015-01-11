using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace PurpleConfig {
	public static class ItemIds {

		// config dictionary
		private static Dictionary<string,string> _configDictionary;

		// constructor - reads the config files and builds the dictionary
		static ItemIds(){
			_configDictionary = new Dictionary<string,string>();
			XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.

			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.config", SearchOption.AllDirectories);
			if(files.Length > 0)
			{
				foreach (string filePath in files) 
				{
					if(filePath.Contains("/Config/"))
					{
						xmlDoc.LoadXml (System.IO.File.ReadAllText(filePath)); // load the file.
						XmlNodeList nodesList = xmlDoc.GetElementsByTagName("add"); // array of the level nodes.
						foreach (XmlNode levelInfo in nodesList) 
						{
							_configDictionary.Add(levelInfo.Attributes["key"].Value,levelInfo.Attributes["value"].Value);
						}	
					}
				}
			} else {
				Debug.LogError("Can not find config files.");
			}
		}

		public static string getConfigString(string searchString)
		{
			string returnData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out returnData);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			return returnData;
		}
		
		public static int getConfigInt(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToInt32(tempData);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return -1;
			}
		}
		
		public static float getConfigFloat(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToSingle(tempData);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return -1;
			}
		}
		
		public static double getConfigDouble(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToDouble(tempData);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return -1;
			}
		}
		
		public static bool getConfigBoolean(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToBoolean(tempData);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return false;
			}
		}

	}
}
