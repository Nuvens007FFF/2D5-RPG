using UnityEngine;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public SkeletonAnimation skeletonAnimation;
    private Vector3 targetPosition;
    private Direction lastDirection;

    public enum CharacterState { Idle, Run, Attack };
    private CharacterState currentState = CharacterState.Idle;
    private CharacterState previousState;
    private bool isAttacking = false;

    public enum Direction { Front, Back, Side };

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");
        targetPosition = transform.position;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isAttacking) // Right mouse button clicked
        {
            SetNewTargetPosition();
            currentState = CharacterState.Run;
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking) // Left mouse button clicked
        {
            SetNewTargetPosition();
            currentState = CharacterState.Run;
            Attack();
        }

        if (currentState == CharacterState.Run)
        {
            if (!MoveCharacter()) currentState = CharacterState.Idle;
        }

        if (currentState != previousState)
        {
            HandleStateChanged();
        }

        previousState = currentState;
    }

    private void SetNewTargetPosition()
    {
        Vector3 newTargetPosition = GetMousePositionInWorldSpace();

        if (newTargetPosition != targetPosition)
        {
            Direction newDirection = DetermineDirection(newTargetPosition);

            targetPosition = newTargetPosition;
            lastDirection = newDirection;

            if (lastDirection == Direction.Side)
            {
                FlipCharacter();
            }

            if (currentState != CharacterState.Attack)
            {
                HandleStateChanged();
            }
        }
    }

    private Vector3 GetMousePositionInWorldSpace()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0f;
        return clickPosition;
    }

    private void FlipCharacter()
    {
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private bool MoveCharacter()
    {
        if ((targetPosition - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void Attack()
    {
        isAttacking = true;
        currentState = CharacterState.Attack;
    }

    private Direction DetermineDirection(Vector3 targetPosition)
    {
        if (Mathf.Abs(targetPosition.x - transform.position.x) > Mathf.Abs(targetPosition.y - transform.position.y))
        {
            return Direction.Side;
        }
        else
        {
            return targetPosition.y > transform.position.y ? Direction.Back : Direction.Front;
        }
    }

    private void HandleAnimationEnd(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name.Contains("attack"))
        {
            currentState = CharacterState.Idle;
            isAttacking = false;
        }
    }

    private void HandleStateChanged()
    {
        string stateName = null;
        bool loop = true;
        switch (currentState)
        {
            case CharacterState.Idle:
                stateName = lastDirection.ToString().ToLower() + "_idle";
                break;
            case CharacterState.Run:
                stateName = lastDirection.ToString().ToLower() + "_run";
                break;
            case CharacterState.Attack:
                stateName = lastDirection.ToString().ToLower() + "_attack";
                loop = false;
                break;
        }
        Debug.Log(stateName);
        skeletonAnimation.AnimationState.SetAnimation(0, stateName, loop);
    }
}
