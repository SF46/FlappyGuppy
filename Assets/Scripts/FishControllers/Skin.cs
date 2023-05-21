using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnSkinSelectedDelegate(Skin skin);
public delegate void OnSkinbuyDelegate();
public class Skin : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button ownedButton;
    [SerializeField] TextMeshProUGUI ownedText;
    [SerializeField] RawImage lockedImage;
    [SerializeField] Image skinImage;
    [SerializeField] Button imageButton;

    public event OnSkinSelectedDelegate OnSkinSelectedEvent;
    public event OnSkinbuyDelegate OnSkinBuyEvent;

    private Color fishColor;
    private int ID;
    private PriceType priceType;
    private int cost;
    private bool isPurchased;
    public GameObject coinImage;
    public GameObject diamondImage;

    private void OnEnable()
    {
        buyButton.onClick.AddListener(SkinInteract);
        imageButton.onClick.AddListener(SkinInteract);
        SetAttributes();
    }
    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(SkinInteract);
        imageButton.onClick.RemoveListener(SkinInteract);
    }

    public void GetSkinData()
    {
        string name = gameObject.name;
        name = Regex.Replace(name, "[^0-9]", "");
        ID = Convert.ToInt32(name);
        FishSkinData skinData = SkinSaveManager.LoadSkin(ID);
        isPurchased = skinData.Purchased;
        priceType = (PriceType)Enum.Parse(typeof(PriceType), skinData.PriceType);
        cost = skinData.Cost;
        ID = skinData.ID;
        UnityEngine.ColorUtility.TryParseHtmlString(skinData.Color, out fishColor);
        SetAttributes();
    }
    public void SetSkinData()
    {
        FishSkinData skindata = new()
        {
            ID = ID,
            Cost = cost,
            Color = "#" + fishColor.ToHexString(),
            PriceType = priceType.ToString(),
            Purchased = isPurchased
        };
        SkinSaveManager.SaveSkin(skindata, ID);
    }
    void SkinInteract()
    {
        if (!isPurchased)
        {
            switch (priceType)
            {
                case PriceType.Coin:
                    if (CurrencyController.Coins >= cost)
                        BuySkin();
                    break;
                case PriceType.Diamond:
                    if (CurrencyController.Diamonds >= cost)
                        BuySkin();
                    break;
            }
            OnSkinBuyEvent?.Invoke();

        }
        else
        {
            OnSkinSelected();
        }
    }
    public void SetAttributes()
    {
        skinImage.color = fishColor;
        if (!isPurchased)
        {
            buyButton.gameObject.SetActive(true);
            ownedButton.gameObject.SetActive(false);
            priceText.text = cost.ToString();
            buyButton.interactable = true;

            if (priceType == PriceType.Coin && cost <= CurrencyController.Coins)
            {
                lockedImage.gameObject.SetActive(false);
                priceText.color = Color.white;
            }
            else if (priceType == PriceType.Diamond && cost <= CurrencyController.Diamonds)
            {
                lockedImage.gameObject.SetActive(false);
                priceText.color = Color.white;
            }
            else
            {
                lockedImage.gameObject.SetActive(true);
                priceText.color = Color.red;
                buyButton.interactable = false;
            }
        }
        else
        {
            lockedImage.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            ownedButton.gameObject.SetActive(true);
            ownedText.text = "OWNED";
            ownedText.color = new Color(0, 0.29f, 0.75f, 1f);
        }
    }
    void Start()
    {
        if (priceType == PriceType.Coin) 
        {
            diamondImage.SetActive(false);
            coinImage.SetActive(true); 
        }

        else if (priceType == PriceType.Diamond)
        {
            diamondImage.SetActive(true);
            coinImage.SetActive(false);
        }
        
    }
    void BuySkin()
    {
        isPurchased = true;
        if (priceType == PriceType.Coin)
            CurrencyController.ChangeCoins(-cost);
        else
            CurrencyController.ChangeDiamonds(-cost);
        cost = 0;
        SetSkinData();
        SetAttributes();
        SkinInteract();
    }
    void OnSkinSelected()
    {
        OnSkinSelectedEvent?.Invoke(this);
    }
    public Color GetSkinColor()
    {
        return fishColor;
    }
    public int GetSkinID()
    {
        return ID;
    }
}
