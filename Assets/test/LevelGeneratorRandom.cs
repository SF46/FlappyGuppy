using UnityEngine;

public class LevelGeneratorRandom : MonoBehaviour
{
    public GameObject finishLinePrefab;
    public GameObject playerPrefab;
    public GameObject borders;

    [ContextMenu("Make")]
    public void MakeLevel(LevelSettings levelSettings)
    {
        Instantiate(playerPrefab, new Vector3(-5f, 0f, 0f), Quaternion.identity);
        Instantiate(levelSettings.backGroundPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Instantiate(borders, new Vector3(0f, 0f, 0f), Quaternion.identity);

        int numbypassObstacleTypes = levelSettings.bypassObstaclePrefab.Count;
        int numbombObstacleTypes = levelSettings.bombObstaclePrefab.Count;

        int numBypassObstaclesPlaced = 0;
        int numBombObstaclesPlaced = 0;

        float xPos = 0f;
        float lastObstacleXPos = 0f;
        int totalObstaclesPlaced = 0;
        int numBypassObstaclesBetweenBombs = Random.Range(levelSettings.minBypassObstacles, levelSettings.maxBypassObstacles + 1);

        while (numBypassObstaclesPlaced < levelSettings.numBypassObstacles || numBombObstaclesPlaced < levelSettings.numBombObstacles)
        {
            float yPos = Random.Range(levelSettings.obstacleYposMin, levelSettings.obstacleYposMax);
            GameObject obstaclePrefab;

            if (numBypassObstaclesPlaced >= levelSettings.numBypassObstacles)
            {
                obstaclePrefab = levelSettings.bombObstaclePrefab[Random.Range(0, numbombObstacleTypes)];
                numBombObstaclesPlaced++;
                numBypassObstaclesBetweenBombs = Random.Range(levelSettings.minBypassObstacles, levelSettings.maxBypassObstacles + 1);
            }
            else if (numBombObstaclesPlaced >= levelSettings.numBombObstacles)
            {
                obstaclePrefab = levelSettings.bypassObstaclePrefab[Random.Range(0, numbypassObstacleTypes)];
                numBypassObstaclesPlaced++;
            }
            else
            {
                if (numBypassObstaclesBetweenBombs == 0)
                {
                    obstaclePrefab = levelSettings.bombObstaclePrefab[Random.Range(0, numbombObstacleTypes)];
                    numBombObstaclesPlaced++;
                    numBypassObstaclesBetweenBombs = Random.Range(levelSettings.minBypassObstacles, levelSettings.maxBypassObstacles + 1);
                }
                else
                {
                    obstaclePrefab = levelSettings.bypassObstaclePrefab[Random.Range(0, numbypassObstacleTypes)];
                    numBypassObstaclesPlaced++;
                    numBypassObstaclesBetweenBombs--;
                }
            }

            float obstacleDistance = Random.Range(levelSettings.minDistance, levelSettings.maxDistance);
            xPos += obstacleDistance;
            Instantiate(obstaclePrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity, transform);

            lastObstacleXPos = xPos;
            totalObstaclesPlaced++;
        }

        Instantiate(finishLinePrefab, new Vector3(lastObstacleXPos + 5f, 0f, 0f), Quaternion.identity, transform);
    }
}