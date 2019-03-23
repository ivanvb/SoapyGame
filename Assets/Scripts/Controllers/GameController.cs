using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	/**
	 * Controls the game state and communicate through an event
	 * when it changes.
	 */

	private bool game_state = false;

	public delegate void GameStateChanged(bool game_state);
	public event GameStateChanged OnGameStateChange;

	GameObject scripter;

	private void Start()
	{
		scripter = GameObject.Find("Scripter");
		scripter.GetComponent<TextAnimController>().PlayStartAnim();
		StartCoroutine(GameStartDelay());
		scripter.GetComponent<CardMatchController>().onVictory += onVictory;
		GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().timeIsOver += onTimeOver;
	}


	public void SetGamePlayable(bool game_state)
	{
		this.game_state = game_state;
		OnGameStateChange(game_state);
	}

	public bool IsGamePlayable()
	{
		return game_state;
	}

	private IEnumerator GameStartDelay()
	{
		yield return new WaitForSeconds(3.2f);
		game_state = true;
		OnGameStateChange(game_state);
	}

	private void onVictory()
	{
		SetGamePlayable(false);
		scripter.GetComponent<TextAnimController>().PlayVictoryAnim();
		StartCoroutine(ScoreScreen());
	}

	private void onTimeOver()
	{
		SetGamePlayable(false);
		scripter.GetComponent<TextAnimController>().PlayTimeOvertAnim();
		StartCoroutine(ScoreScreen());
	}

	private IEnumerator ScoreScreen()
	{
		yield return new WaitForSeconds(3.2f);

		SceneManager.LoadScene("ScoreScene", LoadSceneMode.Single);
	}
}
