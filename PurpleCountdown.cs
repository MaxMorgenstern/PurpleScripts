using UnityEngine;
using System.Collections;

//TODO: a lot!
// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html

public class PurpleCountdown : MonoBehaviour
{
	private static float countdown;
	private static float time;
	
	private static PurpleCountdown instance;

	//public delegate void PurpleCountdownEvent(object passed_object); // countdown event
	//public static event PurpleCountdownEvent TriggerEvent;

	
	// SINGLETON /////////////////////////
	public static PurpleCountdown Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject 	= new GameObject ("PurpleCountdown");
				instance     			= gameObject.AddComponent<PurpleCountdown> ();
			}
			return instance;
		}
	}


	// PUBLIC ////////////////////////////

	public static void Trigger(int seconds)
	{
		Instance.trigger (seconds);
	}

	public static void CancelTrigger(int seconds)
	{
		Instance.cancel_trigger ();
	}
	
	public static void Countdown(int seconds)
	{
		Instance.count_down (seconds);
	}

	public static void CancelCountdown()
	{
		Instance.cancel_count_down ();
	}

	public static void CancelAllCountdown()
	{
		Instance.cancel_all_count_down ();
	}


	
	// PRIVATE ////////////////////////////

	// TRIGGER ////////////////////////////

	private void trigger(int seconds)
	{
		trigger (seconds, false);
	}

	private void trigger(int seconds, bool force)
	{
		if(!IsInvoking("invoke_trigger") || force)	// TODO: ??? Test if there is a active trigger... or force
		{
			countdown = (float)seconds;
			Invoke("invoke_trigger", countdown);
		}
	}

	private void invoke_trigger()
	{
		Debug.Log("Triggered after " + countdown + " seconds!");
		// TODO: Event
	}
	
	private void cancel_trigger()
	{
		CancelInvoke();
	}



	// COUNTDOWN ////////////////////////////

	private void count_down(int seconds)
	{
		countdown = (float)seconds;
		time = (float)seconds;
		StartCoroutine (countdown_trigger ());
	}

	private IEnumerator countdown_trigger()
	{
		while (time > 0)
		{
			yield return new WaitForSeconds(1);
			
			Debug.Log ("Triggered after " + (countdown-time+1) + " seconds!");
			// TODO: Event
			
			time -= 1;
		}

		Debug.Log ("Finished after " + (countdown-time).ToString() + " seconds!");
		// TODO: Event
	}
	
	private void cancel_count_down()
	{
		StopCoroutine (countdown_trigger ());
	}

	private void cancel_all_count_down()
	{
		StopAllCoroutines ();
	}






/*
	private void trigger_purple_event(PurpleCountdownEvent eve, object passed_object)
	{
		if(eve != null)
			eve(passed_object);
	}
*/
}

