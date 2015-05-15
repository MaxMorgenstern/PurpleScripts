using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEngine;

public class PurpleI18n : MonoBehaviour
{
	private static PurpleI18n instance;

	private static Dictionary<string,string> configDictionary;
	private static CultureInfo currentCulture;
	private static string defaultCulture;

	// constructor - reads the language files and builds the dictionary
	static PurpleI18n()
	{
		try{
			defaultCulture = PurpleConfig.Globalization.Culture;
		} catch(Exception e){
			defaultCulture = "en-GB" ;
			PurpleDebug.LogError("Can not read Purple Config! " + e.ToString(), 1);
		}

		configDictionary = new Dictionary<string,string>();
		currentCulture = new CultureInfo( defaultCulture );
	}


	// SINGLETON /////////////////////////
	private static PurpleI18n Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject 	= new GameObject ("PurpleI18nManager");
				instance     			= gameObject.AddComponent<PurpleI18n> ();
				instance.setup();
			}
			return instance;
		}
	}

	public static void Setup()
	{
		Instance.setup ();
	}

	public static void Setup(string culture)
	{
		Instance.setup (culture);
	}

	public static string Get(string key)
	{
		try{
			return Instance.get_entry (key.ToUpper());
		} catch(Exception e){
			PurpleDebug.LogError("Please call 'PurpleI18n.Setup()' initially! " + e.ToString(), 1);
		}
		return "I18n has not been setup for: '"+key+"'";
	}

	public static CultureInfo GetCulture()
	{
		return Instance.get_culture ();
	}


	// PRIVATE FUNCTIONS /////////////////////////
	private void setup()
	{
		setup (defaultCulture);
	}

	private void setup(string culture)
	{
		try {
			currentCulture = new CultureInfo (culture);
			configDictionary = new Dictionary<string,string>();

			XmlDocument xmlDoc = new XmlDocument();

			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),
												("*"+culture+"*.lang"), SearchOption.AllDirectories);
			if(files.Length > 0)
			{
				foreach (string filePath in files)
				{
					xmlDoc.LoadXml (System.IO.File.ReadAllText(filePath)); // load the file.
					XmlNodeList nodesList = xmlDoc.GetElementsByTagName("add"); // array of the level nodes.
					foreach (XmlNode levelInfo in nodesList)
					{
						configDictionary.Add(levelInfo.Attributes["key"].Value,levelInfo.Attributes["value"].Value);
					}
				}
			} else {
				PurpleDebug.LogError("Can not find config files.", 1);
			}
		} catch(Exception e) {
			PurpleDebug.LogError(e, 1);
		}
	}

	private string get_entry(string entry)
	{
		string returnData = String.Empty;
		try
		{
			configDictionary.TryGetValue(entry,out returnData);
		}
		catch (Exception e)
		{
			PurpleDebug.LogWarning(e.ToString());
		}
		return (String.IsNullOrEmpty(returnData)) ? "Missing Label: '"+entry+"'" : returnData;
	}

	private CultureInfo get_culture()
	{
		return currentCulture;
	}
}
