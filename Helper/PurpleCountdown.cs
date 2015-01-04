using UnityEngine;
using System.Collections;

// TODO: create "instance" if invoke and countdown so we can destinguish between them

public class PurpleCountdown : MonoBehaviour
{
	private static float t_time;

	private static float t_trigger;

	private static PurpleCountdown instance;

	public delegate void PurpleCountdownEvent(); // countdown event
	public static event PurpleCountdownEvent CountdownRunEvent;
	public static event PurpleCountdownEvent CountdownDoneEvent;
	public static event PurpleCountdownEvent TriggerRepeatEvent;
	public static event PurpleCountdownEvent TriggerEvent;


	// SINGLETON /////////////////////////
	private static PurpleCountdown Instance
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

	public static float CountdownTimeLeft()
	{
		return Instance.count_down_time_left ();
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
		InvokeRepeating("invoke_trigger_repeating", offset, repeat);
	}

	private void invoke_trigger()
	{
		instance.trigger_purple_event (TriggerEvent);
	}

	private void invoke_trigger_repeating()
	{
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
		t_time = (float)seconds;
		StartCoroutine (countdown_trigger ());
	}

	private IEnumerator countdown_trigger()
	{
		while (t_time > 0)
		{
			yield return new WaitForSeconds(1);

			instance.trigger_purple_event (CountdownRunEvent);

			t_time -= 1;
		}
		yield return new WaitForSeconds(1);
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

	private float count_down_time_left()
	{
		return t_time;
	}

	// EVENT ////////////////////////////

	private void trigger_purple_event(PurpleCountdownEvent eve)
	{
		if(eve != null)
			eve();
	}
}
