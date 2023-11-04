using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Transform playerTransform;

    private void Start()
    {
        // Find the player GameObject with the "Player" tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        // Check if the player GameObject was found
        if (playerObject != null)
        {
            // Assign the transform of the player GameObject to playerTransform
            playerTransform = playerObject.transform;
        }
        else
        {
            // Log an error if the player GameObject was not found
            Debug.LogError("Player GameObject not found in scene!");
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            // Keep the camera at its original z position
            desiredPosition.z = transform.position.z;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
