using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public delegate void EndTimerHandler(Timer sender);
    public event EndTimerHandler OnEndTimer;

    private enum State { EDITABLE, RUNNING, PAUSED, ENDED };
    private float time;
    private State state;
    private Text minText;
    private Text secText;
    public float selectedTime;
    //public Controller controller;
    private Color selectedColor = new Color(.9f, .9f, 1f);

    void Start() {
        time = 0f;
        state = State.EDITABLE;
        minText = transform.Find("Minutes").GetComponent<Text>();
        secText = transform.Find("Seconds").GetComponent<Text>();
    }

    void Update() {
        if (state == State.RUNNING) {
            time -= Time.deltaTime;
            UpdateTimeDisplay();
            if (time <= 0) {
                if (OnEndTimer != null) {
                    OnEndTimer(this);
                }
            }
        }
    }

    //PLAY STOP CONTROLS -------------

    public bool StartTimer() {
        if (state == State.EDITABLE) {
            selectedTime = time;
        }
        if (time > 0) {
            state = State.RUNNING;
        }
        return (state == State.RUNNING);
    }

    public void Pause() {
        state = State.PAUSED;
    }

    public void Restart() {
        time = selectedTime;
        UpdateTimeDisplay();
    }

    public void FullReset() {
        state = State.EDITABLE;
        SetTime(0f);
    }

    public void StopTimer() {
        state = State.ENDED;
    }

    public float LogTime() {
        float elapsedTime = selectedTime - time;
        Restart();
        return elapsedTime;
    }

    //CHANGING THE TIME ---------------

    private void UpdateTimeDisplay() {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        minText.text = minutes.ToString();
        secText.text = seconds.ToString("00");
    }

    public void SetTime(float newTime) {
        time = (newTime <= 0) ? 0 : newTime;
        UpdateTimeDisplay();
    }

    public void AddTime(float addedTime) {
        SetTime(time + addedTime);
    }

    public void SubtractTime(float subtractedTime) {
        SetTime(time - subtractedTime);
    }

    public void RestoreTime(float newTime) {
        selectedTime = (newTime <= 0) ? 0 : newTime;
        Restart();
    }

    //GETTERS -------------------

    public float GetTime() {
        return time;
    }

    public float GetSelectedTime() {
        return selectedTime;
    }

    public bool IsEditable() {
        return state == State.EDITABLE;
    }

    //APPEARANCE

    public void Highlight() {
        minText.color = selectedColor;
        secText.color = selectedColor;
    }

    public void Unhightlight() {
        minText.color = Color.black;
        secText.color = Color.black;
    }

    public void ToggleHighlight() {
        if (minText.color == Color.black) {
            Highlight();
        } else {
            Unhightlight();
        }
    }
}
