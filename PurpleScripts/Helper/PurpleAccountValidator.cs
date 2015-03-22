using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;

public class PurpleAccountValidator
{
	public static bool ValidateUsername( string username )
	{
		// SETTINGS
		int MIN_LENGTH		=  PurpleConfig.Account.User.Name.MinLength;
		int MAX_LENGTH		=  PurpleConfig.Account.User.Name.MaxLength;
		
		// VALIDATION
		if ( string.IsNullOrEmpty (username) )
			return false;
		
		if ( username.Length >= MIN_LENGTH && username.Length <= MAX_LENGTH  )
		{
			Regex r = new Regex("^[a-zA-Z0-9-_]*$");
			if (r.IsMatch(username))
				return true;
		}
		return false;
	}
	
	public static bool ValidatePasswordStrength( string password )
	{
		// SETTINGS
		int MIN_LENGTH				= PurpleConfig.Account.User.Password.MinLength;
		int MAX_LENGTH        		= PurpleConfig.Account.User.Password.MaxLength;
		char[] SPECIAL_CHARACTERS   = PurpleConfig.Account.User.Password.Strength.AllowedSpecialChars.ToCharArray();
		
		bool hasUpperCaseLetter  	= !PurpleConfig.Account.User.Password.Strength.UpperCase;
		bool hasLowerCaseLetter     = !PurpleConfig.Account.User.Password.Strength.LowerCase;
		bool hasDecimalDigit        = !PurpleConfig.Account.User.Password.Strength.DecimalDigit;
		bool hasSpecialCharacter    = !PurpleConfig.Account.User.Password.Strength.SpecialCharacter;
		
		// VALIDATION
		if ( string.IsNullOrEmpty (password) )
			return false;
		
		if ( password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH  )
		{
			if(password.IndexOfAny(SPECIAL_CHARACTERS) != -1) hasSpecialCharacter = true;
			
			foreach (char c in password )
			{
				if      ( char.IsUpper(c) ) hasUpperCaseLetter = true;
				else if ( char.IsLower(c) ) hasLowerCaseLetter = true;
				else if ( char.IsDigit(c) ) hasDecimalDigit    = true;
			}
			
			bool isValid = hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit && hasSpecialCharacter;
			return isValid;
		}
		return false;
	}
}

