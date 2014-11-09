using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;


// PlayerPrefs - Work in WebPlayer
//http://docs.unity3d.com/ScriptReference/PlayerPrefs.html 

// as well as File saving - does not work in web player!?

// perhaps own object for storage???

// how to format data

namespace PurpleStorage
{
	public class PurpleStorage : MonoBehaviour
	{
		private static PurpleStorage instance;

		private static string fileEnding;
		private static bool binaryFormat;
		private static string alternativePath;
		private static bool forcePlayerPrefs;


		// START UP /////////////////////////
		protected PurpleStorage ()
		{
			try{
				fileEnding = "."+PurpleConfig.Storage.File.Extension.TrimStart('.');
				forcePlayerPrefs = PurpleConfig.Storage.ForcePlayerPrefs;		// ???
				alternativePath = PurpleConfig.Storage.File.AlternativePath;	// alt store path
				binaryFormat = PurpleConfig.Storage.File.Binary;				// binary format or raw
			} catch(Exception e){
				fileEnding = ".data";
				forcePlayerPrefs = false;
				alternativePath = String.Empty;
				binaryFormat = true;
				Debug.LogError("Can not read Purple Config! " + e.ToString());
			}
		}


		// SINGLETON /////////////////////////
		public static PurpleStorage Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleStorageManager");
					instance     			= gameObject.AddComponent<PurpleStorage> ();
				}
				return instance;
			}
		}

	
		// PUBLIC FUNCTIONS /////////////////////////

		public static void SaveFile(string filename, string data)
		{
			PurpleFileObject fileData = create_purple_file_object (filename, data);
			Instance.save_binary_file (filename, fileData);
		}

		public static void SaveFile(string filename, object data)
		{
			PurpleFileObject fileData = create_purple_file_object (filename, data);
			Instance.save_binary_file (filename, fileData);
		}

		public static void SaveFile(string filename, PurpleFileObject data)
		{
			Instance.save_binary_file (filename, data);
		}


		public static PurpleFileObject LoadFile(string filename)
		{
			return Instance.load_binary_file (filename);
		}


		
		// PRIVATE FUNCTIONS /////////////////////////

		private void save_binary_file(string filename, PurpleFileObject data) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + "/" + filename + fileEnding);
			bf.Serialize(file, data);
			file.Close();
		}   

		private PurpleFileObject load_binary_file(string filename) {
			if(File.Exists(Application.persistentDataPath + "/" + filename + fileEnding)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/" + filename + fileEnding, FileMode.Open);
				PurpleFileObject data = (PurpleFileObject)bf.Deserialize(file);
				file.Close();
				return data;
			}
			return null;
		}


		
		// PRIVATE HELPER /////////////////////////

		private PurpleFileObject create_purple_file_object(string filename, string dataString)
		{
			return create_purple_file_object (filename, dataString, null);
		}

		private PurpleFileObject create_purple_file_object(string filename, object dataObject)
		{
			return create_purple_file_object (filename, String.Empty, dataObject);
		}

		private PurpleFileObject create_purple_file_object(string filename, string dataString, object dataObject)
		{
			PurpleFileObject pf_object = new PurpleFileObject ();
			pf_object.created = DateTime.Now;
			pf_object.updated = DateTime.Now;

			pf_object.name = filename;
			
			if(!String.IsNullOrEmpty(dataString))
				pf_object.dataString = dataString;

			if(dataObject != null)
				pf_object.dataObject = dataObject;

			return pf_object;
		}

	}


	[Serializable]
	public class PurpleFileObject
	{
		public Guid guid;
		public string name;

		public DateTime created;
		public DateTime updated;

		public string dataString;
		public object dataObject;
		
		// CONSTRUCTOR
		public PurpleFileObject()
		{
			guid = System.Guid.NewGuid ();
		}
	}
}
