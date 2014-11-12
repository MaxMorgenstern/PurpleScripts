using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

// PlayerPrefs - Work in WebPlayer
//http://docs.unity3d.com/ScriptReference/PlayerPrefs.html

// as well as File saving - does not work in web player!?

// TODO: Test Web Player - make file work for all devices
#if UNITY_WEBPLAYER
#endif

// TODO: metadata... get them - name - size - when stored - when updated

namespace PurpleStorage
{
	public class PurpleStorage : MonoBehaviour
	{
		private static PurpleStorage instance;

		private static string fileEnding;
		private static string alternativePath;
		private static bool forcePlayerPrefs;
		private static string metaObjectName;


		// START UP /////////////////////////
		protected PurpleStorage ()
		{
			try{
				fileEnding = "."+PurpleConfig.Storage.File.Extension.TrimStart('.');
				forcePlayerPrefs = PurpleConfig.Storage.ForcePlayerPrefs;
				alternativePath = PurpleConfig.Storage.File.AlternativePath;
				metaObjectName = PurpleConfig.Storage.File.MetaName;
			} catch(Exception e){
				fileEnding = ".data";
				forcePlayerPrefs = false;
				alternativePath = String.Empty;
				metaObjectName = "purple_meta_object";
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

		public static bool SaveFile(string filename, string data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Instance.save_binary_file (filename, fileData);
		}

		public static bool SaveFile(string filename, object data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Instance.save_binary_file (filename, fileData);
		}

		public static bool SaveFile(string filename, PurpleFileObject data)
		{
			return Instance.save_binary_file (filename, data);
		}


		public static PurpleFileObject LoadFile(string filename)
		{
			return Instance.load_binary_file (filename);
		}

		public static string LoadFileString(string filename)
		{
			PurpleFileObject pfo = Instance.load_binary_file (filename);
			return pfo.dataString;
		}

		public static object LoadFileObject(string filename)
		{
			PurpleFileObject pfo = Instance.load_binary_file (filename);
			return pfo.dataObject;
		}



		// PRIVATE FUNCTIONS /////////////////////////

		private bool save_binary_file(string filename, PurpleFileObject data) 
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create (Application.persistentDataPath + "/" + filename + fileEnding);
				bf.Serialize(file, data);
				file.Close();
				return true;
			} 
			catch (Exception err)
			{
				Debug.Log("Got: " + err);
			}
			return false;
		}

		private PurpleFileObject load_binary_file(string filename) 
		{
			if(File.Exists(Application.persistentDataPath + "/" + filename + fileEnding)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/" + filename + fileEnding, FileMode.Open);
				PurpleFileObject data = (PurpleFileObject)bf.Deserialize(file);
				file.Close();
				return data;
			}
			return null;
		}

		private bool delete_binary_file(string filename)
		{
			// TODO
			return false;
		}


		// TODO: Test
		private bool save_player_pref(string filename, string data)
		{
			try 
			{
				PlayerPrefs.SetString(filename, data);
				return true;
			}
			catch (PlayerPrefsException err) 
			{
				Debug.Log("Got: " + err);
			}
			return false;
		}

		// TODO: Test
		private string load_player_pref(string filename)
		{
			if(PlayerPrefs.HasKey (filename))
				return PlayerPrefs.GetString (filename);

			return String.Empty;
		}
		
		// TODO: Test
		private bool delete_player_pref(string filename)
		{
			// TODO
			return false;
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


		private bool update_meta_object(string fileName)
		{
			/*PurpleMetaObject*/ string tmp_meta_object = load_meta_object ();
			if (tmp_meta_object != null) 
			{
				// TODO
			}

			// TODO
			return false;
		}

		private bool save_meta_object(PurpleMetaObject metaObject)
		{
			string data = ""; // TODO metaObject

			try 
			{
				PlayerPrefs.SetString(metaObjectName, data);
				return true;
			}
			catch (PlayerPrefsException err) 
			{
				Debug.Log("Got: " + err);
			}
			return false;
		}

		private /*PurpleMetaObject*/ string load_meta_object()
		{
			if(PlayerPrefs.HasKey (metaObjectName))
				return PlayerPrefs.GetString (metaObjectName);
			
			return null;
		}

	}



	[Serializable]
	public class PurpleMetaObject
	{
		public DateTime updated;
		public Array filelist;
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
