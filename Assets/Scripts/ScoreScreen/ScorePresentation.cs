using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePresentation : MonoBehaviour
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
    public float timeLeft;
    public float timeTotal; //total time given
    public float movePerfect;
    public float moveUsed;
    public int pointsCollected; //cuz maybe he got a streak
    public float ingriQuantity;//numbers of ingridientss. regardless, its a fraction so max always is 1.
    //discuss how team handles ingri fractions so what it could pass easily...total ingri1 and collected Ingri1 or numbersOfIngri and ingriSUM, or an int and always /3 or a float .....??? ingru number varies.    
    public float ingri0;
    public float ingri1;
    public float ingri2;
    //public static ArrayList ingriArray = new ArrayList<> { ingri0, ingri1, ingri2 }; how the heck to declare array list in c#
    public static float virus;


    void Start()
    {
		LoadScore();
        //just to make timer of animatinos work (timeLeft timer is copy to others)
        timeAnimation = timeAnimationDelay;

        //timeLeft bar calculations
        timeFill = (timeLeft / timeTotal);


        //Score calculations
        float timeLeftBackup = timeLeft;
        score = Mathf.Round((timeLeftBackup * 40) + pointsCollected*40 - ((moveUsed - movePerfect) * 30) + ((ingri0 * ingri0 * ingri0 + ingri1 * ingri1 * ingri1 + ingri2 * ingri2 * ingri2) * 200) - (virus * 30));//resta penalisa, suma premia.
        scorePerfect = Mathf.Round((timeTotal * 40) + ((movePerfect-1)*40) + (ingriQuantity * 200)); //alternitivly, just use pointscollected, it must be the same as the moveperfect thing.
        //each hardcored integer in the formula determines how much weight that element of gameplay has on the score. Ingri is n3 so value 1 is so much important than incomplete ingridients.

        //disabling UI for animation purposes
        scoreTextGameObject.SetActive(false);

    }

    void Update() //most scene logic is here. Is wastefull as it checks everything on each update.
    {

        //display scores
        timeTextObject.text = "Time left: " + timeLeft;
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
        }

        //Score animation
        if (moveCount == moveUsed && flagFinish == 0)
        {
            divisorImageObject.SetActive(true);
            scoreTextGameObject.SetActive(true);

            scoreTextObject.text = "Score: " + score;
            
            

            if (scoreImageObject0.fillAmount < score/scorePerfect)
            {
                scoreImageObject0.fillAmount = scoreImageObject0.fillAmount + .05f;
                scoreSoapImageObject0.fillAmount = scoreSoapImageObject0.fillAmount + 0.05f;
            }
            else
            {
                audioScore.Play();
                flagFinish = 1;
            }

        }

    }

	private void LoadScore()
	{
		ScoreComponents scoreComponents =  GameObject.Find("Communicator").GetComponent<Communicator>().scoreComponents;
		timeLeft = scoreComponents.maxTime - scoreComponents.timeTaken;
		timeTotal = scoreComponents.maxTime;

		moveUsed = scoreComponents.moves;
		movePerfect = scoreComponents.movePerfect;

	}



}
