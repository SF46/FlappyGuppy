using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetting", menuName = "Level/LevelSetting")]
public class LevelSettings : ScriptableObject
{
    public List<GameObject> bypassObstaclePrefab;
    public List<GameObject> bombObstaclePrefab;
    public int numBypassObstacles;
    public int numBombObstacles;
    public float minDistance;
    public float maxDistance;
    public int minBypassObstacles;
    public int maxBypassObstacles;
    public float obstacleYposMin;
    public float obstacleYposMax;
    public GameObject backGroundPrefab;
    public int levelReward;
    public bool levelFinished;
}