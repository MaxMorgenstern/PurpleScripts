using UnityEngine;
using PurpleDatabase;
using System.Data;

namespace Testing
{
	public class TestDatabase : MonoBehaviour
	{
		char[] trimChars = { ' ', '-'}; 

		void Start ()
		{
			PurpleI18n.Setup ("de-DE");
			PurpleDatabase.PurpleDatabase.SwitchDatabase ("test");

			// SELECT * FROM <table>
			SimpleSelect ();



		}


		void SimpleSelect()
		{
			Debug.LogWarning("SimpleSelect()");


			// SELECT * FROM <table>
			string q = "SELECT * FROM ONE";
			Debug.Log (q);

			DataTable dt = PurpleDatabase.PurpleDatabase.SelectQuery (q);
			foreach(DataRow dr in dt.Rows)
			{
				string rowOutput = string.Empty;
				foreach(DataColumn dc in dt.Columns)
				{
					rowOutput += dc+":"+dr[dc] + " - ";
				}
				Debug.Log(rowOutput.Trim(trimChars));
			}
			
			Debug.LogWarning ("------------");
			
			// SELECT * FROM <table>
			q = PurpleDatabase.SQLGenerator.Select (from: "one");
			Debug.Log (q);

			dt = PurpleDatabase.SQLGenerator.Select (from: "one").Fetch ();
			foreach(DataRow dr in dt.Rows)
			{
				string rowOutput = string.Empty;
				foreach(DataColumn dc in dt.Columns)
				{
					rowOutput += dc+":"+dr[dc] + " - ";
				}
				Debug.Log(rowOutput.Trim(trimChars));
			}
		}
	}
}

