using System.Collections;
using System.IO;
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
					continue;
				}
				
				// TODO: fallback to en-EN - config
				if(filePath.Contains("/en-EN/") || filePath.Contains("\\en-EN\\"))
				{
					bodyFallback = File.ReadAllText(filePath);
				}
			}

			if(!string.IsNullOrEmpty(body))
			{
				PurpleMail.Send (recipient.email, "Title", body);
			}
		}
	}
}
