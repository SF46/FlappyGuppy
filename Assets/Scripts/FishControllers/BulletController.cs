using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public const string BULLET_COUNT_KEY = "BulletCount";
    public static int bulletCount;
    public GameObject ResInfo;

    public static void Start()
    {
        GetBullet();
    }
    public void Buy1()
    {
        if (CurrencyController.Coins >= 50)
        {
            bulletCount += 1;
            CurrencyController.ChangeCoins(-50);
            SetBullet();
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
            Debug.Log("Not enough Coins");
        }
    }
    public void Buy5()
    {
        if (CurrencyController.Coins >= 250)
        {
            bulletCount += 5;
            CurrencyController.ChangeCoins(-250);
            SetBullet();
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
            Debug.Log("Not enough Coins");
        }
    }
    public void Buy10()
    {
        if (CurrencyController.Coins >= 500)
        {
            bulletCount += 10;
            CurrencyController.ChangeCoins(-500);
            SetBullet();
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
        }
    }
    public void Buy15()
    {
        if (CurrencyController.Coins >= 750)
        {
            bulletCount += 15;
            CurrencyController.ChangeCoins(-750);
            SetBullet();
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
        }
    }
    public void Buy20()
    {
        if (CurrencyController.Coins >= 1000)
        {
            bulletCount += 20;
            CurrencyController.ChangeCoins(-1000);
            SetBullet();
        }
        else
        {
            StartCoroutine(nameof(ShowInfo));
        }
    }
    public static void RemoveBullet()
    {
        --bulletCount;
        SetBullet();
    }
    public static void AddBullet(int value)
    {
        bulletCount += value;
        SetBullet();
    }
    public static void GetBullet()
    {
        if (!PlayerPrefs.HasKey(BULLET_COUNT_KEY))
        {
            bulletCount = 6;
            PlayerPrefs.SetInt(BULLET_COUNT_KEY, bulletCount);
        }
        else
            bulletCount = PlayerPrefs.GetInt(BULLET_COUNT_KEY);
    }
    public static void SetBullet()
    {
        PlayerPrefs.SetInt(BULLET_COUNT_KEY, bulletCount);
    }
    IEnumerator ShowInfo()
    {
        ResInfo.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ResInfo.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AudioManager.instance.Play("Hit");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}