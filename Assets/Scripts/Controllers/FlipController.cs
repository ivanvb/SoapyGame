using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour {

	List<GameObject> cards;

	private void Start()
	{
		GameObject.Find("Scripter").GetComponent<CardMatchController>().onVictory += FlipAllCardToFront;
		GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().timeIsOver += FlipAllCardToFront;
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

	public void FlipAllCardToFront()
	{
		StartCoroutine(Wait());
		
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.5f);
		foreach (GameObject card in cards)
		{
			if (card.GetComponent<CardBehaviour>().GetCardState() != StateConstants.STATE_FRONT_FACING_IDLE)
			{
				card.GetComponent<CardBehaviour>().FlipCard();
			}
		}
	}
}
