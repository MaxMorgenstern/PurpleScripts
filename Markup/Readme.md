This folder contains two stored procedures:
	purple_mysql_to_csharp_model.txt
	mysql_to_csharp_model.txt


"mysql_to_csharp_model.txt" is the procedure found at 
http://www.code4copy.com/post/generate-c-sharp-model-class-mysql-table


"purple_mysql_to_csharp_model.txt" is a modified version for this application.

After installing, you can call the procedure like:
	CALL `PurpleDatabase`.`GenPurpleCSharpModel`('account');


the result will be similar to this:

public class PurpleAccount
{
		public int id { get; set; }
		public string GUID { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string email { get; set; }
		public DateTime email_verification { get; set; }
		public DateTime account_created { get; set; }
		public DateTime timestamp { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string gender { get; set; }
		public DateTime birthday { get; set; }
		public string country_code { get; set; }
		public string language_code { get; set; }
		public DateTime last_seen { get; set; }
		public DateTime last_login { get; set; }
		public string last_login_ip { get; set; }
		public string account_type { get; set; }
		public string token { get; set; }
		public DateTime token_created { get; set; }
		public string comment { get; set; }
		public byte active { get; set; }
}


In order for the class to work propery you still have to modify some variables like GUID to something like:
		private Guid _GUID;
		public string GUID { 
			get
			{
				return _GUID.ToString();
			}

			set
			{
				_GUID = new Guid(value);
			}
		}


		
- Max