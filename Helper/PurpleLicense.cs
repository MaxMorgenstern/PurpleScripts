using UnityEngine;
using System.Collections;
using System.ComponentModel;


// http://msdn.microsoft.com/en-us/library/system.componentmodel.licenseprovider.aspx
// http://stackoverflow.com/questions/2896888/developing-licenses-in-c-sharp-where-do-i-start

/*
public static License CreateLicense(DateTime start, DateTime end, String productName, String userName, String privateKey)
{
	// create the licence terms:
	LicenseTerms terms = new LicenseTerms()
	{
		StartDate = start,
		EndDate = end,
		ProductName = productName,
		UserName = userName
	};
	
	// create the crypto-service provider:
	DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
	
	// setup the dsa from the private key:
	dsa.FromXmlString(privateKey);
	
	// get the byte-array of the licence terms:
	byte[] license = terms.GetLicenseData();
	
	// get the signature:
	byte[] signature = dsa.SignData(license);
	
	// now create the license object:
	return new License()
	{
		LicenseTerms = Convert.ToBase64String(license),
		Signature = Convert.ToBase64String(signature)
	};
}

internal class License()
{
	public string LicenseTerms;
	public string Signature;
}




internal static LicenseTerms GetValidTerms(License license, String publicKey)
{
	// create the crypto-service provider:
	DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
	
	// setup the provider from the public key:
	dsa.FromXmlString(publicKey);
	
	// get the license terms data:
	byte[] terms = Convert.FromBase64String(license.LicenseTerms);
	
	// get the signature data:
	byte[] signature = Convert.FromBase64String(license.Signature);
	
	// verify that the license-terms match the signature data
	if (dsa.VerifyData(terms, signature))
		return LicenseTerms.FromString(license.LicenseTerms);
	else
		throw new SecurityException("Signature Not Verified!");
}





[Serializable]
internal class LicenseTerms
{
	/// <summary>
	/// start date of the license agreement.
	/// </summary>
	public DateTime StartDate { get; set; }
	
	/// <summary>
	/// registered user name for the license agreement.
	/// </summary>
	public String UserName { get; set; }
	
	/// <summary>
	/// the assembly name of the product that is licensed.
	/// </summary>
	public String ProductName { get; set; }
	
	/// <summary>
	/// the last date on which the software can be used on this license.
	/// </summary>
	public DateTime EndDate { get; set; }
	
	/// <summary>
	/// returns the license terms as an obscure (not human readable) string.
	/// </summary>
	/// <returns></returns>
	public String GetLicenseString()
	{
		using (MemoryStream ms = new MemoryStream())
		{
			// create a binary formatter:
			BinaryFormatter bnfmt = new BinaryFormatter();
			
			// serialize the data to the memory-steam;
			bnfmt.Serialize(ms, this);
			
			// return a base64 string representation of the binary data:
			return Convert.ToBase64String(ms.GetBuffer());
			
		}
	}
	
	/// <summary>
	/// returns a binary representation of the license terms.
	/// </summary>
	/// <returns></returns>
	public byte[] GetLicenseData()
	{
		using (MemoryStream ms = new MemoryStream())
		{
			// create a binary formatter:
			BinaryFormatter bnfmt = new BinaryFormatter();
			
			// serialize the data to the memory-steam;
			bnfmt.Serialize(ms, this);
			
			// return a base64 string representation of the binary data:
			return ms.GetBuffer();
			
		}
	}
	
	/// <summary>
	/// create a new license-terms object from a string-representation of the binary
	/// serialization of the licence-terms.
	/// </summary>
	/// <param name="licenseTerms"></param>
	/// <returns></returns>
	internal static LicenseTerms FromString(String licenseTerms)
	{
		
		using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(licenseTerms)))
		{
			// create a binary formatter:
			BinaryFormatter bnfmt = new BinaryFormatter();
			
			// serialize the data to the memory-steam;
			object value = bnfmt.Deserialize(ms);
			
			if (value is LicenseTerms)
				return (LicenseTerms)value;
			else
				throw new ApplicationException("Invalid Type!");
			
		}
	}
	
}
*/



