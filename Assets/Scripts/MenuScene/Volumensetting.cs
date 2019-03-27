using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumensetting : MonoBehaviour
{
    public Slider volume;
    public AudioSource mymusic;

	private void Start()
	{
		
	}
	// Update is called once per frame
	void Update()
    {
		if(volume != null)
		{
			mymusic.volume = volume.value;
		}
        
    }
}
