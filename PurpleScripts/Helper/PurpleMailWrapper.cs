using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Entities.Database;

public class PurpleMailGenerator
{
	public static void SendMail(string template, PurpleAccount recipient)
	{
		SendMail (template, recipient, string.Empty);
	}

	public static void SendMail(string template, PurpleAccount recipient, string password)
	{
		string culture = "#";
		if(!string.IsNullOrEmpty(recipient.language_code) && !string.IsNullOrEmpty(recipient.country_code))
			culture = recipient.language_code + "-"+recipient.country_code.ToUpper();
		string body = get_mail_template (template, culture);

		if(!string.IsNullOrEmpty(body))
		{
			string title = extract_title(body);
			body = clean_body(body);

			PurpleMail.ResetDictionary();
			PurpleMail.AddDictionaryEntry("USERNAME",recipient.username);
			PurpleMail.AddDictionaryEntry("FIRST_NAME",recipient.first_name);
			PurpleMail.AddDictionaryEntry("LAST_NAME",recipient.last_name);
			PurpleMail.AddDictionaryEntry("EMAIL",recipient.email);

			string salt = PurpleHash.Token ().Substring(0,5);
			string token = PurpleHash.CalculateSHA(salt + recipient.guid) + ":" +
							PurpleHash.CalculateSHA(salt + recipient.email) + ":" + salt;
			PurpleMail.AddDictionaryEntry("TOKEN",token);

			if(!string.IsNullOrEmpty(password))
				PurpleMail.AddDictionaryEntry("PASSWORD",password);

			PurpleMail.Send (recipient.email, title, body);

			PurpleMail.ResetDictionary();
		}
	}

	private static string get_mail_template(string template, string culture)
	{
		string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),
		                                    (template+".email"), SearchOption.AllDirectories);
		if(files.Length > 0)
		{
			string body = string.Empty;
			string bodyFallback = string.Empty;
			string languageFallback = PurpleConfig.Mail.Content.Fallback.Language;

			foreach (string filePath in files)
			{
				if(filePath.Contains("/"+culture+"/") || filePath.Contains("\\"+culture+"\\"))
				{
					body = File.ReadAllText(filePath);
					break;
				}

				if(filePath.Contains("/"+languageFallback+"/") || filePath.Contains("\\"+languageFallback+"\\"))
				{
					bodyFallback = File.ReadAllText(filePath);
				}
			}

			if(string.IsNullOrEmpty(body))
				body = bodyFallback;

			return body;
		}
		return string.Empty;
	}

	private static string extract_title(string body)
	{
		Regex titleRegex = new Regex(@"\{(Title:)(.*)\}");
		string title = titleRegex.Match(body).Groups[2].ToString();
		if(string.IsNullOrEmpty(title))
			title = PurpleConfig.Mail.Content.Fallback.Title;
		return title;
	}

	private static string clean_body(string body)
	{
		Regex bodyRegex = new Regex(@"\{(Title:).*\}");
		return bodyRegex.Replace(body, string.Empty);
	}
}
