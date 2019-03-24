using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator : MonoBehaviour {

	public Level level;
	public LevelList levelList;
	public ScoreComponents scoreComponents { get; set; }
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetLevel(Level level)
	{
		this.level = level;
	}

	public Level GetLevel()
	{
		return level;
	}

	public void SetScoreComponent(ScoreComponents scoreComponents)
	{
		this.scoreComponents = scoreComponents;
	}

}
