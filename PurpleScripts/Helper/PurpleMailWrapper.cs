using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Entities.Database;

public class PurpleMailGenerator
{
	public static void SendMail(string template, PurpleAccount recipient)
	{
		string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), 
		                                    (template+".email"), SearchOption.AllDirectories);
		if(files.Length > 0)
		{
			string body = string.Empty;
			string bodyFallback = string.Empty;
			string culture = recipient.language_code + "-"+recipient.country_code.ToUpper();
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

			if(!string.IsNullOrEmpty(body))
			{
				Regex titleRegex = new Regex(@"\{(Title:)(.*)\}");
				string title = titleRegex.Match(body).Groups[2].ToString();
				if(string.IsNullOrEmpty(title))
					title = PurpleConfig.Mail.Content.Fallback.Title;

				Regex bodyRegex = new Regex(@"\{(Title:).*\}");
				body = bodyRegex.Replace(body, string.Empty);

				PurpleMail.ResetDictionary();
				PurpleMail.AddDictionaryEntry("USERNAME",recipient.username);
				PurpleMail.AddDictionaryEntry("FIRST_NAME",recipient.first_name);
				PurpleMail.AddDictionaryEntry("LAST_NAME",recipient.last_name);
				PurpleMail.AddDictionaryEntry("EMAIL",recipient.email);

				string token = PurpleHash.CalculateSHA(recipient.guid) + ":" + PurpleHash.CalculateSHA(recipient.email);
				PurpleMail.AddDictionaryEntry("TOKEN",token);

				// TODO - gender
				// recipient.gender
				
				PurpleMail.Send (recipient.email, title, body);

				PurpleMail.ResetDictionary();
			}
		}
	}
}
