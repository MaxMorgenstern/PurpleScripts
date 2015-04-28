using System;
using _PMGamemaster = Entities.PurpleMessages.Gamemaster;

namespace PurpleNetwork.Client.Calls
{
	public class GameMaster
	{
		public static void AddWarning(string Username, string Password, string Token, 
			string WarnedUser, int WarningLevel, string Comment)
		{
			_PMGamemaster.Warning gmWarning = new _PMGamemaster.Warning ();

			gmWarning.gmUsername = Username;
			gmWarning.gmPassword = Password;
			gmWarning.gmToken = Token;

			gmWarning.warningComment = Comment;
			gmWarning.warningLevel = WarningLevel;
			gmWarning.warningUser = WarnedUser;

			AddWarning(gmWarning);
		}

		public static void AddWarning(_PMGamemaster.Warning GMWarning)
		{
			PurpleNetwork.ToServer ("gamemaster_add_warning", GMWarning);
		}

	}
}
