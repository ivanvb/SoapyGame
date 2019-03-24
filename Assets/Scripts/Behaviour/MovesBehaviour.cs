using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesBehaviour : MonoBehaviour {

	int moves = 0;

	// Use this for initialization

	public void onMovement()
	{
		IncrementCounter();
		UpdateScoreLabel();
	}

	private void IncrementCounter()
	{
		moves++;
	}

	private void UpdateScoreLabel()
	{
		Text text = GetComponent<Text>();
		text.text = "Moves: " + moves.ToString();
	}

	public int GetScore()
	{
		return moves;
	}
}
