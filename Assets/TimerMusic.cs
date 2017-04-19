using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMusic : MonoBehaviour {

    private AudioSource music;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private float fadeStep = 0.3f;

	// Use this for initialization
	void Start () {
        music = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFadingIn && music.volume < 1) {
            music.volume += fadeStep * Time.deltaTime;
        }
	}
}
