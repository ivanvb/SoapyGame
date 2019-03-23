using System.Collections.Generic;

/* Stores Level information, it is intended to be filled in from a .json */
[System.Serializable]
public class Level
{

	public List<int> tablero;
	public int timer;
	public string card_size;
	public List<string> card_arr;
	public List<string> ingredients;
	public int score;
	public int stars;

}
