using UnityEditor;
using UnityEngine;
public enum PriceType
{
    Coin,
    Diamond
}
[CreateAssetMenu(fileName = "FishSettings", menuName = "Fish/Fish Settings")]
public class FishSettings : ScriptableObject
{
    [SerializeField] PriceType priceType;
    [SerializeField] string color;
    [SerializeField] bool purchased;
    [SerializeField] int cost;

    public PriceType PriceType
    {
        get { return priceType; }
        set { priceType = value;
        }
    }
    public string Color
    {
        get { return color; }
        set { color = value; 
        }
    }
    public bool Purchased
    {
        get { return purchased; }
        set { purchased = value; 
        }
    }
    public int Cost
    {
        get { return cost; }
        set { cost = value; 
        }
    }
}