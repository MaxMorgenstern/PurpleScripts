using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UnityEngine;

public class PurpleMail : MonoBehaviour
{
	private static PurpleMail instance;

	private static string senderAddress;
	private static string senderDisplayName;
	private static MailAddress senderMail;

	private static Dictionary<string,string> placeholderDictionary;

	private static string mailHost;
	private static string mailUser;
	private static string mailPassword;

	//private static int mailPort;
	//private static bool mailSSL;


	// START UP /////////////////////////
	protected PurpleMail ()
	{
		placeholderDictionary = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);

		// TODO
		//mailPort = PurpleConfig.Mail.Server.Port;
		//mailSSL = PurpleConfig.Mail.Server.SSL;

		try {
			senderAddress = PurpleConfig.Mail.Sender.Address;
			senderDisplayName = PurpleConfig.Mail.Sender.Name;

			mailHost = PurpleConfig.Mail.Server.Host;
			mailUser = PurpleConfig.Mail.Server.User;
			mailPassword = PurpleConfig.Mail.Server.Password;

		} catch(Exception e){
			senderAddress = "no-reply@example.com";
			senderDisplayName = "No Reply";

			mailHost = "localhost";
			mailUser = String.Empty;
			mailPassword = String.Empty;

			Debug.LogError("Can not read Purple Config! " + e.ToString());
		}

		senderMail = new MailAddress(senderAddress, senderDisplayName);
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

	public static bool Send()
	{
		// TODO: default values
		string recipient = "1@porzelt.net";
		string title = "Hallo, World!";
		string body = "This email is just a HTML test...<br /><b>This is a bold part!</b>";

		return Instance.send_mail(recipient, title, body);
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

	
	// TODO ...
	private void enable_smime()
	{
		/*
		X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
		store.Open(OpenFlags.ReadOnly);
		X509Certificate2Collection certs = store.Certificates;
		X509Certificate2 certificate = null;
		foreach (X509Certificate2 cert in certs)
		{
			Debug.Log(cert.Subject);
			if (cert.Subject.IndexOf("maximilian@porzelt.net") >= 0)
			{
				Debug.Log("found....");
				certificate = cert;
				break;
			}
		}
		*/
	
		/*
		try
		{
			// Find certificate by email adddress in My Personal Store.
			// Once the certificate is loaded to From, the email content
			// will be signed automatically                 
			mail.From.Certificate.FindSubject(mail.From.Address,
			                                   Certificate.CertificateStoreLocation.CERT_SYSTEM_STORE_CURRENT_USER,
			                                   "My");
		}
		catch (Exception exp)
		{
			Debug.Log("No sign certificate found for <" + 
			                  mail.From.Address + ">:" + exp.Message);
		}
		*/
	}

	
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

	private bool send_mail(string recipient, string title, string body)
	{
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
	// TODO: Public

	private void reset_dictionary()
	{
		placeholderDictionary = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
	}

	private void add_dictionary_entry(string key, string value)
	{
		placeholderDictionary.Add (key, value);
	}

	private void update_dictionary_entry(string key, string value)
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

