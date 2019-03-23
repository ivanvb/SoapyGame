using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("Scripter").GetComponent<CardMatchController>().onVirusFlipped += VirusReceiver;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void VirusReceiver(string virus_tag)
	{
		if(virus_tag == "virus1")
		{
			GameObject.Find("Scripter").GetComponent<CardCommander>().SetAllCardsDown();
		}
		else if(virus_tag == "virus2")
		{
			Debug.Log("Hola");
			GameObject.Find("TimeSlider").GetComponent<TimerBehaviour>().TimerVirus(10f);
		}else if(virus_tag == "virus3")
		{
			Debug.Log("Hola2");
		}
	}

	private void ExecuteVirus3()
	{

	}
}
