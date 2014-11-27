using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using UnityEngine;

// TODO : a lot!
using System.Text;
using System.Collections.Generic;

namespace PurpleLicense
{
	public class PurpleLicense : MonoBehaviour
	{
		private static RSACryptoServiceProvider _rsaProvider;
		private static string _rsaPrivateKey;
		private static string _rsaPublicKey;
		
		private static PurpleLicense instance;
		
		private static int keySize;
		private static string cryptoConfig;


		// START UP /////////////////////////
		protected PurpleLicense ()
		{
			try{
				keySize = PurpleConfig.License.KeySize;
				cryptoConfig = PurpleConfig.License.CryptoConfig;
			} catch(Exception e){
				keySize = 2048;
				cryptoConfig = "SHA256";
				Debug.LogError("Can not read Purple Config! " + e.ToString());
			}
		}


		// SINGLETON /////////////////////////
		public static PurpleLicense Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject 	= new GameObject ("PurpleLicenseManager");
					instance     			= gameObject.AddComponent<PurpleLicense> ();
					instance.create_new_key_pair();
				}
				return instance;
			}
		}


		// VARIABLES ////////////////////////////

		public static string PrivateKey
		{
			get
			{
				if(_rsaProvider == null)
				{
					Instance.create_new_key_pair();
				}
				return _rsaPrivateKey;
			}
		}

		public static string PublicKey
		{
			get
			{
				if(_rsaProvider == null)
				{
					Instance.create_new_key_pair();
				}
				return _rsaPublicKey;
			}
		}


		// PUBLIC ////////////////////////////
		public static void CreateKeyPair()
		{
			Instance.create_new_key_pair (keySize);
		}

		public static void CreateKeyPair(int keySize)
		{
			Instance.create_new_key_pair (keySize);
		}
		
		public static void SetLicenseKey(string XMLKey)
		{
			Instance.set_key_pair_from_xml (XMLKey);
		}

		
		// License Term ////////////////////////////

		public static LicenseTerm CreateLicenseTerm(DateTime start, DateTime end, string name, string key)
		{
			return Instance.create_license_term (start, end, name, key);
		}
		
		public static bool ValidateLicenseTerm (LicenseTerm licenseTerm)
		{
			return Instance.validate_license_term (licenseTerm);
		}
		
		public static string GetLicenseTermKey(LicenseTerm licenseTerm)
		{
			return Instance.get_license_term_key (licenseTerm);
		}



		// PRIVATE ////////////////////////////

		private void create_new_key_pair()
		{
			create_new_key_pair (keySize);
		}
		
		private void create_new_key_pair(int keySize)
		{
			_rsaProvider = new RSACryptoServiceProvider(keySize);
			_rsaPrivateKey = _rsaProvider.ToXmlString(true);
			_rsaPublicKey = _rsaProvider.ToXmlString(false);
		}

		private void set_key_pair_from_xml(string XMLKey)
		{
			_rsaProvider = new RSACryptoServiceProvider(keySize);
			_rsaProvider.FromXmlString(XMLKey);
			_rsaPrivateKey = _rsaProvider.ToXmlString(true);
			_rsaPublicKey = _rsaProvider.ToXmlString(false);
		}


		// Hash + Crypting ////////////////////////////

		private string sign_data_base64(string data)
		{
			var dataToSign = Encoding.UTF8.GetBytes(data);
			var dataSigned = _rsaProvider.SignData (dataToSign, CryptoConfig.CreateFromName(cryptoConfig));
			return System.Convert.ToBase64String (dataSigned);
		}

		private bool validate_data(string data, string base64SignedData)
		{	
			return _rsaProvider.VerifyData (Encoding.UTF8.GetBytes (data), CryptoConfig.CreateFromName (cryptoConfig), System.Convert.FromBase64String (base64SignedData));
		}

		private string encrypt_data_base64(string data)
		{
			return System.Convert.ToBase64String (_rsaProvider.Encrypt(Encoding.UTF8.GetBytes(data), false));
		}

		private string decrypt_data_base64(string data)
		{
			return Encoding.UTF8.GetString(_rsaProvider.Decrypt(System.Convert.FromBase64String(data), false));
		}

		
		// License Term ////////////////////////////

		private LicenseTerm create_license_term(DateTime start, DateTime end, string name, string key)
		{
			LicenseTerm lt = new LicenseTerm (){
				StartDate = start,
				EndDate = end,
				Name = name,
				Key = encrypt_data_base64(key)
			};
			lt.Base64Hash = sign_data_base64 (lt.GetReferenceString ());
			return lt;
		}
		
		private string get_license_term_key(LicenseTerm license)
		{
			if(validate_license_term(license))
			{
				return decrypt_data_base64(license.Key);
			}
			return String.Empty;
		}
		
		private bool validate_license_term(LicenseTerm license)
		{
			if(license.StartDate <= DateTime.Now && license.EndDate >= DateTime.Now)
			{
				return validate_data (license.GetReferenceString (), license.Base64Hash);
			}
			return false;
		}













		public static void Test()
		{
			Instance.test ();
		}

		private void test()
		{
			LicenseTerm ltOne = create_license_term(DateTime.MinValue, DateTime.MaxValue, "DummynameOne", "DummykeyOne");
			LicenseTerm ltTwo = create_license_term(DateTime.MinValue, DateTime.MaxValue, "DummynameTwo", "DummykeyTwo");
			LicenseTerm ltdummy = create_license_term(DateTime.MinValue, DateTime.MaxValue, "DummynameDummy", "DummykeyDummy");

			License li = create_license ("Dummyname");
			li = add_term (li, ltOne);
			li = add_term (li, ltTwo);
			li = add_term (li, ltdummy);
			Debug.Log (li.GetReferenceString());

			li = remove_term (li, "DummynameTwo");

			Debug.LogWarning (validate_license(li));

			List<LicenseTerm> ltlist = get_license_terms (li);

			foreach (LicenseTerm lt in ltlist)
			{
				Debug.Log (lt.Name);
			}
		}
		// TODO: add public methods


		// License ////////////////////////////
		
		private License create_license(string name)
		{
			License li = new License (){
				Name = name
			};
			li.Base64Hash = sign_data_base64 (li.GetReferenceString ());
			return li;
		}

		private License add_term(License license, LicenseTerm licenseTerm)
		{
			license.AddTerm (licenseTerm);
			license.Base64Hash = sign_data_base64 (license.GetReferenceString ());
			return license;
		}

		private License remove_term(License license, LicenseTerm licenseTerm)
		{
			return remove_term (license, licenseTerm.Name);
		}

		private License remove_term(License license, string licenseName)
		{
			license.RemoveTerm (licenseName);
			license.Base64Hash = sign_data_base64 (license.GetReferenceString ());
			return license;
		}

		private bool validate_license(License license)
		{
			return validate_data (license.GetReferenceString (), license.Base64Hash);
		}

		private List<LicenseTerm> get_license_terms(License license)
		{
			if(validate_license(license))
			{
				// TODO: validate terms
				return license.GetLicenseTermList();
			}
			return null;
		}

		
	}


	[Serializable]
	public class License
	{
		private List<LicenseTerm> LicenseTermList = new List<LicenseTerm>();

		public string Name;
		public string Base64Hash;

		public void AddTerm(LicenseTerm licenseTerm)
		{
			LicenseTermList.Add (licenseTerm);
		}

		public void RemoveTerm(string licenseName)
		{
			LicenseTermList.Remove (LicenseTermList.Find (x => x.Name == licenseName));
		}

		public string GetReferenceString()
		{
			return GetReferenceString (false);
		}
		
		public string GetReferenceString(bool extended)
		{
			string termDetails = String.Empty;
			var salt = String.Empty;
			if(extended)
			{
				salt = Base64Hash;
			}

			foreach (LicenseTerm lt in LicenseTermList)
			{
				termDetails += lt.GetReferenceString(true);
			}
			return Name + termDetails + salt;
		}

		public List<LicenseTerm> GetLicenseTermList()
		{
			return LicenseTermList;
		}
	}

	
	[Serializable]
	public class LicenseTerm
	{
		public DateTime StartDate;
		public DateTime EndDate;

		public String Name;
		public String Key;

		public String Base64Hash;

		public string GetReferenceString()
		{
			return GetReferenceString (false);
		}

		public string GetReferenceString(bool extended)
		{
			var salt = String.Empty;
			if(extended)
			{
				salt = Base64Hash;
			}

			string sd = StartDate.ToLongTimeString ();
			string ed = EndDate.ToLongTimeString ();
			string termDetails = sd + Name + Key + salt + ed;
			return System.Convert.ToBase64String (Encoding.UTF8.GetBytes(termDetails));
		}
	}
}

