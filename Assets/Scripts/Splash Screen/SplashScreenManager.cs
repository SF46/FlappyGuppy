using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public TextMeshProUGUI LoadRes;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        StartCoroutine(WaitingDelay());
    }

    private IEnumerator WaitingDelay()
    {
        CurrencyController.Start();
        BulletController.Start();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(1);
    }
}