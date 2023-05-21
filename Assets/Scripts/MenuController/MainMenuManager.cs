using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //public const string LEVEL_INDEX_KEY = "LevelIndex";
    public const string MUSIC_INFO_KEY = "Music";
    public const string VIBRATE_INFO_KEY = "Vibrate";
    public const string RATE_VALUE_KEY = "RateValue";
    public const string RATE_COUNT_KEY = "RateCount";
    public const string REACHED_LEVEL_KEY = "ReachedLevel";


    private const string LEVEL_SETTINGS_PATH = "LevelSettings/";

    [Header("Pages")]
    [SerializeField] GameObject _main;
    [SerializeField] GameObject _settings;
    [SerializeField] GameObject _levels;
    [SerializeField] GameObject _characterCustomization;
    [SerializeField] GameObject _buyCoins;
    [SerializeField] GameObject _rewardSystemPage;
    [SerializeField] GameObject _farmMenu;

    [SerializeField] Button farmButton;

    [Header("Resource Texts")]
    public TextMeshProUGUI _coinText;
    public TextMeshProUGUI _diamondText;
    public TextMeshProUGUI _bulletText;
    public TextMeshProUGUI _heartText;
    public TextMeshProUGUI timerText;

    [Header("Music and Vibrate")]
    public Toggle _music;
    public Toggle _vibrate;

    [Header("Settings Page Objects")]
    public GameObject _musicOn;
    public GameObject _musicOff;
    public GameObject _vibrateOn;
    public GameObject _vibrateOff;
    public GameObject _ratePage;

    public int _coins = 1000;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;


    private int _levelCount;
    private int _maxLevel;
    private LevelSettings[] allLevelSettings;
    private readonly List<GameObject> _availableLevels = new();
    private readonly List<GameObject> _unAvailableLevels = new();

    [Header("Level Sprites")]
    public Texture2D LockedLevel;
    public Texture2D UnlockedLevel;

    private static int countToShowRatePage = 0;

    [Header("Rate Page Objects")]
    private int isRated = 0;
    public Texture2D starFull;
    public Texture2D starEmpty;
    public GameObject[] stars;


    public GameObject test_levelPrefab;
    public GameObject test_levelPanel;

    [SerializeField] DailyRewardSystem dailyRewardSystem;
    [SerializeField] GoldFactory goldFactory;
    [SerializeField] SkinController skinController;

    private void OnEnable()
    {
        dailyRewardSystem.OnDailyRewardEvent += ActivateDailyRewardSystemPage;
        dailyRewardSystem.OnRewardComplete += OnRewardCompleted;
        farmButton.onClick.AddListener(OnFarmActivateButton);
        goldFactory.OnBackMenu += OnBackFromFarm;
    }

    private void OnDisable()
    {
        dailyRewardSystem.OnDailyRewardEvent -= ActivateDailyRewardSystemPage;
        dailyRewardSystem.OnRewardComplete -= OnRewardCompleted;
        farmButton.onClick.RemoveListener(OnFarmActivateButton);
        goldFactory.OnBackMenu -= OnBackFromFarm;
    }

    private void OnFarmActivateButton()
    {
        _farmMenu.SetActive(true);
        _main.SetActive(false);
    }
    private void OnBackFromFarm()
    {
        _farmMenu.SetActive(false);
        _main.SetActive(true);
    }
    private void ActivateDailyRewardSystemPage()
    {
        if (_ratePage.activeInHierarchy)
        {
            isRwardPageWasActive = true;
        }
        else
        {
            _rewardSystemPage.SetActive(true);
            _main.SetActive(false);
        }
    }
    private void OnRewardCompleted()
    {
        _rewardSystemPage.SetActive(false);
        _main.SetActive(true);
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        LoadSettings();
        ShowCurrencyValues();
        GetLevelSettings();
        skinController.Init();
        CheckRate();
    }
    private void Update()
    {
        ShowCurrencyValues();
    }

    #region Levels
    void GetLevelSettings()
    {
        allLevelSettings = Resources.LoadAll<LevelSettings>(LEVEL_SETTINGS_PATH).OrderBy(x => x.name.Length).ThenBy(x => x.name).ToArray();
        GameManager.instance.LevelSettings = allLevelSettings;
        var reachedLevel = PlayerPrefs.GetInt(REACHED_LEVEL_KEY, 1);
        var levelCount = allLevelSettings.Length;
        GameManager.instance.CurrentLevel = reachedLevel;
        GameManager.instance.CurrentLevelSettings = allLevelSettings[reachedLevel - 1];
        GameManager.instance.LevelCount = levelCount;
    }
    public void GetAvailableLevels()
    {
        _levelCount = GameManager.instance.LevelCount;
        _maxLevel = GameManager.instance.CurrentLevel;
        var rt = test_levelPanel.GetComponent<RectTransform>();
        if (_levelCount > 12)
            rt.offsetMax = new Vector2(350 * (int)Math.Ceiling((float)_levelCount / 2 - 6), rt.offsetMax.y);
        for (int i = 0; i < _maxLevel; ++i)
        {
            GameObject go = Instantiate(test_levelPrefab, test_levelPanel.transform);
            go.GetComponent<Button>().onClick.AddListener(StartLevel);
            _availableLevels.Add(go);
            _availableLevels[i].GetComponent<RawImage>().texture = UnlockedLevel;
            _availableLevels[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
        for (int i = _maxLevel; i < _levelCount; ++i)
        {
            GameObject go = Instantiate(test_levelPrefab, test_levelPanel.transform);
            _unAvailableLevels.Add(go);
            _unAvailableLevels[i - _maxLevel].GetComponent<RawImage>().texture = LockedLevel;
            _unAvailableLevels[i - _maxLevel].GetComponent<Button>().enabled = false;
            _unAvailableLevels[i - _maxLevel].GetComponentInChildren<Text>().gameObject.SetActive(false);
        }
    }

    public void StartLevel()
    {
        if (HeartController.heartCount > 0)
        {
            var index = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
            GameManager.instance.CurrentLevelSettings = allLevelSettings[index - 1];
            GameManager.instance.LoadGameScene();
        }
    }
    public void ClearLevelsPanel()
    {
        _availableLevels.Clear();
        _unAvailableLevels.Clear();
        int childCount = test_levelPanel.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Destroy(test_levelPanel.transform.GetChild(i).gameObject);
        }
    }
    public void SelectLevel()
    {
        _levels.SetActive(true);
        _main.SetActive(false);
        GetAvailableLevels();
    }
    public void BackFromLevels()
    {
        ClearLevelsPanel();
        _levels.SetActive(false);
        _main.SetActive(true);
    }
    #endregion
    public void ShowCurrencyValues()
    {
        _coinText.text = CurrencyController.Coins.ToString();

        _diamondText.text = CurrencyController.Diamonds.ToString();

        _bulletText.text = BulletController.bulletCount.ToString();

        if (HeartController.heartCount == HeartController.maxHeartCount)
        {
            _heartText.text = "MAX";
            timerText.text = "FULL";
        }
        else
        {
            _heartText.text = HeartController.heartCount.ToString();
            timerText.text = Timer.Instance.UpdateTimer();
        }
    }

    private void CheckRate()
    {
        isRated = PlayerPrefs.GetInt(RATE_VALUE_KEY);
        if (isRated == 0)
        {
            GetSetRateCount();
            if (countToShowRatePage == 5)
                LoadRatePage();
        }
    }
    private void GetSetRateCount()
    {
        countToShowRatePage = PlayerPrefs.GetInt(RATE_COUNT_KEY);
        ++countToShowRatePage;
        PlayerPrefs.SetInt(RATE_COUNT_KEY, countToShowRatePage);
    }
    private bool isRwardPageWasActive = false;
    public void LoadRatePage()
    {
        CheckRate();
        _ratePage.SetActive(true);
        _main.SetActive(false);
    }
    public void Star()
    {
        int starCount = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
        for (int i = 0; i < starCount; ++i)
            stars[i].GetComponent<RawImage>().texture = starFull;
        for (int i = starCount; i < 5; ++i)
            stars[i].GetComponent<RawImage>().texture = starEmpty;
    }
    public void Later()
    {
        countToShowRatePage = 0;
        PlayerPrefs.SetInt(RATE_COUNT_KEY, 0);
        _ratePage.SetActive(false);
        if (isRwardPageWasActive)
            _rewardSystemPage.SetActive(true);
        else
            _main.SetActive(true);
    }
    public void Rate()
    {
        isRated = 1;
        PlayerPrefs.SetInt(RATE_VALUE_KEY, 1);
        _ratePage.SetActive(false);
        if (isRwardPageWasActive)
            _rewardSystemPage.SetActive(true);
        else
            _main.SetActive(true);
    }

    public void ShopMenu()
    {
        CurrencyController.Start();
        _buyCoins.SetActive(true);
        _main.SetActive(false);
    }
    public void BackMainFromBuyShop()
    {
        _buyCoins.SetActive(false);
        _main.SetActive(true);
    }

    public void CharacterCustomization()
    {
        _characterCustomization.SetActive(true);
        _main.SetActive(false);
    }
    public void BackFromCharacter()
    {
        _characterCustomization.SetActive(false);
        _main.SetActive(true);
    }

    public void ResumeGame()
    {
        if (HeartController.heartCount > 0)
        {
            GameManager.instance.CurrentLevelSettings = allLevelSettings[GameManager.instance.CurrentLevel - 1];
            GameManager.instance.LoadGameScene();
        }
    }

    public void Settings()
    {
        LoadSettings();
        _settings.SetActive(true);
        _main.SetActive(false);
    }
    public void BackMenuFromSettings()
    {
        SaveSettings();
        _settings.SetActive(false);
        _main.SetActive(true);
    }

    public void MusicOnOff()
    {
        if (_music.isOn)
        {
            audioMixer.SetFloat("volume", 0f);
            _musicOn.SetActive(true);
            _musicOff.SetActive(false);
        }
        else
        {
            audioMixer.SetFloat("volume", -80f);
            _musicOff.SetActive(true);
            _musicOn.SetActive(false);
        }
    }
    public void VibrateOnOff()
    {
        if (_vibrate.isOn)
        {
            _vibrateOn.SetActive(true);
            _vibrateOff.SetActive(false);
        }
        else
        {
            _vibrateOff.SetActive(true);
            _vibrateOn.SetActive(false);
        }
    }

    public void LoadSettings()
    {
        LoadMusicSettings();
        LoadVibrateSettings();
    }
    void UpdateSettings()
    {
        UpdateMusicSettings();
        UpdateVibrateSettings();
    }
    void LoadMusicSettings()
    {
        var value = PlayerPrefs.GetInt(MUSIC_INFO_KEY);
        if (value == 0)
            _music.isOn = false;
        else
            _music.isOn = true;
        MusicOnOff();
    }
    void LoadVibrateSettings()
    {
        var value = PlayerPrefs.GetInt(VIBRATE_INFO_KEY);
        if (value == 0)
            _vibrate.isOn = false;
        else
            _vibrate.isOn = true;
        VibrateOnOff();
    }
    void UpdateMusicSettings()
    {
        if (_music.isOn)
            PlayerPrefs.SetInt(MUSIC_INFO_KEY, 1);
        else
            PlayerPrefs.SetInt(MUSIC_INFO_KEY, 0);
    }
    void UpdateVibrateSettings()
    {
        if (_vibrate.isOn)
            PlayerPrefs.SetInt(VIBRATE_INFO_KEY, 1);
        else
            PlayerPrefs.SetInt(VIBRATE_INFO_KEY, 0);
    }

    public void SaveSettings()
    {
        UpdateSettings();
    }
}