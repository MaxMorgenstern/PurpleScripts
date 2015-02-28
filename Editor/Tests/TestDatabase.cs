using UnityEngine;
using PurpleDatabase;
using System.Data;
using NUnit.Framework;

namespace PurpleDatabaseWrapper
{
	[TestFixture]
	[Category("SELECT Tests")]
	public class SELECT_Test
	{
		[Test]
		[Category("SELECT Test")]
		public void Select_1 ()
		{
			string expected = "SELECT `test` FROM `one`;";
			string generated = SQLGenerator.Select (select: "test", from: "one");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_2 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` LIMIT 5 OFFSET 10;";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", limit:5, offset:10 );
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_3 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` ORDER BY `test` ASC;";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", sorting:"test ASC" );
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_4 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'variable';";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", where:"test='variable'" );
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_5 ()
		{
			string expected = "SELECT `username` FROM `Account` WHERE `username` = 'Test';";
			string generated = SQLGenerator.Select ("username", "Account", "username = 'Test'");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `username`, `password` FROM `Account` WHERE `username` = 'Test';";
			generated = SQLGenerator.Select ("password");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `username`, `password`, `token` FROM `Account` WHERE `username` = 'Test';";
			generated = SQLGenerator.Select ("token");
			Assert.AreEqual (expected, generated);
		}


		/*
		SELECT * FROM Employees;
		SELECT FirstName FROM Employees;
		SELECT FirstName, LastName, City FROM Employees;
		SELECT FirstName, LastName, City FROM Employees WHERE City = 'London';
		SELECT FirstName, HireDate, City FROM Employees WHERE HireDate >= '1-july-1993';
		SELECT FirstName, LastName, City FROM Employees WHERE City = 'London' OR City = 'Seattle';
		SELECT FirstName, LastName, City FROM Employees WHERE City IN ('Seattle', 'Tacoma', 'Redmond');
		SELECT FirstName, LastName, City FROM Employees ORDER BY City;
		SELECT FirstName, LastName, Country, City FROM Employees ORDER BY Country ASC, City DESC;

		SELECT * FROM table_name WHERE column LIKE 'XXXX%'


		SELECT Vorlesung.VorlNr, Vorlesung.Titel, Professor.PersNr, Professor.Name
			FROM Professor, Vorlesung
			WHERE Professor.PersNr = Vorlesung.PersNr;

		INSERT INTO example (field1, field2, field3) VALUES ('test', 'N', NULL);
		INSERT INTO Store_Information VALUES ('Los Angeles', 10, 900, 'Jan-10-1999');
		
		UPDATE example SET field1 = 'updated value' WHERE field2 = 'N';
		UPDATE CUSTOMERS SET ADDRESS = 'Pune', SALARY = 1000.00;
		DELETE FROM example WHERE field2 = 'N';
		*/


		[Test]
		[Category("SELECT Test")]
		public void Select_6 ()
		{
			string expected = "";
			string generated = "";
			Assert.AreEqual (expected, generated);
		}
		[Test]
		[Category("SELECT Test")]
		public void Select_7 ()
		{
			string expected = "";
			string generated = "";
			Assert.AreEqual (expected, generated);
		}
	}


	[TestFixture]
	[Category("UPDATE Tests")]
	public class UPDATE_Test
	{
		
		[Test]
		[Category("UPDATE Test")]
		public void Update_1 ()
		{
			string expected = "UPDATE `Account` SET `token` = NULL WHERE `username` = 'Test';";
			
			string generated = SQLGenerator.Update ("token=null", "Account",  "username = 'Test'");
			
			Assert.AreEqual (expected, generated);
		}

	}
}
