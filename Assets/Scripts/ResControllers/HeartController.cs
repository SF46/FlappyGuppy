using System.Collections;
using TMPro;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public const string HEART_COUNT_KEY = "HeartCount";
    public static int heartCount;
    public static readonly int maxHeartCount = 20;
    public GameObject ResInfo;
    public GameObject BuyInfo;

    readonly int heartToDiamondCoef = 2;
    int countToFill;
    int requiredDiamonds;

    public static void Start()
    {
        GetHeart();
    }

    public static void GetHeart()
    {
        if(!PlayerPrefs.HasKey(HEART_COUNT_KEY))
        {
            heartCount = 20;
            PlayerPrefs.SetInt(HEART_COUNT_KEY, heartCount);
        }
        else
            heartCount = PlayerPrefs.GetInt(HEART_COUNT_KEY);
    }
    public static void SetHeart()
    {
        PlayerPrefs.SetInt(HEART_COUNT_KEY, heartCount);
    }
    public static void RemoveHeart()
    {
        --heartCount;
        SetHeart();
    }

    public static void AddHeart(int value)
    {
        if (heartCount + value <= maxHeartCount)
        {
            heartCount += value;
            SetHeart();
        }
        else
        {
            heartCount = maxHeartCount;
            SetHeart();
            Debug.Log("Max heart count reached");
        }
    }


    public void Buy1()
    {
        if (CurrencyController.Diamonds >= 2)
        {
            if (heartCount + 1 <= maxHeartCount)
            {
                heartCount += 1;
                CurrencyController.ChangeDiamonds(-2);
                SetHeart();
            }
            else
            {
                Debug.Log("Max heart count reached");
            }
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
            Debug.Log("Not enough Coins");
        }
    }

    public void RefillInfo()
    {
        if (heartCount != maxHeartCount)
        {
            countToFill = maxHeartCount - heartCount;
            requiredDiamonds = countToFill * heartToDiamondCoef;
            BuyInfo.GetComponentInChildren<TextMeshProUGUI>().text = $"Do you want to buy {countToFill} hearts for {requiredDiamonds} diamonds?";
            BuyInfo.SetActive(true);
        }
        else
            Debug.Log("Max heart count reached");
    }
    public void Close()
    {
        BuyInfo.SetActive(false);
    }

    public void RefillHearts()
    {
        if (CurrencyController.Diamonds >= requiredDiamonds)
        {
            heartCount = maxHeartCount;
            CurrencyController.ChangeDiamonds(-requiredDiamonds);
            SetHeart();
        }
        else
        {
            Debug.Log(CurrencyController.Diamonds + " : " + requiredDiamonds);
            StartCoroutine(nameof(ShowInfo));
        }
        BuyInfo.SetActive(false);
    }

    IEnumerator ShowInfo()
    {
        ResInfo.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ResInfo.SetActive(false);
    }
}
