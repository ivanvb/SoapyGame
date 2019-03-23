using UnityEngine;
using UnityEngine.UI;

/**
 * Class that loads the level information and stores it in a LevelList object so it can
 * send a level on request.
 */

public class LevelController : MonoBehaviour {

	public LevelList levelList;
	public Button buttonPrefab;
	public Canvas canvas;
	// Use this for initialization
	void Start () {
		TextAsset t = Resources.Load("Level") as TextAsset;

		levelList = JsonUtility.FromJson<LevelList>(t.text);
		GameObject.Find("Communicator").GetComponent<Communicator>().levelList = levelList;
		InstantiateButtons();

	}

	/* Returns the level in the position specified in the parameters. If
	 * it doesn't exists returns null. */
	public Level GetLevel(int levelNumber)
	{
		if(levelNumber < levelList.Level.Count)
		{
			return levelList.Level[levelNumber];
		}

		return null;
	}

	private void InstantiateButtons()
	{
		int j = 0;
		for (int i = 0; i < levelList.Level.Count; i++)
		{
			if (i!= 0 && i % 4 == 0) j++;
			InstantiateButton(i,j);
		}
	}

	private Vector3 CalculateAppearingPosition(int x, int y)
	{
		float appearingX = (-550) + (360) * x;
		float appearingY = 945 - (360) * y;

		return new Vector3(appearingX, appearingY);
	}

	private void InstantiateButton(int i, int j)
	{
		Button newButton = buttonPrefab;
		newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();
		newButton = Instantiate(newButton, CalculateAppearingPosition(i%4, j), new Quaternion());
		newButton.transform.SetParent(canvas.transform, false);
	}
	
	
}
