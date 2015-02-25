using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/**
 * This class only tests the content. 
 * Two objects with the same content have most likely the same hash values!
 * 
 * This way data that is transmitted via network can be compared!
 */

public class PurpleHash
{
	// MD5 /////////////////////////

	public static string CalculateMD5 (object hashObject)
	{
		return CalculateMD5(PurpleSerializer.ObjectToStringConverter (hashObject), hashObject.ToString());
	}
	
	public static string CalculateMD5 (string hashObject, string salt)
	{
		return convert_string_to_MD5(hashObject + salt);
	}
	
	public static string CalculateMD5 (string hashObject)
	{
		return convert_string_to_MD5(hashObject);
	}
	
	public static string CalculateMD5 (int hashObject)
	{
		return convert_string_to_MD5(hashObject.ToString());
	}
	
	public static string CalculateMD5 (float hashObject)
	{
		return convert_string_to_MD5(hashObject.ToString());
	}

	// SHA /////////////////////////

	public static string CalculateSHA (object hashObject)
	{
		return CalculateSHA(PurpleSerializer.ObjectToStringConverter (hashObject), hashObject.ToString());
	}
	
	public static string CalculateSHA (string hashObject, string salt)
	{
		return convert_string_to_SHA(hashObject + salt);
	}
	
	public static string CalculateSHA (string hashObject)
	{
		return convert_string_to_SHA(hashObject);
	}
	
	public static string CalculateSHA (int hashObject)
	{
		return convert_string_to_SHA(hashObject.ToString());
	}
	
	public static string CalculateSHA (float hashObject)
	{
		return convert_string_to_SHA(hashObject.ToString());
	}
	
	// HELPER /////////////////////////

	private static string convert_string_to_MD5(string strword)
	{
		MD5 md5 = MD5.Create();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strword);
		byte[] hash = md5.ComputeHash(inputBytes);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("x2"));
		}
		return sb.ToString();
	}

	private static string convert_string_to_SHA(string strword)
	{
		HashAlgorithm sha = new SHA1CryptoServiceProvider();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strword);
		byte[] hash = sha.ComputeHash(inputBytes);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("x2"));
		}
		return sb.ToString();
	}
}
