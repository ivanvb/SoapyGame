
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The purpose of this class is to monitor
 * when two cards are matched and do the corresponding
 * actions.
 */

public class CardMatchController : MonoBehaviour {

	GameObject cards;
	List<Transform> childCardsTransforms;
	GameController gameController;
	public delegate void OnVictory();
	public event OnVictory onVictory;

	public delegate void OnMatch(string match_tag);
	public event OnMatch onMatch;

	public delegate void OnVirusFlipped(string virus_tag);
	public event OnVirusFlipped onVirusFlipped;

	public delegate void OnCardDeleted(GameObject card);
	public event OnCardDeleted onCardDeleted;

	private void Start()
	{
		LoadCardsList();
		gameController = GameObject.Find("Scripter").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update () {

		CheckForMatchedCards();
	}

	/* Inserts all card Transform component inside a List.
	 * 
	 * Note: The Transform component is being used instead of the GameObject
	 *		 because it allows to be iterated over, therefore, we can access
	 *		 child components from it.
	 */
	private void LoadCardsList()
	{
		childCardsTransforms = new List<Transform>();
		cards = GameObject.Find("cards");
		
		Transform cardsTransform = cards.GetComponent<Transform>();
		if (cards != null)
		{
			foreach (Transform t in cardsTransform)
			{
				childCardsTransforms.Add(t);
			}
		}
	}

	/* Checks if the two front facing cards have the same tag. If so
	 * calls the corresponding method to manage the event. */
	private void CheckForMatchedCards()
	{
		GameObject card1 = null; GameObject card2 = null; Transform virusCard = null;

		foreach (Transform cardTransform in childCardsTransforms)
		{
			GameObject currentCard = cardTransform.gameObject;
			if (currentCard.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_FRONT_FACING_IDLE)
			{
				if(currentCard.tag == "virus1" || currentCard.tag == "virus2" || currentCard.tag == "virus3")
				{
					if(onVirusFlipped != null && gameController.IsGamePlayable())
					{
						virusCard = cardTransform;
					}
				}else if (card1 == null )
				{
					card1 = currentCard;
				}
				else if (card2 == null)
				{
					card2 = currentCard;
				}
			}
		}

		if(virusCard != null)
		{
			GameObject.Find("Scripter").GetComponent<GameController>().SetGamePlayable(false);
			childCardsTransforms.Remove(virusCard);
			virusCard.gameObject.GetComponent<CardBehaviour>().DeleteCard();

			virusCard.gameObject.GetComponent<CardBehaviour>().SetDestinationPosition(new Vector3(0, -1.5f));
			StartCoroutine(incrementSize(virusCard.gameObject));
			
			onCardDeleted(virusCard.gameObject);
			virusCard = null;
		}

		if (card1 != null && card2 != null && card1.tag == card2.tag && gameController.IsGamePlayable())
		{
			DeleteMatchedCards(card1, card2);
		}
		else if (card1 != null && card2 != null && gameController.IsGamePlayable())
		{
			card1.GetComponent<CardBehaviour>().FlipCardWithDelay(PhysicsConstants.CARD_DELAY_FLIP);
			card2.GetComponent<CardBehaviour>().FlipCardWithDelay(PhysicsConstants.CARD_DELAY_FLIP);
		}
	}

	/* Removes two matches cards from the game  and from the Transform's list.*/
	private void DeleteMatchedCards(GameObject card1, GameObject card2)
	{
		childCardsTransforms.Remove(card1.transform);
		childCardsTransforms.Remove(card2.transform);

		onCardDeleted(card1);
		onCardDeleted(card2);
		card1.GetComponent<CardBehaviour>().DeleteCard();
		card2.GetComponent<CardBehaviour>().DeleteCard();

		onMatch(card1.tag);

		if(onVictory != null  && CheckForVictory())
		{
			onVictory();
		}
	}

	public List<GameObject> GetCardsList()
	{
		return CastCardTransformToGameObject();
	}

	private List<GameObject> CastCardTransformToGameObject()
	{
		List<GameObject> childCardsGameObjects = new List<GameObject>();
		foreach(Transform t in childCardsTransforms)
		{
			childCardsGameObjects.Add(t.gameObject);
		}

		return childCardsGameObjects;
	}

	IEnumerator incrementSize(GameObject card)
	{
		foreach(Transform child in card.transform)
		{
			if(!child.name.Contains("back"))
			{
				child.GetComponent<SpriteRenderer>().sortingOrder = 10000;
			}
		}

		while(card.transform.position.x != 0 && card.transform.position.y != -1.5f)
		{
			yield return null;
		}
		while(card.transform.localScale.x < 6.8f && card.transform.localScale.y < 6.8f)
		{
			card.transform.localScale += new Vector3(0.3f, 0.3f);
			yield return null;
		}

		if(card.tag == "virus1")
		{
			GameObject.Find("AudioManager").GetComponent<AudioController>().PlayShuffleVirus();
		}
		else if(card.tag == "virus2")
		{
			GameObject.Find("AudioManager").GetComponent<AudioController>().PlayTimerVirus();
		}
		yield return new WaitForSeconds(25f * Time.deltaTime);

		card.GetComponent<CardBehaviour>().SetDestinationPosition(new Vector3(0, 8));

		while(card.transform.position.y != 8)
		{
			yield return null;
		}

		card.GetComponent<CardBehaviour>().SetDestinationPosition(new Vector3(10, 10));

		onVirusFlipped(card.tag);
	}

	private bool CheckForVictory()
	{
		bool won = true;
		foreach(Transform transform in childCardsTransforms)
		{
			if(!transform.tag.Contains("virus"))
			{
				won = false;
			}
		}

		if (childCardsTransforms.Count == 0) won = true;

		return won;
	}

}
