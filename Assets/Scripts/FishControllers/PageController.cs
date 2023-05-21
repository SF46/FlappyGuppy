using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    [Header("Pages")]
    public GameObject pausePage;
    public GameObject loosePage;
    public GameObject winPage;

    [Header("Buttons")]
    public GameObject fireButton;
    public GameObject jumpButton;
    public GameObject pauseButton;

    [Header("Texts")]
    public Text heartText;
    public Text bulletCountText;
    public Text levelName;

    //[Header("Word List")]
    //public TextMeshProUGUI[] Words;
    //private void Start()
    //{
    //    UpdateWords();
    //}
    //private void UpdateWords()
    //{
    //    WordController.GetWords(ref Words);
    //}
    public void PuaseMenu()
    {
        pausePage.SetActive(true);
        jumpButton.GetComponent<Button>().enabled = false;
        fireButton.GetComponent<Button>().enabled = false;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        jumpButton.GetComponent<Button>().enabled = true;
        fireButton.GetComponent<Button>().enabled = true;
        pausePage.SetActive(false);
        Time.timeScale = 1;
    }
    public void Main()
    {
        GameManager.instance.LoadMainScene();
        Time.timeScale = 1;
    }
    public void ReplayGame()
    {
        if (HeartController.heartCount > 0)
        {
            SceneManager.LoadScene(2);
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        if (HeartController.heartCount > 0)
            SceneManager.LoadScene(2);
    }
    public void NextLevel()
    {
        GameManager.instance.LoadGameScene();
    }
}