using System;
using System.IO;
using System.Text; 
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class PurpleSerializer
{
	public static string SerializeObjectXML (object pObject) 
	{ 
		string XmlizedString = null; 
		Type pObjectType = pObject.GetType();

		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer (pObjectType); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter (memoryStream, Encoding.UTF8); 
		
		xs.Serialize (xmlTextWriter, pObject); 
		memoryStream = (MemoryStream) xmlTextWriter.BaseStream; 
		XmlizedString = UTF8_byte_array_to_string (memoryStream.ToArray());
		
		return XmlizedString;
	}

	public static T DeserializeObjectXML <T> (string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer (typeof (T)); 
		MemoryStream memoryStream = new MemoryStream (string_to_UTF8_byte_array (pXmlizedString)); 
		return (T)xs.Deserialize(memoryStream); 
	}

	private static string UTF8_byte_array_to_string (byte[] characters) 
	{		
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString (characters, 0, characters.Length);
		return (constructedString); 
	}

	private static byte[] string_to_UTF8_byte_array (string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes (pXmlString); 
		return byteArray; 
	}
}
