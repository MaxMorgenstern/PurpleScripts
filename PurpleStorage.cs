using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using _PurpleSerializer = PurpleSerializer;


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
		private static string filePath;
		private static bool forcePlayerPrefs;
		private static string metaObjectName;
		private static bool usePlayerPrefs;

		// START UP /////////////////////////
		protected PurpleStorage ()
		{
			try{
				fileEnding = "."+PurpleConfig.Storage.File.Extension.TrimStart('.');
				forcePlayerPrefs = PurpleConfig.Storage.ForcePlayerPrefs;
				filePath = PurpleConfig.Storage.File.AlternativePath;
				metaObjectName = PurpleConfig.Storage.File.MetaName;
			} catch(Exception e){
				fileEnding = ".data";
				forcePlayerPrefs = false;
				filePath = String.Empty;
				metaObjectName = "purple_meta_object";
				Debug.LogError("Can not read Purple Config! " + e.ToString());
			}
			usePlayerPrefs = true;
			if(String.IsNullOrEmpty(filePath))
			{
				filePath = Application.persistentDataPath;
			}
		}


		// SINGLETON /////////////////////////
		private static PurpleStorage Instance
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

		public static bool SwitchUsePlayerPrefs()
		{
			if(usePlayerPrefs)
			{
				return SwitchUsePlayerPrefs (false);
			}
			else
			{
				return SwitchUsePlayerPrefs (true);
			}
		}

		public static bool SwitchUsePlayerPrefs(bool usePPref)
		{
			usePlayerPrefs = usePPref;
			return usePlayerPrefs;
		}


		public static bool SaveFile(string filename, string data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return SaveFile (filename, fileData);
		}

		public static bool SaveFile(string filename, object data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return SaveFile (filename, fileData);
		}

		public static bool SaveFile(string filename, PurpleFileObject data)
		{
			if(usePlayerPrefs || forcePlayerPrefs)
			{
				try
				{
					return Instance.save_player_pref(filename, data);
				} 
				catch (Exception ex) 
				{
					usePlayerPrefs = false;
					return Instance.save_binary_file (filename, data);
				}
			}
			else
			{
				return Instance.save_binary_file (filename, data);
			}
		}


		public static PurpleFileObject LoadFile(string filename)
		{
			return Instance.load_pfo_helper (filename, usePlayerPrefs);
		}

		public static string LoadFileString(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename, usePlayerPrefs);
			return pfo.dataString;
		}

		public static object LoadFileObject(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename, usePlayerPrefs);
			return pfo.dataObject;
		}

		public static PurpleFileObject LoadPlayerPref(string filename)
		{
			return Instance.load_pfo_helper (filename, true);
		}

		public static PurpleFileObject LoadBinaryFile(string filename)
		{
			return Instance.load_pfo_helper (filename, false);
		}


		public static bool DeleteFile(string filename)
		{
			if(usePlayerPrefs)
			{
				return Instance.delete_player_pref(filename);
			}
			else
			{
				return Instance.delete_binary_file (filename);
			}
		}
		
		public static bool DeletePlayerPref(string filename)
		{
			return Instance.delete_player_pref(filename);
		}
		
		public static bool DeleteBinaryFile(string filename)
		{
			return Instance.delete_binary_file (filename);
		}


		// PRIVATE FUNCTIONS /////////////////////////

		private PurpleFileObject load_pfo_helper(string filename, bool usePPref)
		{
			PurpleFileObject pfo;
			if(usePPref)
			{
				pfo = load_player_pref<PurpleFileObject> (filename);
			}
			else
			{
				pfo = load_binary_file<PurpleFileObject> (filename);
			}
			return pfo;
		}

		private bool save_binary_file<T>(string filename, T data)
		{
			if (String.IsNullOrEmpty (filename)) return false;
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create (filePath + "/" + filename + fileEnding);
				bf.Serialize(file, data);
				file.Close();
				return true;
			}
			catch (Exception ex)
			{
				Debug.Log("Can not save file: " + ex);
			}
			return false;
		}

		private T load_binary_file<T>(string filename)
		{
			if (String.IsNullOrEmpty (filename)) return default (T);
			if(File.Exists(filePath + "/" + filename + fileEnding)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(filePath + "/" + filename + fileEnding, FileMode.Open);
				T data = (T)bf.Deserialize(file);
				file.Close();
				return data;
			}
			return default (T);
		}

		private bool delete_binary_file(string filename)
		{
			if (String.IsNullOrEmpty (filename)) return false;
			if (File.Exists (filePath + "/" + filename + fileEnding)) {
				File.Delete (filePath + "/" + filename + fileEnding);
				return true;
			}
			return false;
		}

		private bool save_player_pref<T>(string filename, T data)
		{
			if (String.IsNullOrEmpty (filename)) return false;
			string data_string = _PurpleSerializer.ObjectToStringConverter (data);
			try
			{
				PlayerPrefs.SetString(filename, data_string);
				return true;
			}
			catch (PlayerPrefsException ex)
			{
				Debug.Log("Can not save PlayerPref: " + ex);
			}
			return false;
		}

		private T load_player_pref<T>(string filename)
		{
			if (String.IsNullOrEmpty (filename)) return default (T);
			if(!PlayerPrefs.HasKey (filename)) return default (T);

			string data_string = PlayerPrefs.GetString (filename);
			try
			{
				return _PurpleSerializer.StringToObjectConverter<T> (data_string);
			}
			catch(PurpleException ex)
			{
				Debug.LogError("Can not convert PurpleFileObject " + ex);
			}
			return default (T);
		}

		private bool delete_player_pref(string filename)
		{
			if (String.IsNullOrEmpty (filename)) return false;
			if(!PlayerPrefs.HasKey (filename)) return false;
			try
			{
				PlayerPrefs.DeleteKey (filename);
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Can not delete PlayerPref " + ex);
			}
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




		private PurpleMetaObject create_purple_meta_object(string filename)
		{
			PurpleMetaObject pm_object = new PurpleMetaObject ();
			pm_object.updated = DateTime.Now;
			return pm_object;
		}

// TODO... combine with upper functions...
		// TODO: meta object as file or player pref

		// this is the only aditional function
		private bool update_meta_object(string fileName)
		{
			PurpleMetaObject tmp_meta_object = load_meta_object ();
			if (tmp_meta_object != null)
			{
				// TODO

			}

			// TODO
			return false;
		}

		private bool save_meta_object(PurpleMetaObject metaObject)
		{
			string data = _PurpleSerializer.ObjectToStringConverter (metaObject);
			try
			{
				PlayerPrefs.SetString(metaObjectName, data);
				return true;
			}
			catch (PlayerPrefsException ex)
			{
				Debug.Log("Got: " + ex);
			}
			return false;
		}

		private PurpleMetaObject load_meta_object()
		{
			string purpleObjectString = String.Empty;
			try
			{
				if(PlayerPrefs.HasKey (metaObjectName))
				{
					purpleObjectString = PlayerPrefs.GetString (metaObjectName);
					return _PurpleSerializer.StringToObjectConverter<PurpleMetaObject> (purpleObjectString);
				}
			}
			catch(PurpleException ex)
			{
				Debug.LogWarning("Can not convert meta data object! " + ex);
			}
			return null;
		}

	}



	[Serializable]
	public class PurpleMetaObject
	{
		public Guid guid;
		public string hashValue;

		public DateTime updated;
		public string[] filelist;		// TODO... length unknown
	}

	[Serializable]
	public class PurpleFileObject
	{
		public Guid guid;
		public string name;
		public string hashValue;

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
