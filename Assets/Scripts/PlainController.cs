using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlainController : MonoBehaviour {

   public Timer mainTimer;

    private Timer timer;
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
            timer.AddTime(10);
        } else if (Input.GetKeyDown("down")) {
            timer.SubtractTime(10);
        } else if (Input.GetKeyDown("right")) {
            timer.AddTime(60);
        } else if (Input.GetKeyDown("left")) {
            timer.SubtractTime(60);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            StartStop();
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

    public void StartStop() {
        if (state == s.RUNNING) {
            state = s.TIMER;
            timer.Pause();
        } else if (state != s.ENDED) {
            StartTimers();
        }
    }

    private void RestoreTimes() {
        if (Prefs.PlainStored()) {
            mainTimer.RestoreTime(Prefs.GetPlainTime());
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
        Restart();
    }

    private void StoreLastTimes() {
        Prefs.SetPlainTime(mainTimer.GetSelectedTime());
    }
}
