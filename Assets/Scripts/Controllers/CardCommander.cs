using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardCommander : MonoBehaviour {

	public List<List<GameObject>> cards;
	// Use this for initialization
	void Start () {
		GameObject.Find("Scripter").GetComponent<CardMatchController>().onCardDeleted += RemoveFromList;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetAllCardsDown()
	{
		StartCoroutine(FlipCardsDown());
	}

	IEnumerator FlipCardsDown()
	{

		while(!CheckIfDone())
		{
			yield return null;
		}

		foreach(List<GameObject> cardsrow in cards)
		{
			foreach(GameObject card in cardsrow)
			{
				if(card.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_FRONT_FACING_IDLE)
				{
					card.GetComponent<CardBehaviour>().FlipCard();
				}
			}
		}

		yield return new WaitForSeconds(25f * Time.deltaTime);
		MoveAllCardsToPos();

		yield return new WaitForSeconds(23f * Time.deltaTime);

		cards = cards.OrderBy(a => Guid.NewGuid()).ToList();
		List<List<GameObject>> shuffledCards = new List<List<GameObject>>();

		foreach(List<GameObject> cardsrow in cards)
		{
			List<GameObject> newCardsRow = cardsrow.OrderBy(a => Guid.NewGuid()).ToList();
			shuffledCards.Add(newCardsRow);
		}
		
		GameObject.Find("Scripter").GetComponent<LevelLoader>().MoveCards(shuffledCards);
		GameObject.Find("Scripter").GetComponent<GameController>().SetGamePlayable(true);
	}

	bool CheckIfDone()
	{
		bool done = true;
		foreach(List<GameObject> cardsrow in cards)
		{
			foreach(GameObject card in cardsrow)
			{
				if (card.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_BACK_FLIPPING ||
					card.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_FRONT_FLIPPING)
				{
					Debug.Log("im here");
					done = false;
					break;
				}
			}
		}

		return done;
	}

	public void SetCards(List<List<GameObject>> cards)
	{
		this.cards = cards;
	}

	private void RemoveFromList(GameObject card)
	{
		foreach(List<GameObject> cardrow in cards)
		{
			if(cardrow.Contains(card))
			{
				cardrow.Remove(card);
			}
		}
	}

	private void MoveAllCardsToPos()
	{
		foreach(List<GameObject> cardrow in cards)
		{
			foreach(GameObject card in cardrow)
			{
				card.GetComponent<CardBehaviour>().SetDestinationPosition(new Vector3(0, -1));
			}
		}
	}

	public bool CheckIfCanFlip()
	{
		int counter = 0;
		bool canFlip = true;
		foreach(List<GameObject> cardrow in cards)
		{
			foreach(GameObject card in cardrow)
			{
				if(card.GetComponent<CardBehaviour>().GetCardState() != StateConstants.STATE_BACK_FACING_IDLE)
				{
					counter++;
					if(counter == 2)
					{
						canFlip = false;
						break;
					}
				}
			}
		}

		return canFlip;
	}
}
