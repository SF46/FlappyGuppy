using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    Coin,
    Diamond
}
[Serializable]
public class DailyRewardData
{
    public RewardType rewardType;
    public int rewardAmount;
}
public delegate void OnDailyRewardDelegate();
public delegate void OnDailyRewardCompleteDelegate();
public class DailyRewardSystem : MonoBehaviour
{
    public const string CONSECUTIVE_COUNT_KEY = "ConsecutiveCount";
    public const string REWARD_CLAIM_DATE_KEY = "RewardClaimDate";
    public List<DailyRewardData> rewards;
    private int consecutiveCount;
    private const int maxDays = 7;
    private const int hoursInDay = 24;
    private DateTime claimDate;

    public event OnDailyRewardDelegate OnDailyRewardEvent;
    public event OnDailyRewardCompleteDelegate OnRewardComplete;

    [SerializeField] Button collectButton;

    private void OnEnable()
    {
        collectButton.onClick.AddListener(CollectReward);
    }

    private void OnDisable()
    {
        collectButton.onClick.RemoveListener(CollectReward);
    }
    void Start()
    {
        LoadConsecutiveInfo();
        GetClaimDate();
        CheckRewardInfo();
    }

    void LoadConsecutiveInfo()
    {
        consecutiveCount = PlayerPrefs.GetInt(CONSECUTIVE_COUNT_KEY);
    }

    void SetConsecutiveInfo()
    {
        PlayerPrefs.SetInt(CONSECUTIVE_COUNT_KEY, consecutiveCount);
    }
    void GetClaimDate()
    {
        string rewardClaimDateString = PlayerPrefs.GetString(REWARD_CLAIM_DATE_KEY);
        if (string.IsNullOrEmpty(rewardClaimDateString))
        {
            claimDate = DateTime.Today.AddDays(1).AddSeconds(-1);
            var dateString = claimDate.ToString("dd/MM/yyyy HH:mm:ss");
            PlayerPrefs.SetString(REWARD_CLAIM_DATE_KEY, dateString);
        }
        else
        {
            Debug.Log("Reward claim date string: " + rewardClaimDateString);
            claimDate = DateTime.ParseExact(rewardClaimDateString, "M/d/yyyy h:mm:ss tt", null);
        }
    }
    void SetClaimDate(DateTime date)
    {
        PlayerPrefs.SetString(REWARD_CLAIM_DATE_KEY, date.ToString());
    }
    void CheckRewardInfo()
    {
        Debug.Log($"Now date is {DateTime.Now}");
        Debug.Log($"Claim Date is {claimDate}");
        Debug.Log($"Difference in hours is: {(claimDate - DateTime.Now).TotalHours}");
        var difference = (claimDate - DateTime.Now).TotalHours;
        if(difference < hoursInDay && difference > 0)
        {
            OnDailyRewardEvent?.Invoke();
            SetClaimDate(claimDate.AddDays(1));

            //if week is being completed
            if (consecutiveCount > maxDays - 1)
                consecutiveCount = 0;
            SetConsecutiveInfo();
        }
        else if ((claimDate - DateTime.Now).TotalHours < 0)
        {
            OnDailyRewardEvent?.Invoke();
            Debug.Log("More than a day");
            consecutiveCount = 0;
            SetConsecutiveInfo();
            SetClaimDate(DateTime.Now.AddDays(1));
        }
    }
    void RewardPlayer()
    {
        DailyRewardData dailyReward = rewards[consecutiveCount];
        switch (dailyReward.rewardType)
        {
            case RewardType.Coin:
                CurrencyController.ChangeCoins(dailyReward.rewardAmount);
                break;
            case RewardType.Diamond:
                CurrencyController.ChangeDiamonds(dailyReward.rewardAmount);
                break;
        }

        Debug.Log($"Added {dailyReward.rewardAmount} {dailyReward.rewardType}");
    }
    void CollectReward()
    {
        RewardPlayer();
        consecutiveCount++;
        SetConsecutiveInfo();
        OnRewardComplete?.Invoke();
    }
}