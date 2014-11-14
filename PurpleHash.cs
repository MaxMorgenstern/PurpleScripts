using UnityEngine;
using System.Collections;

// TODO: class for hashing
// something like http://support.microsoft.com/kb/307020/de

// TODO: mov into helper

public class PurpleHash
{
	public static int CalculateHash (object hashObject)
	{
		return CalculateHash(PurpleSerializer.ObjectToStringConverter (hashObject));
	}
	
	public static int CalculateHash (string hashObject)
	{
		return hashObject.GetHashCode ();
	}
}

