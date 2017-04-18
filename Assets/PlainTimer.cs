using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlainTimer : MonoBehaviour {

    private float time;
    private bool isRunning;
    private bool isPaused;
    public int smallStep = 60;
    private int largeStep;
    private int step;
    private Text minText;
    private Text secText;
    private float selectedTime;
    public PlainController controller;
    private Color selectedColor;

    // Use this for initialization
    void Start() {
        time = 0f;
        isRunning = false;
        isPaused = false;
        //smallStep = 60;
        largeStep = 300;
        minText = transform.Find("Minutes").GetComponent<Text>();
        secText = transform.Find("Seconds").GetComponent<Text>();
        //controller = GameObject.FindObjectOfType<Controller>();
        selectedColor = new Color(.9f, .9f, 1f);
    }

    // Update is called once per frame
    void Update() {
        if (isRunning) {
            time -= Time.deltaTime;
            ChangeTime();
            if (time <= 0) {
                controller.SignalEnd();
            }
        }

    }

    public void RestoreTimes(float time) {
        selectedTime = time;
        Restart();
    }

    public bool StartTimer() {
        if (time > 0) {
            isRunning = true;
        }
        if (!isPaused) {
            selectedTime = time;
        } else { isPaused = false; }
        return isRunning;
    }

    public void Pause() {
        isRunning = false;
        isPaused = true;
    }

    public void TimeHigherSmall() {
        step = smallStep;
        ChangeTime();
    }

    public void TimeLowerSmall() {
        step = -smallStep;
        ChangeTime();
    }

    public void TimeHigherLarge() {
        step = largeStep;
        ChangeTime();
    }

    public void TimeLowerLarge() {
        step = -largeStep;
        ChangeTime();
    }

    private void ChangeTime() {
        if (!isRunning) {
            time = (time + step <= 0) ? 0 : time + step;
        }
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        minText.text = minutes.ToString();
        secText.text = seconds.ToString("00");
    }

    public void FullReset() {
        isRunning = false;
        step = (int)-time - 1;
        ChangeTime();
    }

    public void Restart() {
        time = selectedTime;
        step = 0;
        ChangeTime();
    }

    public float LogTime() {
        float elapsedTime = selectedTime - time;
        Restart();
        return elapsedTime;
    }

    public void StopTimer() {
        isRunning = false;
    }

    public void Highlight() {
        minText.color = selectedColor;
        secText.color = selectedColor;
    }

    public void Unhightlight() {
        minText.color = Color.black;
        secText.color = Color.black;
    }

    public float GetSelectedTime() {
        return selectedTime;
    }
}
