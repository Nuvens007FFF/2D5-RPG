using UnityEngine;

public class AutoAttachCamera : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        // If canvas is not assigned, try to find it in the current GameObject
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas not assigned and not found on the GameObject. Please assign the Canvas component.");
                return;
            }
        }

        // Attach the main camera to the canvas
        AttachMainCameraToCanvas(canvas);
    }

    void AttachMainCameraToCanvas(Canvas canvas)
    {
        // Find and assign the main camera as the world camera for the canvas
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            canvas.worldCamera = mainCamera;

            // Optionally, set the canvas's position and rotation based on the main camera
            canvas.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 10f; // Adjust the distance as needed
            canvas.transform.LookAt(mainCamera.transform);
        }
        else
        {
            Debug.LogError("Main camera not found. Ensure there is an active main camera in the scene.");
        }
    }
}