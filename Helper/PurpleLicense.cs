using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using UnityEngine;

// TODO : a lot!

namespace PurpleLicense
{
	public class PurpleLicense : MonoBehaviour
	{
		public static License CreateLicense (DateTime start, DateTime end, String productName, String userName, String privateKey)
		{
			LicenseTerm terms = new LicenseTerm()
			{
				StartDate = start,
				EndDate = end,
				ProductName = productName,
				UserName = userName
			};

			DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();

			dsa.FromXmlString(privateKey);

			byte[] license = terms.GetLicenseData();

			byte[] signature = dsa.SignData(license);
			
			return new License()
			{
				LicenseTerms = Convert.ToBase64String(license),
				Signature = Convert.ToBase64String(signature)
			};
		}

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

		public static bool ValidateLicense (License license, String publicKey)
		{
			return true;
		}

		public static LicenseTerm CreateTerm()
		{
			return new LicenseTerm (){};
		}

		public static bool ValidateKey()
		{
			return true;
		}

	}


	[Serializable]
	public class License
	{
		public string LicenseTerms;
		public string Signature;
		public string Key;
	}

	
	[Serializable]
	public class LicenseTerm
	{
		public DateTime StartDate;
		public DateTime EndDate;
		
		public String Name;
		public String Key;

		public String UserName;
		public String ProductName;

		public String GetLicenseString()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter bnfmt = new BinaryFormatter();
				bnfmt.Serialize(ms, this);
				return Convert.ToBase64String(ms.GetBuffer());
			}
		}

		public byte[] GetLicenseData()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter bnfmt = new BinaryFormatter();
				bnfmt.Serialize(ms, this);
				return ms.GetBuffer();	
			}
		}

		internal static LicenseTerm FromString(String licenseTerms)
		{
			using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(licenseTerms)))
			{
				BinaryFormatter bnfmt = new BinaryFormatter();
				object value = bnfmt.Deserialize(ms);
				
				if (value is LicenseTerm)
				{
					return (LicenseTerm)value;
				}
				else
				{
					throw new ApplicationException("Invalid Type!");
				}
			}
		}	
	}
}

