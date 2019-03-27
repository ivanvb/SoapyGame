using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

  class ScorePresentation : MonoBehaviour
{
      public Text timeTextObject;
      public Text moveTextObject;
      public Text scoreTextObject;


      public Image timeImageFill;
      public Image moveImageObject;//upfront to back.
      public Image moveImageObject0;
      public Image scoreImageObject0;
      public Image scoreImageObject1;
      public Image scoreSoapImageObject0;
      public Image scoreSoapImageObject1;

      public GameObject scoreTextGameObject;
      public GameObject divisorImageObject;

      public AudioSource audioTimeleft;
      public AudioSource audioMoves;
      public AudioSource audioScore;

      public float timeAnimationDelay;
      public float animationSmooth;

    protected float timeAnimation;
    protected float timeAnimation0;
    protected float timeFill;
    protected int moveCount = 0;
    protected float score;
    protected float scorePerfect;
    protected int flagFinish = 0;

    

    //to be passed by game scene
      float timeLeft;
      float timeTotal; //total time given
      float movePerfect = 12;
      float moveUsed = 4;
      int pointsCollected; //cuz maybe he got a streak
      float ingriQuantity = 3;//numbers of ingridientss. regardless, its a fraction so max always is 1.
    //discuss how team handles ingri fractions so what it could pass easily...total ingri1 and collected Ingri1 or numbersOfIngri and ingriSUM, or an int and always /3 or a float .....??? ingru number varies.    
      float ingri0 = 0;
      float ingri1 = 0 ;
      float ingri2 = 0;
    //  static ArrayList ingriArray = new ArrayList<> { ingri0, ingri1, ingri2 }; how the heck to declare array list in c#
      int virus;
	bool won = false;

	float ingPoints;
	int cardsMatched = 0;

	List<KeyValuePair<int, int>> ingredients;


    void Start()
    {
		LoadScoreComponents();
		CalculateScore();
		
		StartCoroutine(fillTimeCircle());
		//SaveToJson();
	}

    
	/* Loads Score from persistent GameObject whose purpose is to communicate between scenes */
	private void LoadScoreComponents()
	{
		ScoreComponents scoreComponents =  GameObject.Find("Communicator").GetComponent<Communicator>().scoreComponents;
		timeLeft = scoreComponents.timeTaken;
		timeTotal = scoreComponents.maxTime;

		moveUsed = scoreComponents.moves;
		movePerfect = scoreComponents.movePerfect;

		virus = scoreComponents.amountOfVirusFlipped;

		cardsMatched = scoreComponents.matchedCards;
		ingredients = scoreComponents.ingredients;
		ingPoints = GetIngredientsFraction();
		won = (ingPoints == 1) ? true : false;
	}

	private void CalculateScore()
	{
		score += cardsMatched * 250;
		score += System.Convert.ToInt32(won) * 1150;
		score += ingPoints * 150f;
		score += (timeLeft / timeTotal) * 300;

		score -= virus * 200;
		score = (moveUsed > movePerfect) ? score -= (moveUsed - movePerfect) * 100 : score;

		score = (score <= 0) ? 0 : score;
		score = (int)score;
	}

	/* Gets a fraction based on the ingredients collected by the user, for this it sums the division of the amount collected 
	 * of that ingredient/the amount that the user is asked to collect, then adds each one of those sums and divides them by the
	 * amount of different ingredients asked to the user*/

	private float GetIngredientsFraction()
	{
		float ingPoints = 0;
		if (ingredients.Count > 0)
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				ingPoints += ingredients[i].Key / (float)ingredients[i].Value;
			}

			ingPoints = ingPoints/ingredients.Count;
		}

		return ingPoints;
	}

	/* Fills the circle related to the time and sets the time used Text once its finished, while it is filling the circle
	 * it is generating random numbers so the user sees a calculating-like animation. Once it's done starts a coroutine on
	 * the moves circle*/
	IEnumerator fillTimeCircle()
	{
		audioMoves.Play();
		audioMoves.loop = true;
		timeTextObject.text = "Time left: ";
		timeImageFill.color = Color.grey;
		float usedTime = ((timeLeft * 100) / timeTotal) * 0.01f;

		timeImageFill.fillAmount = 1;
		float fillTime = 0;
		while (fillTime < usedTime)
		{
			fillTime += 0.01f;
			timeImageFill.fillAmount -= 0.01f;
			timeTextObject.text = "Time left: " + new System.Random().Next(10, 99).ToString();

			yield return null;
		}

		timeTextObject.text = "Time left: " + ((int)timeLeft).ToString();
		timeTextObject.enabled = true;

		StartCoroutine(fillMovesCircle());
	}

	/*Fills the circle of the moves, if the user takes more moves than the required for a MovePerfect, a red circle
	 * indicating the Movement Surplus starts to get filled. Random numbers are generated and set as the move used amount
	 * while the circle is being filled to give the illusion of "calculations". Once this coroutine is done calls ScoreFill() */

	IEnumerator fillMovesCircle()
	{
		float moves = ((moveUsed * 100) / (float)movePerfect) * 0.01f;

		float fillMoves = 0;

		Debug.Log(moveUsed + " " + movePerfect);
		Debug.Log(moves);
		
		moveImageObject.fillAmount = 1f;
		while (fillMoves < moves && moveImageObject0.fillAmount > 0)
		{
			moveTextObject.text = "Moves: " + new System.Random().Next(10, 99).ToString();
			fillMoves += 0.01f;
			if (moveImageObject.fillAmount > 0)
			{
				moveImageObject.fillAmount -= 0.01f;
			}
			else
			{
				moveImageObject0.fillAmount -= 0.01f;
			}

			yield return null;
		}

		moveTextObject.color = (moveUsed == movePerfect && won) ? Color.green : Color.yellow;
		moveTextObject.text = "Moves: " + moveUsed.ToString();
		audioMoves.Stop();
		StartCoroutine(scoreFill());
	}

	/* Increments the score on random numbers until they reach the score obtained by the user. Once
	 * it is finished calls the soapFill() coroutine. */
	IEnumerator scoreFill()
	{
		audioTimeleft.loop = true;
		audioTimeleft.Play();
		scoreTextGameObject.SetActive(true);

		float maxScore = score;
		int currentScore = 0;

		while (currentScore < maxScore)
		{
			scoreTextObject.text = "score: " + currentScore;
			System.Random random = new System.Random();
			currentScore += random.Next(10, 40);

			yield return null;
		}

		scoreTextObject.text = "score: " + maxScore;
		audioTimeleft.Stop();
		StartCoroutine(soapFill());
	}

	/* Fills the soap image according to the ingredients obtained by the user */
	IEnumerator soapFill()
	{
		while(scoreSoapImageObject0.fillAmount < ingPoints)
		{
			scoreSoapImageObject0.fillAmount = scoreSoapImageObject0.fillAmount + 0.01f;
			yield return null;
		}
		audioScore.Play();
		
	}

	/*private void SaveToJson()
	{
		
		
		Level currentlevel = GameObject.Find("Communicator").GetComponent<Communicator>().GetLevel();
		LevelList levels = GameObject.Find("Communicator").GetComponent<Communicator>().levelList;
		if (score > currentlevel.score)
		{
			currentlevel.score = (int)score;
			string s = JsonUtility.ToJson(levels);
			System.IO.File.WriteAllText(Application.dataPath + "/Resources/Level.json", s);
			levels.Level[levels.Level.FindIndex(ind => ind.Equals(currentlevel))] = currentlevel;
		}
		
		

		Debug.Log(currentlevel.score);
	}*/

	

	

	

	


}
