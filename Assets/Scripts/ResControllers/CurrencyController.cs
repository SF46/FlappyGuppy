using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public const string COIN_COUNT_KEY = "CoinCount";
    public const string DIAMOND_COUNT_KEY = "DiamondCount";
    public static int Coins;
    public static int Diamonds;

    public static void Start()
    {
        LoadCoins();
        LoadDiamonds();
    }

    public void D_Buy5()
    {
        Diamonds += 5;
        SaveDiamonds();
    }
    public void D_Buy17()
    {
        Diamonds += 17;
        SaveDiamonds();
    }
    public void D_Buy30()
    {
        Diamonds += 30;
        SaveDiamonds();
    }
    public void D_Buy80()
    {
        Diamonds += 80;
        SaveDiamonds();
    }
    public void D_Buy200()
    {
        Diamonds += 200;
        SaveDiamonds();
    }
    public void D_Buy450()
    {
        Diamonds += 450;
        SaveDiamonds();
    }

    public void C_Buy500()
    {
        if (Diamonds >= 25)
        {
            Coins += 500;
            ChangeDiamonds(-25);
            SaveCoins();
        }
    }
    public void C_Buy1500()
    {
        if (Diamonds >= 50)
        {
            Coins += 1500;
            ChangeDiamonds(-50);
            SaveCoins();
        }
    }
    public void C_Buy3200()
    {
        if (Diamonds >= 100)
        {
            Coins += 3200;
            ChangeDiamonds(-100);
            SaveCoins();
        }
    }
    public void C_Buy5000()
    {
        if (Diamonds >= 150)
        {
            Coins += 5000;
            ChangeDiamonds(-150);
            SaveCoins();
        }
    }

    public static void SaveCoins()
    {
        PlayerPrefs.SetInt(COIN_COUNT_KEY, Coins);
    }
    public static void LoadCoins()
    {
        if (!PlayerPrefs.HasKey(COIN_COUNT_KEY))
        {
            Coins = 0;
            PlayerPrefs.SetInt(COIN_COUNT_KEY, Coins);
        }
        else
            Coins = PlayerPrefs.GetInt(COIN_COUNT_KEY);
    }
    public static void ChangeCoins(int value)
    {
        Coins += value;
        SaveCoins();
    }

    public static void SaveDiamonds()
    {
        PlayerPrefs.SetInt(DIAMOND_COUNT_KEY, Diamonds);
    }
    public static void LoadDiamonds()
    {
        if (!PlayerPrefs.HasKey(DIAMOND_COUNT_KEY))
        {
            Diamonds = 20;
            PlayerPrefs.SetInt(DIAMOND_COUNT_KEY, Diamonds);
        }
        else
            Diamonds = PlayerPrefs.GetInt(DIAMOND_COUNT_KEY);
    }
    public static void ChangeDiamonds(int value)
    {
        Diamonds += value;
        SaveDiamonds();
    }
}