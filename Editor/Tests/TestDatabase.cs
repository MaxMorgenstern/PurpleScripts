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
		public void Select_01 ()
		{
			string expected = "SELECT `test` FROM `one`;";
			string generated = SQLGenerator.Select (select: "test", from: "one");
			Assert.AreEqual (expected, generated);

			SQLGenerator.Reset ();

			generated = SQLGenerator.Select ("test");
			generated = SQLGenerator.From ("one");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_02 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` LIMIT 5 OFFSET 10;";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", limit:5, offset:10 );
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test, test2", "one");
			generated = SQLGenerator.Limit (5, 10);
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_03 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` ORDER BY `test` ASC;";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", sorting:"test ASC" );
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test, test2", "one");
			generated = SQLGenerator.OrderBy ("test", "ASC");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test, test2", "one");
			generated = SQLGenerator.OrderBy ("test ASC");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_04 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'variable';";
			string generated = SQLGenerator.Select (select: "test, test2", from: "one", where:"test='variable'" );
			Assert.AreEqual (expected, generated);

			SQLGenerator.Reset ();

			generated = SQLGenerator.Select (select: "test, test2", from: "one", where:"test=variable" );
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_05 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` = 'Test';";
			string generated = SQLGenerator.Select ("test", "one", "test = 'Test'");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.Select ("test2");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.Select ("test3");
			Assert.AreEqual (expected, generated);

			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test' AND `test2` = 'Test12';";
			generated = SQLGenerator.Where ("test2='Test12'");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_05_NoEscape ()
		{
			SQLGenerator.DisableEscape ();
			string expected = "SELECT test FROM one WHERE test = 'Test';";
			string generated = SQLGenerator.Select ("test", "one", "test = 'Test'");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT test, test2 FROM one WHERE test = 'Test';";
			generated = SQLGenerator.Select ("test2");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT test, test2, test3 FROM one WHERE test = 'Test';";
			generated = SQLGenerator.Select ("test3");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT test, test2, test3 FROM one WHERE test = 'Test' AND test2 = 'Test12';";
			generated = SQLGenerator.Where ("test2='Test12'");
			Assert.AreEqual (expected, generated);
			SQLGenerator.EnableEscape ();
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_05_NoStringEscape ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` = 'Test';";
			string generated = SQLGenerator.Select ("test", "one", "test = Test");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT `test`, `test2` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.Select ("test2");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test';";
			generated = SQLGenerator.Select ("test3");
			Assert.AreEqual (expected, generated);
			
			expected = "SELECT `test`, `test2`, `test3` FROM `one` WHERE `test` = 'Test' AND `test2` = 'Test12';";
			generated = SQLGenerator.Where ("test2=Test12");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_06 ()
		{
			string expected = "SELECT * FROM `one`;";
			string generated = SQLGenerator.Select (from: "one");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();

			generated = SQLGenerator.Select (select: "*", from: "one");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_07 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one`;";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_08 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `numbervalue` >= 10;";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue >= 10");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test");
			generated = SQLGenerator.From ("one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue >= 10");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_09 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `numbervalue` = 10 AND `test` = 'test';";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("test = 'test'");
			Assert.AreEqual (expected, generated);

			SQLGenerator.Reset ();

			generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("AND test = 'test'");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_10 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `numbervalue` = 10 OR `test` = 'test';";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("OR test = 'test'");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("test = 'test'", "OR");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test");
			generated = SQLGenerator.Select ("test2");
			generated = SQLGenerator.From ("one");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("test = 'test'", "OR");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_11 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `test` LIKE '%est%';";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Where ("test LIKE '%est%'");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Like ("test", "%est%");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test");
			generated = SQLGenerator.From ("one");
			generated = SQLGenerator.Like ("test", "'%est%'");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_12 ()
		{
			string expected = "SELECT `test` FROM `one` WHERE `numbervalue` = 10 OR `test` LIKE '%est%';";
			string generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Where ("test LIKE '%est%'", "OR");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();

			generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Where ("numbervalue = 10");
			generated = SQLGenerator.Like ("test", "%est%", "OR");
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_13 ()
		{
			string expected = "SELECT `test` FROM `one` LIMIT 1;";
			string generated = SQLGenerator.Select (select: "test", from: "one", limit:1 );
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Select ("test", "one");
			generated = SQLGenerator.Single ();
			Assert.AreEqual (expected, generated);
		}

		[Test]
		[Category("SELECT Test")]
		public void Select_14 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` IN ('value1', 'value2', 'value3');";
			string generated = SQLGenerator.Select ("test, test2", "one");
			generated = SQLGenerator.In ("test", "value1, value2, value3");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("SELECT Test")]
		public void Select_15 ()
		{
			string expected = "SELECT `test`, `test2` FROM `one` WHERE `test` IN (SELECT `dummy` FROM `two` WHERE `dummy` = 'one');";

			string preGenerated = SQLGenerator.Select ("dummy", "two", "dummy=one");
			string generated = SQLGenerator.Select ("test, test2", "one");
			generated = SQLGenerator.In ("test", preGenerated, false);
			Assert.AreEqual (expected, generated);
		}

		/*
		TODO:
		[Test]
		[Category("SELECT Test")]
		public void Select_16 ()
		{
			string expected = "SELECT `one`.`test`, `two`.`dummy` FROM `one`, `two` WHERE `one`.`test2` = `two`.`dummy2`;";
			string generated = "";
			Assert.AreEqual (expected, generated);
		}
		*/
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
			string generated = SQLGenerator.Update ("one=null", "tabletwo",  "two = 'Test'");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();

			generated = SQLGenerator.Table ("tabletwo");
			generated = SQLGenerator.Update ("one=null");
			generated = SQLGenerator.Where ("two = 'Test'");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("UPDATE Test")]
		public void Update_2 ()
		{
			string expected = "UPDATE `tabletwo` SET `one` = NULL, `three` = 'dummy2' WHERE `two` = 'Test2';";
			string generated = SQLGenerator.Update ("one=null", "tabletwo",  "two = 'Test2'");
			generated = SQLGenerator.Update ("three=dummy2");
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.Table ("tabletwo");
			generated = SQLGenerator.Update ("one=null");
			generated = SQLGenerator.Update ("three=dummy2");
			generated = SQLGenerator.Where ("two = 'Test2'");
			Assert.AreEqual (expected, generated);

		}
	}


	[TestFixture]
	[Category("INSERT Tests")]
	public class INSERT_Test
	{	
		[Test]
		[Category("INSERT Test")]
		public void Insert_1 ()
		{
			string expected = "INSERT INTO `tablename` (`field1`, `field2`, `field3`) VALUES ('value', 'OK', NULL);";
			string generated = SQLGenerator.Insert ("tablename", new string[]{ "field1=value, field2=OK, field3=null" });
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("INSERT Test")]
		public void Insert_1b ()
		{
			string expected = "INSERT INTO `tablename` (`field1`, `field2`, `field3`) VALUES ('value', 'OK', NULL), ('test', 42, 'test2');";
			string generated = SQLGenerator.Insert ("tablename", new string[]{ "field1=value, field2=OK, field3=null", "field1=test, field2=42, field3='test2'" });
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("INSERT Test")]
		public void Insert_2 ()
		{
			string expected = "INSERT INTO `tablename` VALUES ('value b', 'NOK', 900);";
			string generated = SQLGenerator.Insert ("tablename", new string[]{ "value b, NOK, 900" });
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("INSERT Test")]
		public void Insert_2b ()
		{
			string expected = "INSERT INTO `tablename` VALUES ('value b', 'NOK', 900), ('number 6', 'OKAY', NULL);";
			string generated = SQLGenerator.Insert ("tablename", new string[]{ "value b, NOK, 900", "number 6, OKAY, null" });
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("INSERT Test")]
		public void Insert_3 ()
		{
			string expected = "INSERT INTO `tablename` (`col1`, `col2`) VALUES ('A', 'B'), ('C', 'D'), ('E', 'F'), ('G', 'H'), ('I', 'J');";
			string generated = SQLGenerator.Insert ("tablename", "col1, col2");
			generated = SQLGenerator.Values ("A, B");
			generated = SQLGenerator.Values (new string[] {"C, D", "E, F"});
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
			string generated = SQLGenerator.Delete("example");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("DELETE Test")]
		public void Delete_2 ()
		{
			string expected = "DELETE FROM `example` WHERE `field` = 'N';";
			string generated = SQLGenerator.Delete("example", "field=N");
			Assert.AreEqual (expected, generated);
		}
		
		[Test]
		[Category("DELETE Test")]
		public void Delete_3 ()
		{
			string expected = "DELETE FROM `example` WHERE `field` = 'N' OR `field2` = 123;";
			string generated = SQLGenerator.Delete("example", "field=N");
			generated = SQLGenerator.Where ("field2 = 123", "OR");
			Assert.AreEqual (expected, generated);
		}
		[Test]
		[Category("DELETE Test")]
		public void Delete_4 ()
		{
			string expected = "DELETE FROM `example` LIMIT 1;";
			string generated = SQLGenerator.Delete("example");
			generated = SQLGenerator.Single();
			Assert.AreEqual (expected, generated);
			
			SQLGenerator.Reset ();

			generated = SQLGenerator.Delete("example", limit:1);
			Assert.AreEqual (expected, generated);
		}
	}
}
