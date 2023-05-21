using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public const string HEART_TIME_LEFT_KEY = "HeartTimeLeft";
    public const float HeartTimerValue = 600;

    public float heartTimeLeft;
    public DateTime pauseTime;
    public DateTime quitTime;
    public bool heartTimerOn = false;
    public string timerText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        HeartController.Start();
        heartTimeLeft = PlayerPrefs.GetFloat(HEART_TIME_LEFT_KEY);
        if (heartTimeLeft == 0)
        {
            heartTimeLeft = HeartTimerValue;
        }
        GetRealTime.instance.GetDates();
    }
    void Update()
    {
        if (HeartController.heartCount != HeartController.maxHeartCount)
        {
            heartTimerOn = true;
            if (heartTimeLeft > 0)
            {
                heartTimeLeft -= Time.deltaTime;
                //UpdateTimer();
            }
            else
            {
                HeartController.AddHeart(1);
                heartTimeLeft = HeartTimerValue;
            }
        }
        else
        {
            heartTimerOn = false;
        }
    }
    public void SecondsAfterQuit(float sec)
    {
        heartTimeLeft -= sec;
        while (heartTimeLeft < 0)
        {
            if (HeartController.heartCount != HeartController.maxHeartCount)
                HeartController.AddHeart(1);
            heartTimeLeft += HeartTimerValue;
        }
    }
    public string UpdateTimer()
    {
        var temp = heartTimeLeft;
        float minutes = Mathf.FloorToInt(temp / 60);
        float seconds = Mathf.FloorToInt(temp % 60);

        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
