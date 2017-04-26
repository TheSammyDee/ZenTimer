using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimesLogger : MonoBehaviour {

    private Text logText;

	// Use this for initialization
	void Start () {
        logText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string AddTimeText(float time) {
        string oldText = logText.text;
        logText.text = oldText + ConvertTimeToString(time) + "\n";
        return logText.text;
    }

    public string AddDatedTimeText(float time) {
        string oldText = logText.text;
        string newText = System.DateTime.Now.ToString("MMM dd") + " " + ConvertTimeToString(time) + "\n";
        logText.text = oldText + newText;
        return logText.text;
    }

    public string ConvertTimeToString(float time) {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string timeString = minutes.ToString() + ":" + seconds.ToString("00");
        return timeString;
    }

    public void ClearText() {
        logText.text = "";
    }
}
