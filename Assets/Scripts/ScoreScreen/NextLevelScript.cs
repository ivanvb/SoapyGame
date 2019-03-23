using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(OpenNextLevel);
	}
	
	void OpenNextLevel()
	{
		Communicator communicator = GameObject.Find("Communicator").GetComponent<Communicator>();
		LevelList levelList = communicator.levelList;
		Level currentLevel = communicator.GetLevel();
		int currentIndex = levelList.Level.IndexOf(currentLevel);
		
		if(levelList.Level[currentIndex + 1] != null)
		{
			communicator.SetLevel(levelList.Level[currentIndex + 1]);
			SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
		}
	}
}
