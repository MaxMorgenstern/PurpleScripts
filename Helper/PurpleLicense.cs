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
				}
				instance.create_new_key_pair();
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









		/*
		public static License CreateLicense (DateTime start, DateTime end, String productName, String userName, String privateKey)
		{
			LicenseTerm terms = new LicenseTerm()
			{
				StartDate = start,
				EndDate = end,
				Name = productName
			};

			DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();

			dsa.FromXmlString(privateKey);

			byte[] license = terms.GetLicenseData_OLD();

			byte[] signature = dsa.SignData(license);
			
			return new License()
			{
				LicenseTerms = Convert.ToBase64String(license),
				Signature = Convert.ToBase64String(signature)
			};
		}
		*/



		/*
		public static LicenseTerm GetLicenseDetails (License license, String publicKey)
		{
			DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
			dsa.FromXmlString(publicKey);
			
			byte[] terms = Convert.FromBase64String(license.LicenseTerms);
			
			byte[] signature = Convert.FromBase64String(license.Signature);
			
			if (dsa.VerifyData(terms, signature))
			{
				return LicenseTerm.FromString(license.LicenseTerms);
			}
			else
			{
				throw new SecurityException("Signature Not Verified!");
			}
		}
		*/




		public static bool ValidateLicense (License license)
		{
			return true;
		}



		public static LicenseTerm CreateLicenseTerm(DateTime start, DateTime end, string name, string key)
		{
			return Instance.create_license_term (start, end, name, key);
		}

		public static string GetLicenseTermKey(LicenseTerm license)
		{
			return Instance.get_license_key (license);
		}
		
		
		private LicenseTerm create_license_term(DateTime start, DateTime end, string name, string key)
		{
			LicenseTerm lt = new LicenseTerm (){
				StartDate = start,
				EndDate = end,
				Name = name,
				Key = encrypt_data_base64(key)
			};
			lt.Base64Hash = sign_data_base64 (lt.GetLicenseDetails ());
			return lt;
		}

		private string get_license_key(LicenseTerm license)
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
				// TODO: this has an error
				return validate_data (license.GetLicenseDetails (), license.Base64Hash);
			}
			return false;
		}
		
		
	}


	[Serializable]
	public class License
	{
		public string LicenseTerms;
		public string Signature;
		public string Name;

		public String Base64Hash;
	}

	
	[Serializable]
	public class LicenseTerm
	{
		public DateTime StartDate;
		public DateTime EndDate;

		public String Name;
		public String Key;

		public String Base64Hash;

		public string GetLicenseDetails()
		{
			string sd = StartDate.ToLongTimeString ();
			string ed = EndDate.ToLongTimeString ();
			string termDetails = sd + Name + Key + ed;
			return System.Convert.ToBase64String (Encoding.UTF8.GetBytes(termDetails));
		}
	}
}

