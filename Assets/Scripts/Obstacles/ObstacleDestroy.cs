using UnityEngine;
public class ObstacleDestroy : MonoBehaviour
{
    SpriteRenderer renderer1;
    Collider2D collider1;
    private void Start()
    {
        renderer1 = GetComponent<SpriteRenderer>();
        collider1 = GetComponent<EdgeCollider2D>();
        if(collider1 == null)
        {
            collider1 = GetComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        if (transform.position.x < -16 || transform.position.x > 20)
        {
            renderer1.enabled = false;
            collider1.enabled = false;
        }
        else
        {
            renderer1.enabled = true;
            collider1.enabled = true;
        }
    }
}
