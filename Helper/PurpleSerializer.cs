using System;
using System.IO;
using System.Text; 
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using _JSON = Newtonsoft.Json.JsonConvert;

public class PurpleSerializer
{
	// Formating
	private static bool useJSONMessage = true;

	// convert an object into a string
	public static string ObjectToStringConverter(object message)
	{
		return ObjectToStringConverter(message, false);
	}
	
	public static string ObjectToStringConverter(object message, bool forceXML)
	{
		string return_message = null;
		if(useJSONMessage && !forceXML)
		{
			try{
				return_message = _JSON.SerializeObject(message);
			} catch(Exception e){
				Debug.LogWarning("Can not convert object to JSON: " + e.ToString());
				Debug.Log("Set message encoding standard to XML");
				useJSONMessage = false;
			}
		}
		
		if (String.IsNullOrEmpty (return_message))
		{
			return_message = serialize_object_XML(message);
		}
		return return_message;
	}
	
	public static T StringToObjectConverter <T> (string message)
	{
		try{
			return (T)_JSON.DeserializeObject<T>(message);
		} catch(Exception e){
			Debug.LogWarning("Can not convert message using JSON: " + e.ToString());
			try{
				return (T)deserialize_object_XML<T>(message);
			} catch(Exception ex){
				Debug.LogWarning("Can not convert message using XML: " + ex.ToString());
				throw new PurpleException ("Can not convert string to the predefined object!");
			}
		}
	}


	// PRIVATE /////////////////////////

	private static string serialize_object_XML (object pObject) 
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

	private static T deserialize_object_XML <T> (string pXmlizedString) 
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
