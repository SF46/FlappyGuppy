using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    public float speed = 0.2f;

    void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
