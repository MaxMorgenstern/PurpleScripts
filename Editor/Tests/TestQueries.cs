#if UnityTestToolsIsPresent
using UnityEngine;
using PurpleDatabase;
using System.Data;
using NUnit.Framework;

namespace PurpleDatabaseQuery
{
	[TestFixture]
	[Category("Valid Query Tests")]
	public class Valid_Query_Test
	{
		[Test]
		[Category("Valid Test")]
		public void Valid_Tests ()
		{
			string query = string.Empty;

			query = "DELETE FROM `example`;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT `test`, `test2` FROM `one` LIMIT 5 OFFSET 10;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "INSERT INTO `tablename` (`col1`, `col2`) VALUES ('A', 'B'), ('C', 'D'), ('E', 'F'), ('G', 'H'), ('I', 'J');";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "DELETE FROM `example` WHERE `field` = 'N' OR `field2` = 123;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "UPDATE `tabletwo` SET `one` = NULL, `three` = 'dummy2' WHERE `two` = 'Test2';";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));
		}
	}

	[TestFixture]
	[Category("Critical Query Tests")]
	public class Critical_Query_Test
	{
		[Test]
		[Category("CRITICAL Test")]
		public void Critical_Tests ()
		{
			string query = string.Empty;

			query = "SELECT * FROM account WHERE created = '2015';";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsTrue (PurpleDatabase.PurpleDatabase.IsSQLValid (query, false));

			query = "SELECT * FROM account WHERE deleted = 1;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsTrue (PurpleDatabase.PurpleDatabase.IsSQLValid (query, false));

			query = "SELECT * FROM account WHERE `delete` = 1;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse (PurpleDatabase.PurpleDatabase.IsSQLValid (query, false));
		}
	}


	[TestFixture]
	[Category("Invalid Query Tests")]
	public class Invalid_Query_Test
	{
		[Test]
		[Category("INVALID Test")]
		public void Invalid_Tests ()
		{
			string query = string.Empty;

			query = "ALTER TABLE Persons ADD DateOfBirth date";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "CREATE DATABASE dbname;";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM customers WHERE username = '' OR 1''";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT fieldlist FROM customers WHERE name = '\''; DROP TABLE users; --';";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM users WHERE name = '' OR '1'='1' -- ';";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM users WHERE name = 'a';DROP TABLE users; SELECT * FROM userinfo WHERE 't' = 't';";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM Users WHERE Name ='' or ''='' AND Pass ='' or ''=''";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));
		}

		[Test]
		[Category("INVALID Test 2")]
		public void Invalid_Tests_2 ()
		{
			string query = string.Empty;

			query = "SELECT id, name, message FROM messages WHERE id = 1; DELETE FROM users;";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "select id, name, message FROM messages WHERE id = 1; delete FROM users;";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT id, name, message FROM messages WHERE id = 1; DELETE FROM users; --";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "select title, text from news where id=10 or 1=1";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM users WHERE name = 'test' OR '1'='1';";
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsFalse(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));

			query = "SELECT * FROM users WHERE name ='name' OR '1'=1;";
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, true));
			Assert.IsTrue(PurpleDatabase.PurpleDatabase.IsSQLValid(query, false));
		}
	}
}
#endif
