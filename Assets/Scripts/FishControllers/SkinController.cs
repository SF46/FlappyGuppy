using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    public const string CURRENT_SKIN_KEY = "CurrentSkinIndex";
    [SerializeField] GameObject skinsPanel;
    [SerializeField] GameObject skinPrefab;
    [SerializeField] Image selectedSkin;

    private readonly List<Skin> fishList = new();

    private int currentSkinIndex;

    public void Init()
    {
        SkinSaveManager.Init();
        LoadSkins();
    }
    private void LoadSkins()
    {
        var skinCount = SkinSaveManager.GetSkinCount();

        for (int i = 0; i < skinCount; i++)
        {
            var skin = Instantiate(skinPrefab, skinsPanel.transform);
            skin.name = $"Skin{i}";
            var skinComponent = skin.GetComponent<Skin>();
            skinComponent.OnSkinSelectedEvent += OnSkinSelected;
            skinComponent.OnSkinBuyEvent += OnSkinBuy;
            skinComponent.GetSkinData();
            fishList.Add(skinComponent);
        }
        GetCurrentSkinIndex();
        OnSkinSelected(fishList[currentSkinIndex]);
    }

    private void GetCurrentSkinIndex()
    {
        if (!PlayerPrefs.HasKey(CURRENT_SKIN_KEY))
        {
            currentSkinIndex = 0;
            PlayerPrefs.SetInt(CURRENT_SKIN_KEY, currentSkinIndex);
        }
        else
        {
            currentSkinIndex = PlayerPrefs.GetInt(CURRENT_SKIN_KEY, 0);
        }
    }
    private void OnSkinBuy()
    {
        foreach(var skin in fishList)
        {
            skin.SetAttributes();
        }
    }

    private void OnSkinSelected(Skin skin)
    {
        currentSkinIndex = skin.GetSkinID();
        PlayerPrefs.SetInt(CURRENT_SKIN_KEY, currentSkinIndex);
        GameManager.instance.CurrentSkinColor = fishList[currentSkinIndex].GetSkinColor();
        selectedSkin.color = fishList[currentSkinIndex].GetSkinColor();
    }
}