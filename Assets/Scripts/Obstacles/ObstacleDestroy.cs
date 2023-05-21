using UnityEngine;
public class ObstacleDestroy : MonoBehaviour
{
    SpriteRenderer renderer;
    EdgeCollider2D collider;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        if (transform.position.x < -16 || transform.position.x > 20)
        {
            renderer.enabled = false;
            collider.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            collider.enabled = true;
        }
    }
}
