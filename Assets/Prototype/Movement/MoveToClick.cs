using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform targetTransform; // Target position to move towards
    public bool isMoving = false;

    private void Update()
    {
        // Get mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            targetTransform.position = mousePos;
            isMoving = true;
        }

        // Move towards target position
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);

            // Stop moving if close enough to target position
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}