using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour {

	void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(OpenLevelPickerScene);
	}

	void OpenLevelPickerScene()
	{
		SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
	}
}
