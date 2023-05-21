using System;
using UnityEngine;

public class GetRealTime : MonoBehaviour
{
    public static GetRealTime instance;

    public const string PREVIOUS_DATE_KEY = "PreviousDate";
    public DateTime quitTime;
    public DateTime currentTime;
    public TimeSpan difference;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GetDates()
    {
        GetPreviousDate();
        GetCurrentDate();
        difference = currentTime - quitTime;
        Debug.Log("Last DateTime: " + quitTime);
        Debug.Log("Current DateTime: " + currentTime);
        Debug.Log("Difference = " + difference.TotalSeconds);
        if (quitTime == DateTime.MinValue)
        {
            Debug.Log("quit time is null");
        }
        else
        {
            difference = currentTime - quitTime;
            Timer.Instance.SecondsAfterQuit((float)difference.TotalSeconds);
        }
    }

    void GetPreviousDate()
    {
        string date = PlayerPrefs.GetString(PREVIOUS_DATE_KEY);
        if (string.IsNullOrEmpty(date))
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        quitTime = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", null);
    }
    void GetCurrentDate()
    {
        currentTime = DateTime.Now;
    }
    private void OnApplicationQuit()
    {
        OnStopTimer();
    }
    private void OnApplicationFocus(bool pause)
    {
        if (!pause)
            OnStopTimer();
    }

    private void OnStopTimer()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        PlayerPrefs.SetString(PREVIOUS_DATE_KEY, date);
        PlayerPrefs.SetFloat(Timer.HEART_TIME_LEFT_KEY, Timer.Instance.heartTimeLeft);
    }
}
