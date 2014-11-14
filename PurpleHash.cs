using UnityEngine;
using System.Collections;

// TODO: class for hashing
// something like http://support.microsoft.com/kb/307020/de

// TODO: mov into helper
using System.Security.Cryptography;
using System.Text;

public class PurpleHash
{
	public static string CalculateHash (object hashObject)
	{
		return CalculateHash(PurpleSerializer.ObjectToStringConverter (hashObject), hashObject.ToString());
	}
	
	public static string CalculateHash (string hashObject, string hashObjectType)
	{
		return convert_string_to_MD5(hashObject.ToString() + hashObjectType.ToString());
	}

	public static string CalculateHash (string hashObject)
	{
		return convert_string_to_MD5(hashObject.ToString());
	}



	private const string _myGUID = "{C05ACA39-C810-4DD1-B138-41603713DD8A}";
	private static string convert_string_to_MD5(string strword)
	{
		MD5 md5 = MD5.Create();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(_myGUID + strword);
		byte[] hash = md5.ComputeHash(inputBytes);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("x2"));
		}
		return sb.ToString();
	}
}
