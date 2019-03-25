using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class that controls the behaviour of the Slider that represents the time.
 * This class has an event that notifies its subscribers when the time is over.
 */

public class TimerBehaviour : MonoBehaviour {

	float timer;
	Slider slider;
	GameObject scripter;
	public Image Fill;

	public delegate void TimeIsOver();
	public event TimeIsOver timeIsOver;
	Color currentColor;

	IEnumerator coroutine;

	private void Start()
	{
		coroutine = TimerCountdown();
		scripter = GameObject.Find("Scripter");
		scripter.GetComponent<GameController>().OnGameStateChange += StartTimer;
		scripter.GetComponent<LevelLoader>().onLevelLoaded += SetTimer;
		Fill.color = new Color(0.7764f, 0.8117f, 1);
		currentColor = Fill.color;
	}

	/* Method that is subscribed to the event 'onLevelLoaded', it receives the current level and sets the timer based
	 * on it. */
	public void SetTimer(Level level)
	{
		timer = level.timer;

		slider = GetComponent<Slider>();

		slider.maxValue = timer;
		slider.value = timer;
	}

	/* Starts the timer if it hasn't started yet */
	private void StartTimer(bool GameState)
	{
		if(GameState)
		{
			StartCoroutine(coroutine);
		}
		else
		{
			StopCoroutine(coroutine);
		}
		
	}

	/* Reduces the slider value until it reaches 0. As this is method is supposed to be called as a coroutine,
	 * it returns IEnumerator. */
	IEnumerator TimerCountdown()
	{
		while(slider.value > 0)
		{
			timer -= Time.deltaTime;
			slider.value = timer;
			yield return null;
		}

		timeIsOver();
	}

	public void TimerVirus(float timeToReduce)
	{
		StartCoroutine(ReduceTime(10f));
	}

	IEnumerator ReduceTime(float time)
	{
		float currentTime = slider.value;
		float goal = currentTime - time;
		yield return new WaitForSeconds(10f * Time.deltaTime);
		Fill.color = new Color(1,0,0,1);

		if (goal < 0) goal = 0;

		while(slider.value > goal)
		{
			slider.value -= 6f * Time.deltaTime;
			yield return null;
		}

		timer = slider.value;

		yield return new WaitForSeconds(10f * Time.deltaTime);
		Fill.color = currentColor;
		GameObject.Find("Scripter").GetComponent<GameController>().SetGamePlayable(true);
	}

	public float GetMaxTime()
	{
		return slider.maxValue;
	}

	public float GetUserTime()
	{
		return slider.value;
	}

}
