using UnityEngine;

[CreateAssetMenu(fileName = "Gold Factory Settings", menuName = "Settings/Gold Factory Settings")]
public class GoldFactorySettings : ScriptableObject
{
    public int Level;
    public int CoinPerHour;
    public int CurrentStorage;
    public int LevelCost;
    public bool IsPurchased;   
}
