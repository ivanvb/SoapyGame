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

	List<KeyValuePair<int, int>> ingredients;


    void Start()
    {
		LoadScore();

		//just to make timer of animatinos work (timeLeft timer is copy to others)
		timeAnimation = timeAnimationDelay;

		//timeLeft bar calculations
		timeFill = (timeLeft / timeTotal);

		float ingri = Mathf.Pow(ingri0, 3) + Mathf.Pow(ingri1, 3) + Mathf.Pow(ingri2, 3); //This is done so 1 (got all ingridient x) is really important.
		ingri = 1;
		//Score calculations
		
		float timeLeftBackup = timeLeft;
		/*if (moveUsed >= movePerfect)
		{
			score = Mathf.Round((((timeLeftBackup / timeTotal) * 100) - ((moveUsed - movePerfect) * 25) + (ingri * 1000) - (virus * 25)) / 10);//resta penalisa, suma premia.
		}
		else
		{
			score = Mathf.Round((((timeLeftBackup / timeTotal) * 100) - ((movePerfect - moveUsed) * 25) + (ingri * 1000) - (virus * 25)) / 10);//resta penalisa, suma premia.
		}*/

		score = 4000;


		if (score < 0)
		{
			score = 0;
		}

		if (moveUsed == 0)
		{
			score = 0;
		}

		scorePerfect = Mathf.Round(((((timeTotal / 1.5f) / timeTotal) * 100) + pointsCollected + (ingriQuantity * 1000)) / 10); //alternitivly, just use pointscollected, it must be the same as the moveperfect thing.
																																//each hardcored integer in the formula determines how much weight that element of gameplay has on the score. Ingri is n3 so value 1 is so much important than incomplete ingridients.
		ingriQuantity = 3;
		if (score > scorePerfect)
		{
			scorePerfect = score; // so soapImageBar stay within 1...cuz score/scoreP would b hihger than 1
		}

		if (moveUsed == movePerfect && ingri == ingriQuantity)
		{
			scorePerfect = scorePerfect + timeLeftBackup * 10;
			score = scorePerfect;
		}
		//disabling UI for animation purposes
		scoreTextGameObject.SetActive(false);

		StartCoroutine(fillTimeCircle());
		//StartCoroutine(soapFill());
		//StartCoroutine(scoreFill());
		//StartCoroutine(fillMovesCircle());

	}

    void Update() //most scene logic is here. Is wastefull as it checks everything on each update.
    {

        //display scores
        /*timeTextObject.text = "Time left: " + timeLeft;
        timeImageFill.fillAmount = timeFill;

        //Animation of time score
        if (timeLeft > 0) //checks "timeLeft" Score
        {
            if (Time.time > timeAnimation)
            {
                timeAnimation = Time.time + timeAnimationDelay;
                timeLeft = timeLeft - 1f;
                timeFill = ((timeLeft - .1f) / timeTotal);
                audioTimeleft.Play();
            }


        }

		 //Moves Score animation
		 if (timeLeft <= 0) //Flag to know timeLeft animation finish. Must be changed. Needs rework as probably there would be animations inbetween scene events
		 {
			 timeImageFill.color = Color.grey;
			 if (Time.time > timeAnimation) //because last timeleft gives a new entry time.
			 {
				 timeAnimation = Time.time + timeAnimationDelay;

				 if (moveCount < movePerfect)
				 {
					 moveTextObject.text = "Moves: " + moveCount;
					 moveCount = moveCount + 1;
					 moveImageObject.fillAmount = moveImageObject.fillAmount - 1 / movePerfect;
					 audioMoves.Play();
				 }//comparing moveUsed to movePerfect

				 if (movePerfect == moveUsed && moveCount == movePerfect)
				 {
					 moveTextObject.color = Color.green;
					 moveTextObject.text = "Perfect moves!";
				 }

				 if (moveUsed != movePerfect && moveCount >= movePerfect)
				 {
					 moveTextObject.text = "Moves: " + moveCount;
					 moveTextObject.color = Color.yellow;

					 if (moveCount < moveUsed)
					 {

						 moveCount = moveCount + 1;
						 moveImageObject0.fillAmount = moveImageObject0.fillAmount - (1 / (2 * movePerfect));
						 // x2 so if you need souble the moves to win, you are doing pre-tty bad.
						 audioMoves.Play();
					 }
				 }

			 }
		 }*/

		 //Score animation
		 /*if (moveCount == moveUsed && flagFinish == 0)
		 {
			 divisorImageObject.SetActive(true);
			 scoreTextGameObject.SetActive(true);

			 scoreTextObject.text = "Score: " + score;

			 if (scoreSoapImageObject0.fillAmount < ingPoints)
			 {

				 scoreSoapImageObject0.fillAmount = scoreSoapImageObject0.fillAmount + 0.01f;
			 }
			 else
			 {
				 audioScore.Play();
				 flagFinish = 1;
			 }

		 }*/

		
    }

	private void LoadScore()
	{
		ScoreComponents scoreComponents =  GameObject.Find("Communicator").GetComponent<Communicator>().scoreComponents;
		timeLeft = scoreComponents.timeTaken;
		timeTotal = scoreComponents.maxTime;

		moveUsed = scoreComponents.moves;
		movePerfect = scoreComponents.movePerfect;

		virus = scoreComponents.amountOfVirusFlipped;

		ingredients = scoreComponents.ingredients;
		ingPoints = GetIngredientsFraction();
		won = (ingPoints == 1) ? true : false;
	}

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

	IEnumerator soapFill()
	{
		while(scoreSoapImageObject0.fillAmount < ingPoints)
		{
			scoreSoapImageObject0.fillAmount = scoreSoapImageObject0.fillAmount + 0.01f;
			yield return null;
		}
		audioScore.Play();
		
	}

	IEnumerator scoreFill()
	{
		audioTimeleft.loop = true;
		audioTimeleft.Play();
		scoreTextGameObject.SetActive(true);
		
		float maxScore = score;
		int currentScore = 0;

		while(currentScore < maxScore)
		{
			scoreTextObject.text = "score: " + currentScore;
			System.Random random = new System.Random();
			currentScore += random.Next(50, 225);
			
			yield return null;
		}

		scoreTextObject.text = "score: " + maxScore;
		audioTimeleft.Stop();
		StartCoroutine(soapFill());
	}

	IEnumerator fillTimeCircle()
	{
		Debug.Log(timeLeft);
		audioMoves.Play();
		audioMoves.loop = true;
		timeTextObject.text = "Time left: ";
		timeImageFill.color = Color.grey;
		float usedTime = ((timeLeft * 100)/timeTotal) * 0.01f;

		timeImageFill.fillAmount = 1;
		float fillTime = 0;
		while(fillTime < usedTime)
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

	IEnumerator fillMovesCircle()
	{
		float moves = ((moveUsed * 100) / (float)movePerfect) * 0.01f;

		float fillMoves = 0;

		moveImageObject.fillAmount = 1f;
		while(fillMoves < moves)
		{
			moveTextObject.text = "Moves: " + new System.Random().Next(10, 99).ToString();
			fillMoves += 0.005f;
			if (moveImageObject.fillAmount > 0 && moveUsed > movePerfect)
			{
				moveImageObject.fillAmount -= 0.005f;
			}
			else
			{
				moveImageObject0.fillAmount -= 0.005f;
			}
			
			yield return null;
		}

		moveTextObject.color = (moveUsed == movePerfect && won) ? Color.green : Color.yellow;
		moveTextObject.text = "Moves: " + moveUsed.ToString();
		audioMoves.Stop();
		StartCoroutine(scoreFill());
	}


}
