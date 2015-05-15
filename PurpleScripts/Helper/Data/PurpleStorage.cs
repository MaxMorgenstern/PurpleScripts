using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using _PurpleSerializer = PurpleSerializer;


// PlayerPrefs - Work in WebPlayer
//http://docs.unity3d.com/ScriptReference/PlayerPrefs.html

// as well as File saving - does not work in web player!?
// TODO: Test Web Player - make file work for all devices
#if UNITY_WEBPLAYER
#endif
// Note: meta files only available for ppref

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
				PurpleDebug.LogError("Can not read Purple Config! " + e.ToString(), 1);
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

		public static void Setup()
		{
			Instance.get_ppref_setting ();
		}

		public static bool SwitchUsePlayerPrefs()
		{
			if(Instance.get_ppref_setting())
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
			return Instance.switch_player_pref (usePPref);
		}


		// SAVE /////////////////////////
		public static bool SavePlayerPref(string filename, string data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			try
			{
				return Instance.save_player_pref(filename, fileData);
			}
			catch (Exception ex)
			{
				PurpleDebug.LogWarning(ex);
				return false;
			}
		}

		public static bool SavePlayerPref(string filename, object data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			try
			{
				return Instance.save_player_pref(filename, fileData);
			}
			catch (Exception ex)
			{
				PurpleDebug.LogWarning(ex);
				return false;
			}
		}

		public static bool SavePlayerPref<t>(string filename, t data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			try
			{
				return Instance.save_player_pref(filename, fileData);
			}
			catch (Exception ex)
			{
				PurpleDebug.LogWarning(ex);
				return false;
			}
		}


		public static bool SaveBinaryFile(string filename, string data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Instance.save_binary_file (filename, fileData);
		}

		public static bool SaveBinaryFile(string filename, object data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Instance.save_binary_file (filename, fileData);
		}

		public static bool SaveBinaryFile<t>(string filename, t data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Instance.save_binary_file (filename, fileData);
		}


		public static bool Save(string filename, string data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Save (filename, fileData);
		}

		public static bool Save(string filename, object data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Save (filename, fileData);
		}

		public static bool Save<t>(string filename, t data)
		{
			PurpleFileObject fileData = Instance.create_purple_file_object (filename, data);
			return Save (filename, fileData);
		}

		public static bool Save(string filename, PurpleFileObject data)
		{
			return Instance.save_pfo_helper (filename, data);
		}


		// LOAD /////////////////////////
		public static PurpleFileObject Load(string filename)
		{
			return Instance.load_pfo_helper (filename);
		}

		public static t Load<t>(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename);
			if(pfo != null)
				return (t)_PurpleSerializer.StringToObjectConverter<t> (pfo.dataString);
			return default(t);
		}

		public static string LoadString(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename);
			if(pfo != null)
				return pfo.dataString;
			return string.Empty;
		}

		public static object LoadObject(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename);
			if(pfo != null)
				return pfo.dataObject;
			return null;
		}

		public static t LoadObject<t>(string filename)
		{
			return Load<t>(filename);
		}

		public static PurpleFileObject LoadPlayerPref(string filename)
		{
			return Instance.load_pfo_helper (filename, true);
		}

		public static t LoadPlayerPref<t>(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename, true);
			if(pfo != null)
				return (t)_PurpleSerializer.StringToObjectConverter<t> (pfo.dataString);
			return default(t);
		}

		public static PurpleFileObject LoadBinaryFile(string filename)
		{
			return Instance.load_pfo_helper (filename, false);
		}

		public static t LoadBinaryFile<t>(string filename)
		{
			PurpleFileObject pfo = Instance.load_pfo_helper (filename, false);
			if(pfo != null)
				return (t)_PurpleSerializer.StringToObjectConverter<t> (pfo.dataString);
			return default(t);
		}

		// DELETE /////////////////////////
		public static bool DeleteFile(string filename)
		{
			return Instance.delete_pfo_helper (filename);
		}

		public static bool DeletePlayerPref(string filename)
		{
			return Instance.delete_player_pref(filename);
		}

		public static bool DeleteBinaryFile(string filename)
		{
			return Instance.delete_binary_file (filename);
		}


		// META /////////////////////////
		public static PurpleMetaObject CreateMetaObject()
		{
			return Instance.create_purple_meta_object ();
		}

		public static bool SaveMetaObject(PurpleMetaObject metaObject)
		{
			return Instance.save_meta_object(metaObject);
		}

		public static PurpleMetaObject LoadMetaObject()
		{
			return Instance.load_meta_object ();
		}

		public static bool UpdateMetaObject(PurpleMetaObject metaObject)
		{
			return Instance.update_meta_object (metaObject);
		}

		public static bool UpdateMetaObject(string data)
		{
			return Instance.update_meta_object (data);
		}

		public static bool UpdateMetaObject(string data, bool add)
		{
			return Instance.update_meta_object (data, add);
		}



		// PRIVATE FUNCTIONS /////////////////////////
		private bool save_pfo_helper(string filename, PurpleFileObject data)
		{
			if(usePlayerPrefs || forcePlayerPrefs)
			{
				try
				{
					return save_player_pref(filename, data);
				}
				catch (Exception ex)
				{
					PurpleDebug.LogWarning(ex);
					usePlayerPrefs = false;
					return save_binary_file (filename, data);
				}
			}
			else
			{
				return save_binary_file (filename, data);
			}
		}

		private bool delete_pfo_helper(string filename)
		{
			if(usePlayerPrefs || forcePlayerPrefs)
			{
				return delete_player_pref(filename);
			}
			else
			{
				return delete_binary_file (filename);
			}
		}

		private PurpleFileObject load_pfo_helper(string filename)
		{
			bool boolDisjunktion = (usePlayerPrefs || forcePlayerPrefs) ? true : false;
			return load_pfo_helper (filename, boolDisjunktion);
		}

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
				PurpleDebug.Log("Can not save file: " + ex, 1);
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
				PurpleDebug.Log("Can not save PlayerPref: " + ex, 1);
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
				PurpleDebug.LogError("Can not convert PurpleFileObject " + ex);
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
				PurpleDebug.LogError("Can not delete PlayerPref " + ex, 1);
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

		private PurpleFileObject create_purple_file_object<t>(string filename, t dataObject)
		{
			return create_purple_file_object (filename, _PurpleSerializer.ObjectToStringConverter (dataObject), null);
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

		private bool get_ppref_setting()
		{
			return usePlayerPrefs;
		}

		private bool switch_player_pref(bool usePPref)
		{
			usePlayerPrefs = usePPref;
			return usePlayerPrefs;
		}


		// PRIVATE META OBJECTS /////////////////////////

		private PurpleMetaObject create_purple_meta_object()
		{
			PurpleMetaObject pm_object = new PurpleMetaObject ();
			pm_object.updated = DateTime.Now;
			return pm_object;
		}

		private bool update_meta_object(string data)
		{
			return update_meta_object (data, true);
		}

		private bool update_meta_object(string data, bool add)
		{
			PurpleMetaObject tmp_meta_object = load_meta_object ();
			if (tmp_meta_object != null)
			{
				if(add)
				{
					tmp_meta_object.filelist.Add(data);
				}
				else
				{
					tmp_meta_object.filelist.Remove(data);

				}
				return update_meta_object(tmp_meta_object);
			}
			return false;
		}

		private bool update_meta_object(PurpleMetaObject metaObject)
		{
			return save_meta_object (metaObject);
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
				PurpleDebug.Log("Got: " + ex);
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
				PurpleDebug.LogWarning("Can not convert meta data object! " + ex);
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
		public List<string> filelist;

		// CONSTRUCTOR
		public PurpleMetaObject()
		{
			guid = System.Guid.NewGuid ();
			filelist = new List<string>();
		}
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
