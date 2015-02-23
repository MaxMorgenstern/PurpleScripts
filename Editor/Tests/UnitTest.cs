using System;
using NUnit.Framework;
using UnityEngine;
using System.Threading;

namespace PurpleTests
{
	/*
	 * Countdown
	 * 
	 * I18n
	 * Serializer
	 * Storage
	 * 
	 * Network
	 * Server
	 * Client
	 * Messages
	 * Queue
	 * SpamPrevention
	 */

	[TestFixture]
	[Category("PurpleConfig Tests")]
	public class PurpleConfigTests
	{
		[Test]
		[Category("Test not existing variables")]
		public void GetBool_Fail ()
		{
			Assert.IsFalse(PurpleConfig.ItemIds.getConfigBoolean ("DoesNotExist"));
		}

		[Test]
		[Category("Test not existing variables")]
		public void GetDouble_Fail ()
		{
			Assert.AreEqual(0, PurpleConfig.ItemIds.getConfigDouble ("DoesNotExist"));
		}

		[Test]
		[Category("Test not existing variables")]
		public void GetFloat_Fail ()
		{
			Assert.AreEqual(0, PurpleConfig.ItemIds.getConfigFloat ("DoesNotExist"));
		}
		
		[Test]
		[Category("Test not existing variables")]
		public void GetInt_Fail ()
		{
			Assert.AreEqual(0, PurpleConfig.ItemIds.getConfigInt ("DoesNotExist"));
		}
		
		[Test]
		[Category("Test not existing variables")]
		public void GetString_Fail ()
		{
			Assert.IsNullOrEmpty(PurpleConfig.ItemIds.getConfigString ("DoesNotExist"));
		}

	
		[Test]
		[Category("Test existing variables")]
		public void GetBool ()
		{
			Assert.IsTrue(PurpleConfig.ItemIds.getConfigBoolean ("PurpleTest.Bool"));
		}
		
		[Test]
		[Category("Test existing variables")]
		public void GetDouble ()
		{
			Assert.AreEqual(42.5, PurpleConfig.ItemIds.getConfigDouble ("PurpleTest.Double"));
		}
		
		[Test]
		[Category("Test existing variables")]
		public void GetFloat ()
		{
			Assert.AreEqual(42.5, PurpleConfig.ItemIds.getConfigFloat ("PurpleTest.Float"));
		}
		
		[Test]
		[Category("Test existing variables")]
		public void GetInt ()
		{
			Assert.AreEqual(42, PurpleConfig.ItemIds.getConfigInt ("PurpleTest.Int"));
		}
		
		[Test]
		[Category("Test existing variables")]
		public void GetString ()
		{
			Assert.AreEqual("Test", PurpleConfig.ItemIds.getConfigString ("PurpleTest.String"));
		}
	}

	/*
	[TestFixture]
	[Category("PurpleCountdown Tests")]
	public class PurpleCountdownTests
	{
		private bool statsUpdated = false;
		private ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

		[Test]
		[Category("Test Countdown")]
		public void Countdown ()
		{
			statsUpdated = false;
			statsUpdatedEvent.Reset();

			PurpleCountdown countDownTrigger = PurpleCountdown.NewInstance ();
			countDownTrigger.TriggerEvent += delegate {
				statsUpdated = true;
				statsUpdatedEvent.Set();
			};
			countDownTrigger.Trigger (1, 1, 1);

			statsUpdatedEvent.WaitOne (4500, false);
			Assert.IsTrue(statsUpdated);
		}
	}
	*/
	[TestFixture]
	[Category("PurpleHash Tests")]
	public class PurpleHashTests
	{
		[Test]
		[Category("Test MD5")]
		public void MD5 ()
		{
			string MD5Reference = "0cbc6611f5540bd0809a388dc95a615b";
			Assert.AreEqual(MD5Reference, PurpleHash.CalculateMD5 ("Test"));
		}
		
		[Test]
		[Category("Test SHA")]
		public void SHA ()
		{
			string SHAReference = "640ab2bae07bedc4c163f679a746f7ab7fb5d1fa";
			Assert.AreEqual(SHAReference, PurpleHash.CalculateSHA ("Test"));
		}
	}


	[TestFixture]
	[Category("PurplePassword Tests")]
	public class PurplePasswordTests
	{
		[Test]
		[Category("Test Password")]
		public void Password ()
		{
			string password = "TestPassword123!";
			PurplePassword PurplePasswordInstance = new PurplePassword ();
			string passwordHash = PurplePasswordInstance.CreateHash (password);
			
			Assert.IsTrue(PurplePasswordInstance.ValidatePassword (password, passwordHash));
		}

		[Test]
		[Category("Test Password")]
		public void Password_Fail ()
		{
			string password = "TestPassword123!";
			PurplePassword PurplePasswordInstance = new PurplePassword ();
			string passwordHash = PurplePasswordInstance.CreateHash (password);
			
			Assert.IsFalse(PurplePasswordInstance.ValidatePassword ("WrongPassword987?", passwordHash));
		}
	}
}
