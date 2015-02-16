using UnityEngine;
using PurpleDatabase;
using System.Data;


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
		#if UNITY_5_0
		q = PurpleDatabase.SQLGenerator.Select (from: "one");
		Debug.Log (q);

		dt = PurpleDatabase.SQLGenerator.Select (from: "one").Fetch ();
		#else
		PurpleDatabase.SQLGenerator.Reset();
		PurpleDatabase.SQLGenerator.Select("*");
		PurpleDatabase.SQLGenerator.From("one");
		q = PurpleDatabase.SQLGenerator.Get();
		Debug.Log(q);

		dt = PurpleDatabase.SQLGenerator.Get().Fetch();
		#endif
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

