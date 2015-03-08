using UnityEngine;
using PurpleDatabase;
using PurpleDatabase.Extension;
using System.Data;
using NUnit.Framework;

namespace PurpleDatabaseWrapperExtension
{

	[TestFixture]
	[Category("SELECT Tests")]
	public class SELECT_Test
	{
		[Test]
		[Category("SELECT Test")]
		public void Select_01 ()
		{
			string expected = "SELECT `test` FROM `one`;";
			string generated = SQLGenerator.New().Select (select: "test", from: "one");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test").From("one");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_02 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` LIMIT 5 OFFSET 10;";
			string generated = SQLGenerator.New().Select (select: "test, test2", from: "one", limit:5, offset:10 );
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test, test2", "one").Limit (5, 10);
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_03 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` ORDER BY `test` ASC;";
			string generated = SQLGenerator.New().Select (select: "test, test2", from: "one", sorting:"test ASC" );
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test, test2", "one").OrderBy ("test", "ASC");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test, test2").From("one").OrderBy ("test ASC");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_04 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'variable';";
			string generated = SQLGenerator.New().Select ("test, test2").From("one").Where("test='variable'");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test, test2").From("one").Where("test=variable");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_05 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` = 'Test';";
			string generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2").Select ("test3");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test' AND `test2` = 'Test12';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2").Select ("test3").Where ("test2='Test12'");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_05_NoEscape ()
		{
			SQLGenerator.DisableEscape ();
			string expected = "SELECT test FROM one WHERE test = 'Test';";
			string generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'");
			Assert.AreEqual (expected, generated);

			expected = "SELECT test, test2 FROM one WHERE test = 'Test';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2");
			Assert.AreEqual (expected, generated);

			expected = "SELECT test, test2, test3 FROM one WHERE test = 'Test';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2").Select ("test3");
			Assert.AreEqual (expected, generated);

			expected = "SELECT test, test2, test3 FROM one WHERE test = 'Test' AND test2 = 'Test12';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2").Select ("test3").Where ("test2='Test12'");
			Assert.AreEqual (expected, generated);
			SQLGenerator.EnableEscape ();
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_05_NoStringEscape ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` = 'Test';";
			string generated = SQLGenerator.New().Select ("test", "one", "test = Test");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.New().Select ("test", "one", "test = 'Test'").Select ("test2");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test' AND `test2` = 'Test12';";
			generated = SQLGenerator.New().Select ("test", "one", "test = Test").Select ("test2").Select ("test3").Where ("test2=Test12");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_06 ()
		{
			string expected = "SELECT * FROM `one`;";
			string generated = SQLGenerator.New().Select (from: "one");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("*").From("one");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_09 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `numbervalue` = 10 AND `test` = 'test';";
			string generated = SQLGenerator.New().Select ("test", "one").Select ("test2").Where ("numbervalue = 10").Where ("test = 'test'");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test", "one").Select ("test2").Where ("numbervalue = 10").Where ("AND test = 'test'");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_10 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `numbervalue` = 10 OR `test` = 'test';";
			string generated = SQLGenerator.New().Select ("test", "one").Select ("test2").Where ("numbervalue = 10").Where ("OR test = 'test'");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test", "one").Select ("test2").Where ("numbervalue = 10").Where ("test = 'test'", "OR");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test").Select ("test2").From ("one").Where ("numbervalue = 10").Where ("test = 'test'", "OR");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_11 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` LIKE '%est%';";
			string generated = SQLGenerator.New().Select ("test", "one").Where ("test LIKE '%est%'");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test", "one").Like ("test", "%est%");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test").From ("one").Like ("test", "'%est%'");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_12 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `numbervalue` = 10 OR `test` LIKE '%est%';";
			string generated = SQLGenerator.New().Select ("test", "one").Where ("numbervalue = 10").Where ("test LIKE '%est%'", "OR");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Select ("test", "one").Where ("numbervalue = 10").Like ("test", "%est%", "OR");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_13 ()
		{
			string expected = "SELECT `test` FROM `one` LIMIT 1;";
			string generated = SQLGenerator.New().Select ("test", "one").Single ();
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_14 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` IN ('value1', 'value2', 'value3');";
			string generated = SQLGenerator.New().Select ("test, test2", "one");
			generated = SQLGenerator.In ("test", "value1, value2, value3");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_15 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` IN (SELECT `dummy` FROM `two` WHERE `dummy` = 'one');";

			string preGenerated = SQLGenerator.New().Select ("dummy", "two", "dummy=one");
			string generated = SQLGenerator.New().Select ("test, test2", "one").In ("test", preGenerated, false);
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
			string expected = "UPDATE `tabletwo` SET `one` = NULL WHERE `two` = 'Test';";
			string generated = SQLGenerator.New().Update ("one=null", "tabletwo",  "two = 'Test'");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Table ("tabletwo").Update ("one=null").Where ("two = 'Test'");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("UPDATE Test")]
		public void Update_2 ()
		{
			string expected = "UPDATE `tabletwo` SET `one` = NULL, `three` = 'dummy2' WHERE `two` = 'Test2';";
			string generated = SQLGenerator.New().Update ("one=null", "tabletwo",  "two = 'Test2'").Update ("three=dummy2");
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Table ("tabletwo").Update ("one=null").Update ("three=dummy2").Where ("two = 'Test2'");
			Assert.AreEqual (expected, generated);

		}
	}


	[TestFixture]
	[Category("INSERT Tests")]
	public class INSERT_Test
	{
		[Test]
		[Category("INSERT Test")]
		public void Insert_3 ()
		{
			string expected = "INSERT INTO `tablename` (`col1`, `col2`) VALUES ('A', 'B'), ('C', 'D'), ('E', 'F'), ('G', 'H'), ('I', 'J');";
			string generated = SQLGenerator.New().Insert ("tablename", "col1, col2").Values ("A, B").Values (new string[] {"C, D", "E, F"});
			generated = SQLGenerator.Values (new string[] {"col1=G, col2=H", "col1=I, col2=J"});
			Assert.AreEqual (expected, generated);
		}
	}


	[TestFixture]
	[Category("DELETE Tests")]
	public class DELETE_Test
	{
		[Test]
		[Category("DELETE Test")]
		public void Delete_1 ()
		{
			string expected = "DELETE FROM `example`;";
			string generated = SQLGenerator.New().Delete("example");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("DELETE Test")]
		public void Delete_3 ()
		{
			string expected = "DELETE FROM `example` WHERE `field` = 'N' OR `field2` = 123;";
			string generated = SQLGenerator.Delete("example", "field=N").Where ("field2 = 123", "OR");
			Assert.AreEqual (expected, generated);
		}
		[Test]
		[Category("DELETE Test")]
		public void Delete_4 ()
		{
			string expected = "DELETE FROM `example` LIMIT 1;";
			string generated = SQLGenerator.Delete("example").Single();
			Assert.AreEqual (expected, generated);


			generated = SQLGenerator.New().Delete("example", limit:1);
			Assert.AreEqual (expected, generated);
		}
	}
}
