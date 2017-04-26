using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

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
		if (isFadingIn) {
            music.volume += fadeStep * Time.deltaTime;
            if (music.volume >= 1) isFadingIn = false;
        } else if (isFadingOut) {
            music.volume -= fadeStep * Time.deltaTime;
            if (music.volume <= 0) isFadingOut = false;
        }
	}

    public void FadeInMusic() {
        isFadingIn = true;
        isFadingOut = false;
    }

    public void FadeOutMusic() {
        isFadingOut = true;
        isFadingIn = false;
    }
}
