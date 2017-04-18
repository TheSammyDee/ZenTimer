using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlainController : MonoBehaviour {

   public PlainTimer mainTimer;

    private PlainTimer timer;
    private enum s { TIMER, LIMIT, RUNNING, ENDED };
    private s state;
    private AudioSource music;
    private AudioSource gong;
    private bool musicFading;

    // Use this for initialization
    void Start() {
        timer = mainTimer;
        state = s.TIMER;
        music = GetComponent<AudioSource>();
        gong = transform.Find("Gong").GetComponent<AudioSource>();
        musicFading = false;
        RestoreTimes();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("up")) {
            timer.TimeHigherSmall();
        } else if (Input.GetKeyDown("down")) {
            timer.TimeLowerSmall();
        } else if (Input.GetKeyDown("right")) {
            timer.TimeHigherLarge();
        } else if (Input.GetKeyDown("left")) {
            timer.TimeLowerLarge();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            if (state == s.RUNNING) {
                state = s.TIMER;
                timer.Pause();
            } else if (state != s.ENDED) {
                StartTimers();
            }
        } else if (Input.GetKeyDown(KeyCode.Backspace)) {
            FullReset();
        } else if (Input.GetKeyDown(KeyCode.RightShift)) {
            Restart();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            SceneManager.LoadScene("Timer");
        }
        if (musicFading) {
            music.volume -= 0.3f * Time.deltaTime;
            if (music.volume <= 0) {
                music.Stop();
                music.volume = 1;
                musicFading = false;
            }
        }

    }

    private void RestoreTimes() {
        if (Prefs.PlainStored()) {
            mainTimer.RestoreTimes(Prefs.GetPlainTime());
        }
    }

    private void StartTimers() {
        if (mainTimer.StartTimer()) {
            StoreLastTimes();
            music.Play();
            state = s.RUNNING;
        }
    }

    private void FullReset() {
        state = s.TIMER;
        mainTimer.FullReset();
        music.Stop();
    }

    private void Restart() {
        FullReset();
        mainTimer.Restart();
    }

    public void SignalEnd() {
        state = s.ENDED;
        musicFading = true;
        gong.Play();
        mainTimer.StopTimer();
    }

    private void StoreLastTimes() {
        Prefs.SetPlainTime(mainTimer.GetSelectedTime());
    }
}
