using UnityEngine;
using System.Collections;

public class PurpleCountdown : MonoBehaviour
{
	private int _ticks;
	private float _countdown;
	
	private static GameObject _gameObject;
	
	public delegate void PurpleCountdownEvent(); // countdown event
	
	public event PurpleCountdownEvent CountdownRunEvent;
	public event PurpleCountdownEvent CountdownDoneEvent;
	
	public event PurpleCountdownEvent TriggerEvent;
	
	// SINGLETON /////////////////////////
	private static PurpleCountdown Instance
	{
		get
		{
			_gameObject = new GameObject ("PurpleCountdown_"+System.Guid.NewGuid().ToString());
			return _gameObject.AddComponent<PurpleCountdown> ();
		}
	}
	
	public int CountDownLeft
	{
		get
		{
			return (int)Mathf.Floor(_countdown);
		}
	}
	
	
	// PUBLIC ////////////////////////////
	public static PurpleCountdown NewInstance()
	{
		return Instance;
	}
	
	public void DestroyInstance()
	{
		Destroy (_gameObject);
		Destroy(this);
	}
	
	
	// TRIGGER ////////////////////////////
	public void Trigger(float offset)
	{
		_ticks = -1;
		Invoke("invoke_trigger", offset);
	}
	
	public void Trigger(float offset, float repeatRate)
	{
		_ticks = -1;
		InvokeRepeating("invoke_trigger", offset, repeatRate);
	}
	
	public void Trigger(float offset, float repeatRate, int numberOfCalls)
	{
		_ticks = numberOfCalls;
		InvokeRepeating("invoke_trigger", offset, repeatRate);
	}
	
	public void CancelTrigger()
	{
		CancelInvoke();
	}
	
	public bool IsTriggerRunning()
	{
		return IsInvoking ("invoke_trigger");
	}
	
	// COUNTDOWN ////////////////////////////
	public void CountDown(int seconds)
	{
		_countdown = (float)seconds;
		StartCoroutine (countdown_trigger ());
	}
	
	
	
	// PRIVATE ////////////////////////////
	
	// TRIGGER ////////////////////////////
	private void invoke_trigger()
	{
		if(_ticks > 0)
		{
			if(_ticks <= 1)
				CancelInvoke();
			_ticks--;
		}
		trigger_purple_event (TriggerEvent);
	}
	
	
	// COUNTDOWN ////////////////////////////
	
	private IEnumerator countdown_trigger()
	{
		while (_countdown > 0)
		{
			yield return new WaitForSeconds(1);
			
			trigger_purple_event (CountdownRunEvent);
			
			_countdown -= 1;
		}
		yield return new WaitForSeconds(1);
		trigger_purple_event (CountdownDoneEvent);
	}
	
	public void CancelCountDown()
	{
		StopCoroutine ("countdown_trigger");
	}
	
	
	// EVENT ////////////////////////////
	private void trigger_purple_event(PurpleCountdownEvent eve)
	{
		if(eve != null)
			eve();
	}
}
