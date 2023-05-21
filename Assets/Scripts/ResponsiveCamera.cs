using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResponsiveCamera : MonoBehaviour
{
    private Camera cam;
    private float targetAspectRatio;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetAspectRatio = 16f / 9f; // Set your desired aspect ratio here (e.g., 16:9)
    }

    private void Start()
    {
        UpdateCameraAspect();
    }

    private void UpdateCameraAspect()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        float scaleHeight = currentAspectRatio / targetAspectRatio;

        // If the current aspect ratio is wider than the target aspect ratio, adjust the camera's viewport to maintain the desired width
        if (scaleHeight < 1f)
        {
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
            cam.rect = rect;
        }
        else // If the current aspect ratio is narrower than the target aspect ratio, adjust the camera's viewport to maintain the desired height
        {
            float scaleWidth = 1f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
            cam.rect = rect;
        }
    }
}
