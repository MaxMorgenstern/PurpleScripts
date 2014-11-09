using UnityEngine;
using System.Collections;

// TODO: create "instance" if invoke and countdown so we can destinguish between them

public class PurpleCountdown : MonoBehaviour
{
	private static float t_countdown;
	private static float t_time;

	private static float t_trigger;
	private static float t_trigger_repeating;

	private static PurpleCountdown instance;

	public delegate void PurpleCountdownEvent(); // countdown event
	public static event PurpleCountdownEvent CountdownRunEvent;
	public static event PurpleCountdownEvent CountdownDoneEvent;
	public static event PurpleCountdownEvent TriggerRepeatEvent;
	public static event PurpleCountdownEvent TriggerEvent;


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

	public static void Trigger(float offset)
	{
		Instance.trigger (offset);
	}

	public static void Trigger(float offset, float repeat)
	{
		Instance.trigger (offset, repeat);
	}

	public static void CancelTrigger()
	{
		Instance.cancel_trigger ();
	}

	private static bool TestTrigger()
	{
		return Instance.test_trigger();
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

	private void trigger(float offset)
	{
		t_trigger = offset;
		Invoke("invoke_trigger", t_trigger);
	}

	private void trigger(float offset, float repeat)
	{
		t_trigger_repeating = offset;
		InvokeRepeating("invoke_trigger_repeating", offset, repeat);
	}

	private void invoke_trigger()
	{
		Debug.Log("Triggered after " + t_trigger + " seconds!");
		instance.trigger_purple_event (TriggerEvent);
	}

	private void invoke_trigger_repeating()
	{
		Debug.Log("Repeating... First time triggered after " + t_trigger_repeating + " seconds!");
		instance.trigger_purple_event (TriggerRepeatEvent);
	}

	private void cancel_trigger()
	{
		CancelInvoke();
	}

	private bool test_trigger()
	{
		return IsInvoking ("invoke_trigger");
	}


	// COUNTDOWN ////////////////////////////

	private void count_down(int seconds)
	{
		t_countdown = (float)seconds;
		t_time = (float)seconds;
		StartCoroutine (countdown_trigger ());
	}

	private IEnumerator countdown_trigger()
	{
		while (t_time > 0)
		{
			yield return new WaitForSeconds(1);

			Debug.Log ("Triggered after " + (t_countdown-t_time+1) + " seconds!");
			instance.trigger_purple_event (CountdownRunEvent);

			t_time -= 1;
		}

		Debug.Log ("Finished after " + (t_countdown-t_time).ToString() + " seconds!");
		instance.trigger_purple_event (CountdownDoneEvent);
	}

	private void cancel_count_down()
	{
		StopCoroutine ("countdown_trigger");
	}

	private void cancel_all_count_down()
	{
		StopAllCoroutines ();
	}


	// EVENT ////////////////////////////

	private void trigger_purple_event(PurpleCountdownEvent eve)
	{
		if(eve != null)
			eve();
	}
}
