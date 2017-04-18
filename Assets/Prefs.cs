using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour {

    const string TIMER_TIME = "timerTime";
    const string LIMIT_TIME = "limitTime";
    const string PLAIN_TIME = "plainTime";
    const string RECORDS = "records";
    const string STATS = "stats";
    const string HIGHEST = "highest";
    const string GOAL = "goal";

	public static void SetTimerTime (float time) {
        PlayerPrefs.SetFloat(TIMER_TIME, time);
    }

    public static float GetTimerTime() {
        return PlayerPrefs.GetFloat(TIMER_TIME);
    }

    public static void SetLimitTime(float time) {
        PlayerPrefs.SetFloat(LIMIT_TIME, time);
    }

    public static float GetLimitTime() {
        return PlayerPrefs.GetFloat(LIMIT_TIME);
    }

    public static void SetPlainTime(float time) {
        PlayerPrefs.SetFloat(PLAIN_TIME, time);
    }

    public static float GetPlainTime() {
        return PlayerPrefs.GetFloat(PLAIN_TIME);
    }

    public static void SetRecords(string records) {
        PlayerPrefs.SetString(RECORDS, records);
    }

    public static string GetRecords() {
        return PlayerPrefs.GetString(RECORDS);
    }

    public static void ClearRecords() {
        PlayerPrefs.DeleteKey(RECORDS);
    }

    public static void SetStats(string stats) {
        PlayerPrefs.SetString(STATS, stats);
    }

    public static string GetStats() {
        return PlayerPrefs.GetString(STATS);
    }

    public static void ClearStats() {
        PlayerPrefs.DeleteKey(STATS);
    }

    public static void SetGoal(float goal) {
        PlayerPrefs.SetFloat(GOAL, goal);
    }

    public static float GetGoal() {
        return PlayerPrefs.GetFloat(GOAL);
    }

    public static void SetHighest(float time) {
        PlayerPrefs.SetFloat(HIGHEST, time);
    }

    public static float GetHighest() {
        return PlayerPrefs.GetFloat(HIGHEST);
    }

    public static void ClearHighest() {
        PlayerPrefs.DeleteKey(HIGHEST);
    }

    public static bool TimerStored() {
        return PlayerPrefs.HasKey(TIMER_TIME);
    }

    public static bool LimitStored() {
        return PlayerPrefs.HasKey(LIMIT_TIME);
    }

    public static bool PlainStored() {
        return PlayerPrefs.HasKey(PLAIN_TIME);
    }

    public static bool RecordsStored() {
        return PlayerPrefs.HasKey(RECORDS);
    }

    public static bool StatsStored() {
        return PlayerPrefs.HasKey(STATS);
    }

    public static bool GoalStored() {
        return PlayerPrefs.HasKey(GOAL);
    }
}
