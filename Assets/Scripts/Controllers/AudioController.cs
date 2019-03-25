using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour {

	/**
	 * Plays a sound depending on the events it receives.
	 */

	public AudioSource victorySound;
	public AudioSource timeOverSound;
	public AudioSource gameBackgroundMusic;
	public AudioSource victory;
	public AudioSource shuffleVirus;
	public AudioSource timerVirus;
	public AudioSource startAudio;

	GameObject scripter;

	// Use this for initialization
	void Start () {
		scripter = GameObject.Find("Scripter");
		scripter.GetComponent<CardMatchController>().onVictory += PlayVictorySound;

		GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().timeIsOver += PlayTimeOverSound;

		


		gameBackgroundMusic.Play();
	}
	

	void PlayVictorySound()
	{
		StartCoroutine(PlaySoundWithDelay(0.9f, victorySound));
	}

	void PlayTimeOverSound()
	{
		StartCoroutine(PlaySoundWithDelay(0.2f, timeOverSound));
	}

	/* Plays a sound with a delay, uses a 'coroutine' to achieve this. */
	IEnumerator PlaySoundWithDelay(float delay, AudioSource audio)
	{
		yield return new WaitForSeconds(delay);

		audio.Play();
	}

	public void PlayDebug()
	{
		victory.Play();
	}

	public void PlayDebug2()
	{
		victorySound.Play();
	}


	public void PlayShuffleVirus()
	{
		shuffleVirus.Play();
	}

	public void PlayTimerVirus()
	{
		timerVirus.Play();
	}

	public void PlayStartSound()
	{
		StartCoroutine(PlaySoundWithDelay(2.5f, startAudio));
	}

}
