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
	ScoreComponents scoreComponents = new ScoreComponents();

	private void Start()
	{
		scripter = GameObject.Find("Scripter");
		scripter.GetComponent<TextAnimController>().PlayStartAnim();
		GameObject.Find("AudioManager").GetComponent<AudioController>().PlayStartSound();
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
		scoreComponents.won = true;
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

		fillScoreComponents();
		SceneManager.LoadScene("ScoreScene", LoadSceneMode.Single);
	}

	private void fillScoreComponents()
	{
		float maxTime = GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().GetMaxTime();
		float userTime = GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().GetUserTime();

		int perfectMoves = GameObject.Find("Scripter").GetComponent<LevelLoader>().GetNonVirusCards();
		int userMoves = GameObject.Find("Moves").GetComponent<MovesBehaviour>().GetScore();

		scoreComponents.maxTime = maxTime;
		scoreComponents.timeTaken = userTime;

		scoreComponents.movePerfect = perfectMoves;
		scoreComponents.moves = userMoves;

		GameObject.Find("Communicator").GetComponent<Communicator>().SetScoreComponent(scoreComponents);
	}
}
