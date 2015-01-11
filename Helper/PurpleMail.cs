using System.Collections;
using System.Net.Mail;
using System.Net;
using UnityEngine;
using System;

public class PurpleMail
{
	// START UP /////////////////////////
	protected PurpleMail ()
	{

	}

	public static void Send()
	{
		string bodyText = "This email is just a test...<br /><b>this is bold text!</b>";
		try
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("maximilian@porzelt.net", "Maximilian Porzelt");
			mail.To.Add("1@porzelt.net");
			
			mail.Subject = "this is a test email.";
			mail.Body = bodyText;
			
			mail.IsBodyHtml = true;
			
			SmtpClient client = new SmtpClient(PurpleConfig.Mail.Host);
			client.Credentials = new NetworkCredential(PurpleConfig.Mail.User, PurpleConfig.Mail.Password);
			
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
			
			
			// TODO: comment in
			//client.Send(mail);
			
			Debug.Log ("Mail sent...!?");
		}
		catch (SmtpException exc)
		{
			Debug.Log("Fehler: "+ exc.Message);
		}
		catch (Exception exce)
		{
			Debug.Log(exce);
		}
	}
}

