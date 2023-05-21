using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

[Serializable]
public class FishSkinData
{
    public int ID { get; set; }
    public string PriceType { get; set; }
    public string Color { get; set; }
    public bool Purchased { get; set; }
    public int Cost { get; set; }

}
public class FishSkinDataWrapper
{
    public List<FishSkinData> FishSkins = new();
}
public static class SkinSaveManager
{
    public static List<FishSkinData> fishSkinData = new();
    public static string filename = "FishSkinData.json";
    private readonly static string filePath = Application.persistentDataPath + "/FishSkinData.json";

    public static void Init()
    {
        if (!File.Exists(filePath))
        {
            UnityWebRequest webRequest = new();
            if (Application.platform == RuntimePlatform.Android)
            {
                webRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + filename);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                webRequest = UnityWebRequest.Get(Application.dataPath + "/Raw/" + filename);
            }
            else
            {
                webRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + filename);
            }
            webRequest.SendWebRequest();
            while (!webRequest.isDone) { }
            File.WriteAllBytes(filePath, webRequest.downloadHandler.data);
        }
        ReadJSON();
    }

    private static void ReadJSON()
    {
        string jsonString = File.ReadAllText(filePath);
        var skinJson = JsonConvert.DeserializeObject<FishSkinDataWrapper>(jsonString);
        fishSkinData = skinJson.FishSkins;
        Debug.Log(jsonString);
    }
    public static void SaveSkin(FishSkinData skinData, int id)
    {
        fishSkinData[id] = skinData;
        SaveSkins();
    }
    //public static void Init()
    //{
    //    string fullPath;
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        fullPath = "jar:file://" + Application.dataPath + "!/assets/" + directory + filename;
    //        UnityWebRequest request = UnityWebRequest.Get(fullPath);
    //        request.SetRequestHeader("Content-Type", "application/json");
    //        while (!request.isDone) { }
    //        string jsonString = request.downloadHandler.text;
    //        var skinJson = JsonUtility.FromJson<FishSkinDataWrapper>(jsonString);
    //        fishSkinData = skinJson.FishSkins;
    //    }
    //    else if (Application.platform == RuntimePlatform.IPhonePlayer)
    //    {
    //        fullPath = Application.dataPath + "/Raw/" + directory + filename;
    //        string jsonString = File.ReadAllText(fullPath);
    //        var skinJson = JsonUtility.FromJson<FishSkinDataWrapper>(jsonString);
    //        fishSkinData = skinJson.FishSkins;
    //    }
    //    else
    //    {
    //        Debug.Log(Application.platform + " is not Android");
    //    }
    //    Debug.Log(Application.platform);

    //}
    public static int GetSkinCount()
    {
        return fishSkinData.Count;
    }
    private static void SaveSkins()
    {

        FishSkinDataWrapper skinDataWrapper = new()
        {
            FishSkins = fishSkinData
        };
        string json = JsonConvert.SerializeObject(skinDataWrapper, Formatting.Indented);
        Debug.Log("Save path is : " + filePath);
        File.WriteAllText(filePath, json);
    }
    public static FishSkinData LoadSkin(int id)
    {
        return fishSkinData[id];
    }
}
