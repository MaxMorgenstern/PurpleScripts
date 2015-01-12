using System.Collections;
using System.Net.Mail;
using System.Net;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class PurpleMail : MonoBehaviour
{
	private static PurpleMail instance;

	private static string senderAddress;
	private static string senderDisplayName;
	private static MailAddress senderMail;

	private static string mailHost;
	private static string mailUser;
	private static string mailPassword;

	//private static int mailPort;
	//private static bool mailSSL;


	// START UP /////////////////////////
	protected PurpleMail ()
	{
		try {
			senderAddress = PurpleConfig.Mail.Sender.Address;
			senderDisplayName = PurpleConfig.Mail.Sender.Name;
			senderMail = new MailAddress(senderAddress, senderDisplayName);

			mailHost = PurpleConfig.Mail.Server.Host;
			mailUser = PurpleConfig.Mail.Server.User;
			mailPassword = PurpleConfig.Mail.Server.Password;

			// TODO
			//mailPort = PurpleConfig.Mail.Server.Port;
			//mailSSL = PurpleConfig.Mail.Server.SSL;

		} catch(Exception e){
			Debug.LogError("Can not read Purple Config! " + e.ToString());
		}
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
		string title = "Hallo, Welt!";
		string body = "This email is just a test...<br /><b>this is bold text!</b>";

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
			
			mail.Subject = title;
			mail.Body = body;

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

	// TODO ...
	private string replace_placeholder(string source)
	{

		string ret = source;

		// TODO... replace

		return ret;
	}

}

