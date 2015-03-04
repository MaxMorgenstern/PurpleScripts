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
			
			SQLGenerator.Reset ();
			
			generated = SQLGenerator.New().Select ("test").From("one");
			Assert.AreEqual (expected, generated);
		}
	}
}
