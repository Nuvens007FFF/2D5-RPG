using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Transform playerTransform;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        // Try to find the player GameObject when the script starts
        FindPlayer();
    }

    private void Update()
    {
        // If the playerTransform is still null, try to find the player GameObject again
        if (playerTransform == null)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
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

            // Use SmoothDamp to interpolate between current position and desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
    }
}
