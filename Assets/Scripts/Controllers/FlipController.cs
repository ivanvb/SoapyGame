using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour {

	List<GameObject> cards;

	private void Start()
	{
		GameObject.Find("Scripter").GetComponent<CardMatchController>().onVictory += FlipAllCards;
		GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().timeIsOver += FlipAllCards;
	}
	// Update is called once per frame
	void Update () {
		cards = GameObject.Find("Scripter").GetComponent<CardMatchController>().GetCardsList();
	}

	public void FlipAllCards()
	{
		foreach(GameObject card in cards)
		{
			card.GetComponent<CardBehaviour>().FlipCard();
		}
	}
}
