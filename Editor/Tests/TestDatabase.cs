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
		

		q = PurpleDatabase.SQLGenerator.Select (select: "test", from: "one");
		Debug.Log (q);
		
		q = PurpleDatabase.SQLGenerator.Select (select: "test, field2", from: "one", limit:5, offset:10 );
		Debug.Log (q);
		
		q = PurpleDatabase.SQLGenerator.Select (select: "test, field2", from: "one", sorting:"test ASC" );
		Debug.Log (q);
		
		q = PurpleDatabase.SQLGenerator.Select (select: "test, field2", from: "one", where:"test='variable'" );
		Debug.Log (q);


		PurpleDatabase.SQLGenerator.Reset();
		PurpleDatabase.SQLGenerator.Select("test");
		PurpleDatabase.SQLGenerator.Select("dummy");
		PurpleDatabase.SQLGenerator.From("one");
		q = PurpleDatabase.SQLGenerator.Get();
		Debug.Log(q);
		

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
