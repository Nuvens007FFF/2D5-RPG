using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform targetTransform; // Target position to move towards
    public CharacterStateMachine stateMachine;
    public bool isMoving = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetTransform.position = mousePos;
            isMoving = true;
            stateMachine.ChangeState(CharacterState.Moving);
        }

        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, step);

        // Stop moving if close enough to target position
        if (Vector2.Distance(transform.position, targetTransform.position) < 0.1f)
        {
            isMoving = false;
            stateMachine.ChangeState(CharacterState.Idle);
        }
    }
}

