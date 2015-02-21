using UnityEngine;
using System.Collections;

// This has never been tested
// TODO: Test this

public class PurpleStorageTest : MonoBehaviour
{
	private bool doneTesting = false;
	private bool startTesting = false;

	private float timerStart;
	private float timerEnd;
	
	private string data = "12345678abcdefgh";
	private string b1024_tempData;
	private string kb512_tempData;
	private int count = 0;

	// private string keyPrefix = "key_";

	public void OnGUI() {
		if (!startTesting)
			PlayerPrefsTest();
	}

	public void PlayerPrefsTest()
	{
		if (b1024_tempData == null) 
		{
			b1024_tempData="";
			for(count = 0; count < 64; count++)
				b1024_tempData += data;

			kb512_tempData = "";
			for(count = 0; count < 1024; count++)
				kb512_tempData += b1024_tempData;
		}

		Debug.Log ("Start test...");

		if (!startTesting)
		{
			startTesting = true;
			timerStart = Time.time;
		}


		try {
			count = 0;
			while(!doneTesting)
			{
				PlayerPrefs.SetString("key"+count, kb512_tempData);
				count++;
				if(count > 64)
					doneTesting = true;
			}
		} catch (PlayerPrefsException err) {
			Debug.Log("Got: " + err);
			doneTesting = true;
		}
		PlayerPrefs.Save ();

		timerEnd = Time.time;

		Debug.Log ("Finished Test...");
		Debug.Log (timerEnd-timerStart);
		Debug.Log ("Count: " + (count-1));
		Debug.Log ((kb512_tempData.Length * (count-1)/1024/1024) + "MB stored");
	}
}

