using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime = 0.3f;
    public Vector3 offset;

    // Set border size
    public float XborderSize = 15f;
    public float YborderSize = 15f;

    private Transform playerTransform;
    PlayerController playerController;
    private Vector3 velocity = Vector3.zero;
    private float initialWaitTime = 2.5f; // Adjust the initial delay as needed
    private float currentWaitTime = 0.1f;

    private void Start()
    {
        // Try to find the player GameObject when the script starts
        FindPlayer();
    }

    private void Update()
    {
        currentWaitTime += Time.deltaTime;
        // Check if the initial delay has passed
        if (currentWaitTime < initialWaitTime)
        {
            // Do nothing during the initial delay
            return;
        }

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
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }

        // Check if the player GameObject was found
        if (playerObject != null)
        {
            // Assign the transform of the player GameObject to playerTransform
            playerTransform = playerObject.transform;
        }
        else
        {
            // Log an error if the player GameObject was not found
            //Debug.LogError("Player GameObject not found in scene!");
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null && !playerController.isDied)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            // Keep the camera at its original z position
            desiredPosition.z = transform.position.z;

            // Use SmoothDamp to interpolate between current position and desired position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            // Calculate the minimum and maximum bounds
            Vector3 minBounds = new Vector3(-XborderSize / 2f, -YborderSize / 2f, transform.position.z);
            Vector3 maxBounds = new Vector3(XborderSize / 2f, YborderSize / 2f, transform.position.z);

            // Clamp the camera position within the specified bounds
            smoothedPosition = new Vector3(
                Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y),
                Mathf.Clamp(smoothedPosition.z, minBounds.z, maxBounds.z)
            );

            transform.position = smoothedPosition;
        }
    }
}
