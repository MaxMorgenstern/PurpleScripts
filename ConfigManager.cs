using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ConfigManager {
	public static class ItemIds {

		// Networking
		public static class Network {
			public static string IP { get { return getConfigString ("Network.IP"); } }
			public static string Host { get { return getConfigString ("Network.Host"); } }
			public static int Port { get { return getConfigInt ("Network.Port"); } }
			public static int MaxPlayer { get { return getConfigInt ("Network.MaxPlayer"); } }

			public static class MasterServer {
				public static string GameName { get { return getConfigString ("Network.MasterServer.GameName"); } }
				public static string GameType { get { return getConfigString ("Network.MasterServer.GameType"); } }
				public static string ServerURL { get { return getConfigString ("Network.MasterServer.URL"); } }
			}
		}

		// Database
		public static class Database {
			public static string IP { get { return getConfigString ("Database.IP"); } }
			public static string Name { get { return getConfigString ("Database.Name"); } }
			public static string User { get { return getConfigString ("Database.User"); } }
			public static string Password { get { return getConfigString ("Database.Password"); } }
		}

		// Password generation/validation
		public static class Password {
			public static int SaltByteSize { get { return getConfigInt ("Password.SaltByteSize"); } }
			public static int HashByteSize { get { return getConfigInt ("Password.HashByteSize"); } }
			public static int IterationIndex { get { return getConfigInt ("Password.IterationIndex"); } }
			public static int SaltIndex { get { return getConfigInt ("Password.SaltIndex"); } }

			public static class PBKDF2 {
				public static int Iterations { get { return getConfigInt ("Password.PBKDF2.Iterations"); } }
				public static int Index { get { return getConfigInt ("Password.PBKDF2.Index"); } }
			}
		}





		// TODO: Better Debug

		/**************
		 * Functions in order to pass the config variables
		 *************/

		// constructor - reads the config files and builds the dictionary
		static ItemIds(){
			_configDictionary = new Dictionary<string,string>();
			XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.

			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*PurpleSettings*.config", SearchOption.AllDirectories);
			if(files.Length > 0)
			{
				foreach (string filePath in files) 
				{
					xmlDoc.LoadXml (System.IO.File.ReadAllText(filePath)); // load the file.
					XmlNodeList nodesList = xmlDoc.GetElementsByTagName("add"); // array of the level nodes.
					foreach (XmlNode levelInfo in nodesList) 
					{
						_configDictionary.Add(levelInfo.Attributes["key"].Value,levelInfo.Attributes["value"].Value);
					}	
				}
			} else {
				Debug.LogError("Can not find config files.");
			}
		}

		// config dictionary
		private static Dictionary<string,string> _configDictionary;

		private static string getConfigString(string searchString)
		{
			string returnData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out returnData);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
			}
			return returnData;
		}
		
		private static int getConfigInt(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToInt32(tempData);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
				return -1;
			}
		}
		
		private static float getConfigFloat(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToSingle(tempData);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
				return -1;
			}
		}
		
		private static double getConfigDouble(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToDouble(tempData);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
				return -1;
			}
		}
		
		private static bool getConfigBoolean(string searchString)
		{
			string tempData = string.Empty;
			try
			{
				_configDictionary.TryGetValue(searchString,out tempData);
				return Convert.ToBoolean(tempData);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
				return false;
			}
		}

	}
}
