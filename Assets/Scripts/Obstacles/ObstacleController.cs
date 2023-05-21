using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 3.0f;

    [Header("Obstacles and Models")]    
    public LevelGeneratorRandom levelGenerator;


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    void Start()
    {
        StartLevel(GameManager.instance.CurrentLevelSettings);
        StartCoroutine(Roller());
    }

    private IEnumerator Roller()
    {
        while (true)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.left);
            yield return null;
        }
    }

    void StartLevel(LevelSettings levelSettings)
    {
        levelGenerator.MakeLevel(levelSettings);
    }
}