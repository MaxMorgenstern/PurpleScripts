using System;
using NUnit.Framework;
using PurpleConfig;
using UnityEngine;

namespace PurpleTests
{
	[TestFixture]
	[Category("Purple Example Tests")]
	public class BasicTest
	{
		[Test]
		[Category("TestCategory")]
		public void TestCase ()
		{
			Assert.AreEqual (20, 20);
		}
	}

	[TestFixture]
	[Category("Purple Config Tests")]
	public class PurpleConfigTest
	{
		[Test]
		public void StringTest ()
		{
			bool b = PurpleConfig.ItemIds.getConfigBoolean ("DoesNotExist");
			Assert.IsFalse ( b );
		}
	}
}