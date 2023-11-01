using UnityEngine;
using Spine.Unity;
using System.Collections;
using CharacterEnums;

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

    public SwordController swordController;
    public GameObject frontPivot;
    public GameObject backPivot;
    public GameObject rightPivot;
    public GameObject defaultPivot;

    private float attackCooldown = 1f; // Cooldown period in seconds
    private float nextAttackTime = 0f; // Time when the next attack can be performed

    public SkillSO s;

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");
        if (swordController == null) Debug.LogError("swordController is not assigned!");
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

        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime) // Left mouse button clicked
        {
            SetNewTargetPosition();
            currentState = CharacterState.Run;
            Attack();

            // Set the time for the next attack
            nextAttackTime = Time.time + attackCooldown;
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

        if(transform.position == s.image.transform.position)
        {
            //Nếu player chạm vào vùng nước xoáy, thì bị hất tung
        }
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

        // Start the swing sword coroutine
        StartCoroutine(swordController.SwingSword(lastDirection, frontPivot, backPivot, rightPivot, defaultPivot));

        // Start the attack timeout coroutine
        StartCoroutine(AttackTimeout());
    }


    private IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(0.4f); // Adjust to match the actual duration of your attack animation
        if (currentState == CharacterState.Attack)
        {
            Debug.LogWarning("Attack animation did not complete. Reverting to Idle state.");
            isAttacking = false;
            currentState = CharacterState.Idle;
        }
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
                // Check if the new direction is the same as the last direction
                // and if the character is currently running
                if (skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name == stateName)
                {
                    return; // Ignore setting the new animation
                }
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