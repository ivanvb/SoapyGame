using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour {

	Button thisButton;
	Text text;
	// Use this for initialization
	void Start () {
		thisButton = gameObject.GetComponent<Button>();
		text = thisButton.GetComponentInChildren<Text>();
		thisButton.onClick.AddListener(clicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void clicked()
	{
		Level level = GameObject.Find("Scripter").GetComponent<LevelController>().GetLevel(Convert.ToInt32(text.text) - 1);
		GameObject.Find("Communicator").GetComponent<Communicator>().SetLevel(level);
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}
}
