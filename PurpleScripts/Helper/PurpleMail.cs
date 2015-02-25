using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UnityEngine;

public class PurpleMail : MonoBehaviour
{
	private static PurpleMail 	instance;

	private static string 		senderAddress;
	private static string 		senderDisplayName;
	private static MailAddress 	senderMail;

	private static string 		mailHost;
	private static string 		mailUser;
	private static string 		mailPassword;

	private static int 			mailPort;
	private static bool 		mailUseSSL;

	private static string		storedTitle;
	private static string		storedBody;
	
	private static Dictionary<string,string> placeholderDictionary;
	

	// START UP /////////////////////////
	protected PurpleMail ()
	{
		try {
			senderAddress = PurpleConfig.Mail.Sender.Address;
			senderDisplayName = PurpleConfig.Mail.Sender.Name;

			mailHost = PurpleConfig.Mail.Server.Host;
			mailUser = PurpleConfig.Mail.Server.User;
			mailPassword = PurpleConfig.Mail.Server.Password;

			mailPort = PurpleConfig.Mail.Server.Port;
			mailUseSSL = PurpleConfig.Mail.Server.SSL;

		} catch(Exception e){
			senderAddress = "no-reply@example.com";
			senderDisplayName = "No Reply";

			mailHost = "localhost";
			mailUser = String.Empty;
			mailPassword = String.Empty;

			mailPort = 25;
			mailUseSSL = false;

			Debug.LogError("Can not read Purple Config! " + e.ToString());
		}

		senderMail = new MailAddress(senderAddress, senderDisplayName);
		placeholderDictionary = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
		
		storedTitle = String.Empty;
		storedBody = String.Empty;
	}

	// SINGLETON /////////////////////////
	private static PurpleMail Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject 	= new GameObject ("PurpleMailManager");
				instance     			= gameObject.AddComponent<PurpleMail> ();
			}
			return instance;
		}
	}

	// PUBLIC FUNCTIONS /////////////////////////
	public static bool Send(string recipient)
	{
		return Instance.send_mail (recipient);
	}

	public static bool Send(string recipient, string title, string body)
	{
		return Instance.send_mail (recipient, title, body);
	}

	public static bool Send(string recipient, string title, string body, MailAddress sender)
	{
		return Instance.send_mail (recipient, title, body, sender);
	}

	public static bool Send(string recipient, string title, string body, string senderAddress, string senderName)
	{
		return Instance.send_mail (recipient, title, body, senderAddress, senderName);
	}

	public static void SetMail(string title, string body)
	{
		Instance.set_mail (title, body);
	}

	public static void ResetDictionary()
	{
		Instance.reset_dictionary ();
	}
	
	public static void AddDictionaryEntry(string key, string value)
	{
		Instance.add_dictionary_entry (key, value);
	}


	// TODO ...
	/*
	public static void EnableSigning()
	{
		Instance.enable_smime ();
	}
	
	public static void DisableSigning()
	{
		Instance.disable_smime ();
	}
	
	private void enable_smime()
	{
	}

	private void disable_smime()
	{
	}
	*/

	
	// PRIVATE FUNCTIONS /////////////////////////

	private bool send_mail(string recipient, string title, string body, string sender_address, string sender_name)
	{
		return send_mail(recipient, title, body, new MailAddress(sender_address, sender_name));
	}
	
	private bool send_mail(string recipient, string title, string body, MailAddress sender)
	{
		senderMail = sender;
		bool result = send_mail(recipient, title, body);
		senderMail = new MailAddress(senderAddress, senderDisplayName);
		return result;
	}

	private bool send_mail(string recipient)
	{
		if(!String.IsNullOrEmpty(storedTitle) && !String.IsNullOrEmpty(storedBody))
		{
			return send_mail(recipient, storedTitle, storedBody);
		}
		return false;
	}

	private bool send_mail(string recipient, string title, string body)
	{
		storedTitle = title;
		storedBody = body;

		try 
		{
			MailMessage mail = new MailMessage();
			mail.From = senderMail;
			mail.To.Add(recipient);
			
			mail.Subject = replace_placeholder(title);
			mail.Body = replace_placeholder(body);

			mail.IsBodyHtml = is_html(body);
			
			SmtpClient client = new SmtpClient(mailHost);
			client.Credentials = new NetworkCredential(mailUser, mailPassword);

			client.Port = mailPort;
			client.EnableSsl = mailUseSSL;

			client.Send(mail);
			return true;
		}
		catch (SmtpException exc)
		{
			Debug.Log("Fehler: "+ exc.Message);
		}
		catch (Exception exce)
		{
			Debug.Log(exce);
		}
		return false;
	}

	private void set_mail(string title, string body)
	{
		storedTitle = title;
		storedBody = body;
	}


	// PRIVATE HELPER /////////////////////////
	
	private bool is_html(string probe)
	{
		Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>|<br *\/>");
		return tagRegex.IsMatch(probe);
	}
	
	private string replace_placeholder(string source)
	{
		Regex re = new Regex(@"\{(\w+)\}", RegexOptions.Compiled);
		if(placeholderDictionary.Count > 0)
		{
			return re.Replace(source, match => placeholderDictionary[match.Groups[1].Value]);
		}
		return source;
	}


	// PRIVATE DICTIONARY HELPER /////////////////////////

	private void reset_dictionary()
	{
		placeholderDictionary = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
	}

	private void add_dictionary_entry(string key, string value)
	{
		placeholderDictionary.Remove (key);
		placeholderDictionary.Add (key, value);
	}

	private string get_placeholder_entry(string entry)
	{
		string returnData = String.Empty;
		try
		{
			placeholderDictionary.TryGetValue(entry,out returnData);
		}
		catch (Exception e)
		{
			Debug.LogWarning(e.ToString());
		}
		return (String.IsNullOrEmpty(returnData)) ? entry : returnData;
	}

}
