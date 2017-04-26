using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    public Timer mainTimer;
    public Timer limitTimer;
    public Text timeLog;
    public Text recordLog;
    public Text statsLog;
    public Image bgImage;
    public int goalLeeway = 30;

    //for test viewing in inspector
    public float testTime;

    private Timer timer;
    private enum s {TIMER, LIMIT, RUNNING, ENDED };
    private s state;
    private AudioSource music;
    private AudioSource gong;
    private bool musicFading;
    public float highestRecord;
    public float highestThisRound;
    private bool lastChance;
    private float elapsedTime = 0;
    public float goal;

    // Use this for initialization
    void Start() {
        timer = mainTimer;
        timer.Highlight();
        state = s.TIMER;
        music = GetComponent<AudioSource>();
        gong = transform.Find("Gong").GetComponent<AudioSource>();
        musicFading = false;
        lastChance = false;
        if (Prefs.GoalStored()) {
            goal = Prefs.GetGoal();
        } else {
            goal = 60f;
        }
        //RestoreTimes();
        SetGoalTimes();
        DisplayRecords();
        DisplayStats();
        mainTimer.OnEndTimer += SignalEnd;
        limitTimer.OnEndTimer += SignalEnd;
    }

    // Update is called once per frame
    void Update() {
        if (state == s.RUNNING) {
            elapsedTime += Time.deltaTime;
        }
        if (Input.GetKeyDown("up")) {
            timer.AddTime(10);
        } else if (Input.GetKeyDown("down")) {
            timer.SubtractTime(10);
        } else if (Input.GetKeyDown("right")) {
            timer.AddTime(60);
        } else if (Input.GetKeyDown("left")) {
            timer.SubtractTime(60);
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            if (state == s.TIMER) {
                state = s.LIMIT;
                timer.Unhightlight();
                timer = limitTimer;
                timer.Highlight();
            } else if (state == s.LIMIT) {
                state = s.TIMER;
                timer.Unhightlight();
                timer = mainTimer;
                timer.Highlight();
            }           
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            if (state == s.RUNNING) {
                CheckForEnd();
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
            SceneManager.LoadScene("PlainTimer");
        } else if (Input.GetKey("c")) {
           if (Input.GetKeyDown("r")) {
                ClearRecords();
            } else if (Input.GetKeyDown("s")) {
                ClearStats();
            }

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
        if (Prefs.TimerStored()) {
            mainTimer.RestoreTime(Prefs.GetTimerTime());
        }
        if (Prefs.LimitStored()) {
            limitTimer.RestoreTime(Prefs.GetLimitTime());
        }
    }

    private void SetGoalTimes() {
        if (Prefs.LimitStored()) {
            limitTimer.RestoreTime(Prefs.GetLimitTime());
        }
        mainTimer.RestoreTime(goal + (float)goalLeeway);
    }

    private void StartTimers() {
        if (mainTimer.StartTimer()&&(state==s.TIMER||state==s.LIMIT)) {
            timer.Unhightlight();
            StoreLastTimes();
            if (limitTimer.GetTime() > mainTimer.GetTime()) {
                limitTimer.RestoreTime(limitTimer.GetTime() - mainTimer.GetTime());
            }
            limitTimer.StartTimer();
            music.Play();
            state = s.RUNNING;
            HideUI();
        }
    }

    private void HideUI() {
        bgImage.GetComponent<RectTransform>().SetAsLastSibling();
    }

    private void ShowUI() {
        bgImage.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    private void FullReset() {
        state = s.TIMER;
        mainTimer.FullReset();
        limitTimer.FullReset();
        timer = mainTimer;
        timer.Highlight();
        limitTimer.Unhightlight();
        timeLog.text = "";
        music.Stop();
    }

    private void Restart() {
        FullReset();
        mainTimer.Restart();
        limitTimer.Restart();
        SetGoalTimes();
    }

    private void CheckForEnd() {
        if (lastChance) {
            FullEnd();
        } else {
            LogTime();
        }
    }

    private void LogTime() {
        float time = mainTimer.LogTime();
        testTime = time;
        string oldText = timeLog.text;
        timeLog.text = oldText + ConvertTimeToString(time) + "\n";
        if (Mathf.Floor(time) > highestThisRound) {
            highestThisRound = time;
        }
    }

    private string ConvertTimeToString(float time) {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string loggedTime = minutes.ToString() + ":" + seconds.ToString("00");
        return loggedTime;
    }

    public void SignalEnd(Timer sender) {
        //float mainTime = mainTimer.GetTime();
        if (sender.GetInstanceID() == mainTimer.GetInstanceID()) {
            if (!lastChance) { limitTimer.StopTimer(); }
            FullEnd();
        } else {
            lastChance = true;
            limitTimer.StopTimer();
        }
    }

    private void FullEnd() {
        state = s.ENDED;
        lastChance = false;
        musicFading = true;
        gong.Play();
        LogTime();
        mainTimer.StopTimer();
        limitTimer.RestoreTime(elapsedTime);
        elapsedTime = 0;
        if (Mathf.Floor(highestThisRound) > highestRecord) {
            AddNewRecord();
        }
        AddNewStat();
        AdjustGoal();
        ShowUI();
        highestThisRound = 0;
    }

    private void AdjustGoal() {
        if (Mathf.Floor(highestThisRound) < goal) {
            goal -= 10;
        } else {
            goal = Mathf.Floor(highestThisRound);
        }
        Prefs.SetGoal(goal);
    }

    private void AddNewRecord() {
        Prefs.SetHighest(highestThisRound);
        highestRecord = highestThisRound;
        AddToTimesList(recordLog, highestThisRound);
        Prefs.SetRecords(recordLog.text);
    }

    private void AddNewStat() {
        AddToTimesList(statsLog, highestThisRound);
        Prefs.SetStats(statsLog.text);
    }

    private void AddToTimesList(Text textList, float numToAdd) {
        string oldText = textList.text;
        string newText = System.DateTime.Now.ToString("MMM dd") + " " + ConvertTimeToString(numToAdd) + "\n";
        textList.text = oldText + newText;
    }

    private void StoreLastTimes() {
        Prefs.SetTimerTime(mainTimer.GetSelectedTime());
        Prefs.SetLimitTime(limitTimer.GetSelectedTime());
    }

    private void DisplayRecords() {
        if (Prefs.RecordsStored()) {
            recordLog.text = Prefs.GetRecords();
            highestRecord = Prefs.GetHighest();
        }
    }

    private void ClearRecords() {
        Prefs.ClearRecords();
        Prefs.ClearHighest();
        highestRecord = 0;
        recordLog.text = "";
    }

    private void DisplayStats() {
        if (Prefs.StatsStored()) {
            statsLog.text = Prefs.GetStats();
        }
    }

    private void ClearStats() {
        Prefs.ClearStats();
        statsLog.text = "";
    }
}
