using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTrackerController : MonoBehaviour {

    [SerializeField]
    private Timer mainTimer, limitTimer;
    [SerializeField]
    private TimesLogger timeLog, statsLog, recordsLog;
    [SerializeField]
    private Image bgImage;
    [SerializeField]
    private Music timerMusic;

    private int goalLeeway = 30;
    private Timer selectedTimer;
    //private enum s 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
