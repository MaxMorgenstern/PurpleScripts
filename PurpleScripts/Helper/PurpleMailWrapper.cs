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
			foreach (string filePath in files) 
			{
				string culture = recipient.language_code + "-"+recipient.country_code.ToUpper();
				if(filePath.Contains("/"+culture+"/") || filePath.Contains("\\"+culture+"\\"))
				{
					body = File.ReadAllText(filePath);
					break;
				}
				
				// TODO: fallback to en-EN - config
				if(filePath.Contains("/en/") || filePath.Contains("\\en\\"))
				{
					bodyFallback = File.ReadAllText(filePath);
				}
			}
			
			if(string.IsNullOrEmpty(body))
				body = bodyFallback;

			if(!string.IsNullOrEmpty(body))
			{
				// TODO: fallback title - config
				Regex titleRegex = new Regex(@"\{(Title:)(.*)\}");
				string title = titleRegex.Match(body).Groups[2].ToString();
				if(string.IsNullOrEmpty(title))
					title = "Default Title";

				Regex bodyRegex = new Regex(@"\{(Title:).*\}");
				body = bodyRegex.Replace(body, string.Empty);

				PurpleMail.ResetDictionary();
				PurpleMail.AddDictionaryEntry("USERNAME",recipient.username);
				PurpleMail.AddDictionaryEntry("FIRST_NAME",recipient.first_name);
				PurpleMail.AddDictionaryEntry("LAST_NAME",recipient.last_name);
				PurpleMail.AddDictionaryEntry("EMAIL",recipient.email);

				// TODO - gender
				// recipient.gender
				
				PurpleMail.Send (recipient.email, title, body);

				PurpleMail.ResetDictionary();
			}
		}
	}
}
