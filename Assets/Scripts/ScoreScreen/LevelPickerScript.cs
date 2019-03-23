using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPickerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(OpenLevelPickerScene);
	}
	
	void OpenLevelPickerScene()
	{
		SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
	}
}
