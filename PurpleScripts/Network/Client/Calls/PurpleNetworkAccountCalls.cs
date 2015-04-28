using System;
using _PMBasic = Entities.PurpleMessages;
using _PMClient = Entities.PurpleMessages.User;

namespace PurpleNetwork.Client.Calls
{
	public class Account
	{
		public static void ValidateUsername(string username)
		{
			_PMBasic.Data basicData = new _PMBasic.Data ();
			basicData.data = username;
			PurpleNetwork.ToServer ("client_validate_username", basicData);
		}

		public static void RegisterAccount(string Username, string FirstName, string LastName, string Email,
			string Gender, string Language, string Country, DateTime Birthday)
		{
			_PMClient.CreateAccount accountData = new _PMClient.CreateAccount ();
			accountData.ClientBirthday = Birthday;
			accountData.ClientCountry = Country;
			accountData.ClientEmail = Email;
			accountData.ClientFirstName = FirstName;
			accountData.ClientGender = Gender;
			accountData.ClientLanguage = Language;
			accountData.ClientLastName = LastName;
			accountData.ClientName = Username;

			RegisterAccount (accountData);
		}

		public static void RegisterAccount(_PMClient.CreateAccount accountData)
		{
			PurpleNetwork.ToServer ("client_register", accountData);
		}

		public static void DisableAccount(string ClientName, string ClientPassword)
		{
			_PMClient.Authentication authObject = new _PMClient.Authentication ();
			authObject.ClientName = ClientName;
			authObject.ClientPassword = ClientPassword;

			DisableAccount (authObject);
		}

		public static void DisableAccount(_PMClient.Authentication authObject)
		{
			PurpleNetwork.ToServer ("client_disable", authObject);
		}
	}
}

