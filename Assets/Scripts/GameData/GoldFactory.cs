using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnBackToMainMenuDelegate();
public class GoldFactory : MonoBehaviour
{
    public const string GOLD_FACTORY_TIMER_VALUE = "FactoryTimerValue";
    public const string GOLD_FACTORY_LEVEL = "FactoryLevel";
    [SerializeField] GoldFactorySettings[] factorySettings;
    [SerializeField] Button backButton;
    [SerializeField] Button collectButton;
    [SerializeField] Button upgradeButton;

    [SerializeField] TextMeshProUGUI incomeCount;
    [SerializeField] TextMeshProUGUI upgradeValue;
    [SerializeField] TextMeshProUGUI storageValue;
    [SerializeField] TextMeshProUGUI storageMaxValue;
    GoldFactorySettings currentFactorySettings;
    int currentLevel;
    int currentCollectedCoins = 0;
    int timerValue;
    public event OnBackToMainMenuDelegate OnBackMenu;

    Coroutine farmCoroutine = null;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackToMenu);
        upgradeButton.onClick.AddListener(BuyNewLevel);
        collectButton.onClick.AddListener(CollectFarm);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackToMenu);
        upgradeButton.onClick.RemoveListener(BuyNewLevel);
        collectButton.onClick.RemoveListener(CollectFarm);
    }

    private void Start()
    {
        GetCurrentFactoryLevel();
        currentFactorySettings = factorySettings[currentLevel];
        GetTimerValue();
        StartFarming();
        SetAttributes();
    }

    void GetTimerValue()
    {
        if (PlayerPrefs.HasKey(GOLD_FACTORY_TIMER_VALUE))
        {
            timerValue = PlayerPrefs.GetInt(GOLD_FACTORY_TIMER_VALUE);
            timerValue -= (int)GetRealTime.instance.difference.TotalSeconds;
            if (timerValue <= 0)
                timerValue = 0;
            currentCollectedCoins = currentFactorySettings.CurrentStorage - (currentFactorySettings.CoinPerHour * timerValue / 3600);
        }
        else
        {
            timerValue = currentFactorySettings.CurrentStorage * 3600 / currentFactorySettings.CoinPerHour;
            PlayerPrefs.SetInt(GOLD_FACTORY_TIMER_VALUE, timerValue);
        }
    }

    void SetTimerValue()
    {
        var collectedTimerValue = currentCollectedCoins * 3600 / currentFactorySettings.CoinPerHour;
        timerValue -= collectedTimerValue;
        PlayerPrefs.SetInt(GOLD_FACTORY_TIMER_VALUE, timerValue);
    }

    private void OnApplicationQuit()
    {
        SetTimerValue();
    }
    void SetAttributes()
    {
        incomeCount.text = currentFactorySettings.CoinPerHour.ToString();
        if (currentLevel == factorySettings.Length - 1)
        {
            Debug.Log("MaxLevelReached");
            upgradeValue.text = "Max";
        }
        else
            upgradeValue.text = factorySettings[currentLevel + 1].LevelCost.ToString();
        storageMaxValue.text = currentFactorySettings.CurrentStorage.ToString();
        storageValue.text = currentCollectedCoins.ToString();

    }
    void OnBackToMenu()
    {
        OnBackMenu?.Invoke();
    }
    void GetCurrentFactoryLevel()
    {
        currentLevel = PlayerPrefs.GetInt(GOLD_FACTORY_LEVEL);
    }
    void SetCurrentFactoryLevel()
    {
        PlayerPrefs.SetInt(GOLD_FACTORY_LEVEL, currentLevel);
    }
    void BuyNewLevel()
    {
        if (TryToBuy())
        {
            CollectFarm();
            currentFactorySettings = factorySettings[++currentLevel];
            CurrencyController.ChangeDiamonds(-currentFactorySettings.LevelCost);
            currentFactorySettings.IsPurchased = true;
            SetCurrentFactoryLevel();
            SetAttributes();
            UpdateTimer();
        }
    }
    public bool TryToBuy()
    {
        if (currentLevel + 1 < factorySettings.Length && CurrencyController.Diamonds < factorySettings[currentLevel + 1].LevelCost)
            return false;
        if (currentLevel + 1 >= factorySettings.Length)
            return false;
        return true;
    }
    void StartFarming()
    {
        farmCoroutine = StartCoroutine(GoldFarm());
    }
    IEnumerator GoldFarm()
    {
        while (currentCollectedCoins < currentFactorySettings.CurrentStorage)
        {
            yield return new WaitForSeconds(1f);
            timerValue--;
            yield return new WaitForSeconds(59);
            currentCollectedCoins += currentFactorySettings.CoinPerHour * 60 / 3600;
            SetAttributes();
            Debug.Log($"{currentCollectedCoins} / {currentFactorySettings.CurrentStorage}");
        }
    }
    void UpdateTimer()
    {
        timerValue = currentFactorySettings.CurrentStorage * 3600 / currentFactorySettings.CoinPerHour;
    }
    private void CollectFarm()
    {
        CurrencyController.ChangeCoins(currentCollectedCoins);
        currentCollectedCoins = 0;
        if (farmCoroutine == null)
        {
            farmCoroutine = StartCoroutine(GoldFarm());
        }
        else
        {
            StopCoroutine(farmCoroutine);
            StartFarming();
        }
        SetAttributes();
        UpdateTimer();
    }
}