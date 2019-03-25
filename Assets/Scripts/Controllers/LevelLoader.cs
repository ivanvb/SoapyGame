using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

/**
 * Puts the cards in their corresponding position for the current level, also flips them for a brief 
 * amount of time so the user can see where each card is.
 */
public class LevelLoader : MonoBehaviour {
	
	Level currentLevel;
	string newpath = "Prefabs/Cards/";

	public delegate void OnLevelLoaded(Level level);
	public event OnLevelLoaded onLevelLoaded;

	bool levelSent = false;
	GameObject scripter;

	public int level;

	// Use this for initialization
	void Start () {

		GameObject communicator = GameObject.Find("Communicator");
		
		scripter = GameObject.Find("Scripter");
		currentLevel = communicator.GetComponent<Communicator>().GetLevel();


		LoadLevel();
		StartCoroutine(FlipCardsOnDelay(1.4f, 1f));
	
	}

	private void Update()
	{
		if (!levelSent && onLevelLoaded != null)
		{
			onLevelLoaded(currentLevel);
			levelSent = true;
		}
	}

	/* Instantiates all cards in the starting position */
	public void LoadLevel()
	{
		GameObject cards = new GameObject();
		cards.name = "cards";

		int n = currentLevel.tablero[0];
		int m = currentLevel.tablero[1];

		int x = 0;
		UpdatePath();
		List<List<GameObject>> cards_in_board = new List<List<GameObject>>();
		currentLevel.card_arr = currentLevel.card_arr.OrderBy(a => Guid.NewGuid()).ToList(); //shuffles cards

		float appearing_x = PhysicsConstants.CARD_APPEARING_X;
		float appearing_y = PhysicsConstants.CARD_APPEARING_Y;
		float appearing_z = PhysicsConstants.CARD_APPEARING_Z;

		for (int i = 0; i < n; i++)
		{
			List<GameObject> buffer = new List<GameObject>();
			for (int j = 0; j < m; j++)
			{
				string temporalPath = GetCurrentCardPath(currentLevel.card_arr[x]);
				GameObject newCard = Instantiate(Resources.Load(temporalPath) as GameObject, new Vector3(appearing_x, appearing_y, appearing_z), new Quaternion(0, 180, 0, 0));
				newCard.name = "card";
				newCard.transform.parent = cards.transform;

				buffer.Add(newCard);
				x++;
			}
			cards_in_board.Add(buffer);
		}

		GameObject.Find("Scripter").GetComponent<CardCommander>().SetCards(cards_in_board);
		MoveCards(cards_in_board);
	}

	/* Adds to the path the folder where the cards of the selected size are stored */

	private void UpdatePath()
	{
		if(currentLevel.card_size == "small")
		{
			newpath += "Small_cards/";
		}
		else if(currentLevel.card_size == "med")
		{
			newpath += "Medium_cards/";
		}
		else if (currentLevel.card_size == "big")
		{
			newpath += "Big_cards/";
		}
	}
	

	/* adds the name of the card prefab to the path */
	private string GetCurrentCardPath(string card_code)
	{
		string temporalPath = newpath;

		if(card_code == CardResourcesConstants.CARD_ALOE_VERA_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_ALOE_VERA_FILE;
		}
		else if(card_code ==  CardResourcesConstants.CARD_HONEY_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_HONEY_FILE;
		}
		else if(card_code == CardResourcesConstants.CARD_COCONUT_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_COCONUT_FILE;
		}
		else if(card_code == CardResourcesConstants.CARD_COTTON_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_COTTON_FILE;
		}
		else if (card_code == CardResourcesConstants.CARD_VIRUS1_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_VIRUS1_FILE;
		}
		else if (card_code == CardResourcesConstants.CARD_VIRUS2_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_VIRUS2_FILE;
		}
		else if (card_code == CardResourcesConstants.CARD_VIRUS3_CODE)
		{
			temporalPath += CardResourcesConstants.CARD_VIRUS3_FILE;
		}


		return temporalPath;
	}

	/* Moves each card to its corresponding position on the board */

	public void MoveCards(List<List<GameObject>> cards)
	{
		for(int i = 0; i <cards.Count; i++)
		{
			for(int j = 0; j < cards[i].Count; j++)
			{
				GameObject c_card = cards[i][j];
				if(c_card.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_BACK_FACING_IDLE ||
					c_card.GetComponent<CardBehaviour>().GetCardState() == StateConstants.STATE_FRONT_FACING_IDLE)
				{
					c_card.GetComponent<CardBehaviour>().SetDestinationPosition(calculateCardPosition(i+1, j+1));
				}
			}
		}
	}

	public Level GetCurrentLevel()
	{
		return currentLevel;
	}

	/*Flips cards so the user can have a look at each one for a few seconds */
	private IEnumerator FlipCardsOnDelay(float delay1, float delay2)
	{
		yield return new WaitForSeconds(delay1);
		scripter.GetComponent<FlipController>().FlipAllCards();
		yield return new WaitForSeconds(delay2);
		scripter.GetComponent<FlipController>().FlipAllCards();
	}

	/*Returns a vector3 with the position of the card */
	private Vector3 calculateCardPosition(int x, int y)
	{
		Vector3 appearingPosition = new Vector3();
		if(currentLevel.card_size == "small")
		{
			appearingPosition = new Vector3(
						(x * PhysicsConstants.SMALL_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS - PhysicsConstants.SMALL_CARD_X_POSITION_OFFSET), 
						(y * PhysicsConstants.SMALL_CARD_VERTICAL_SPACE_BETWEEN_CARDS - PhysicsConstants.SMALL_CARD_Y_POSITION_OFFSET));
		}
		else if(currentLevel.card_size == "med")
		{
			appearingPosition = new Vector3(
						(x * PhysicsConstants.MED_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS - PhysicsConstants.MED_CARD_X_POSITION_OFFSET),
						(y * PhysicsConstants.MED_CARD_VERTICAL_SPACE_BETWEEN_CARDS - PhysicsConstants.MED_CARD_Y_POSITION_OFFSET));
		}
		else if(currentLevel.card_size == "big")
		{
			appearingPosition = new Vector3(
						(x * PhysicsConstants.BIG_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS - PhysicsConstants.BIG_CARD_X_POSITION_OFFSET),
						(y * PhysicsConstants.BIG_CARD_VERTICAL_SPACE_BETWEEN_CARDS - PhysicsConstants.BIG_CARD_Y_POSITION_OFFSET));
		}

		return appearingPosition;
		
	}

	public int GetNonVirusCards()
	{
		int nonVirus = 0;
		foreach(string card in currentLevel.card_arr)
		{
			if(!card.Contains("V"))
			{
				nonVirus++;
			}
		}

		return nonVirus;
	}

	
}
