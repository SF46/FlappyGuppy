using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string CURRENT_FISH_COLOR = "CurrentFishColor";

    public static GameManager instance;
    private LevelSettings currentLevelSettings;
    private int currentLevel;
    private int levelCount;
    private LevelSettings[] levelsettings;
    private Color currentSkinColor;
    public Color CurrentSkinColor
    {
        get { return currentSkinColor; }
        set { currentSkinColor = value; }
    }

    public LevelSettings[] LevelSettings 
    { 
        set { levelsettings = value; } 
    }

    public int LevelCount 
    { 
        get { return levelCount; } 
        set { levelCount = value; } 
    }

    public int CurrentLevel 
    { 
        get { return currentLevel; } 
        set { currentLevel = value; } 
    }
    public LevelSettings CurrentLevelSettings 
    { 
        get { return currentLevelSettings; } 
        set { currentLevelSettings = value; } 
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        GetCurrentFishColor();
    }

    private void GetCurrentFishColor()
    {
        var fishColorString = "#" + PlayerPrefs.GetString(CURRENT_FISH_COLOR);
        if (ColorUtility.TryParseHtmlString(fishColorString, out currentSkinColor))
        {
            Debug.Log("Color parsing completed: " + currentSkinColor);
        }
        else
        {
            Debug.Log("Color parsing error: " + fishColorString);
        }
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }
    public void OnLevelFinished()
    {
        if (currentLevelSettings.levelFinished == false)
        {
            currentLevelSettings.levelFinished = true;
            CurrencyController.ChangeCoins(currentLevelSettings.levelReward);
            currentLevel++;
            PlayerPrefs.SetInt(MainMenuManager.REACHED_LEVEL_KEY, currentLevel);
            currentLevelSettings = levelsettings[currentLevel];
            Debug.Log("Now this level is finished");
        }
        else
        {
            Debug.Log("This level was finished");
            CurrencyController.ChangeCoins(50);
        }
    }
    private void OnApplicationQuit()
    {
        SetCurrentFishColor();
    }

    private void SetCurrentFishColor()
    {
        var fishColorString = ColorUtility.ToHtmlStringRGB(currentSkinColor);
        PlayerPrefs.SetString(CURRENT_FISH_COLOR, fishColorString);
    }
}
