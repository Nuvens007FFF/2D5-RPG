using UnityEngine;
using Spine.Unity;
using System.Collections;
using CharacterEnums;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 5f;
    public SkeletonAnimation skeletonAnimation;
    private Vector3 targetPosition;
    private Direction lastDirection;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;
    public Transform playerTransform;

    public enum CharacterState { Idle, Run, Attack };
    private CharacterState currentState = CharacterState.Idle;
    private CharacterState previousState;
    private int bossAttack;
    public float turnCooldown = 20f;
    private float nextTurnTime = 0f;

    public BossAttackController leftClawController;
    public BossAttackController rightClawController;
    public BossAttackController tailController;
    public GameObject leftClaw;
    public GameObject rightClaw;
    public GameObject tail;
    public GameObject attackPoint;

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");

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

        targetPosition = transform.position;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;
    }

    private void Update()
    {
        //Debug.Log(isAttacking + " " + (IsPlayerInRange() + " " + (Time.time >= nextAttackTime)));
        if (!isAttacking)
        {
            ChasePlayer();
        }

        if ((IsPlayerInRange()) && !isAttacking && Time.time >= nextAttackTime)
        {
            currentState = CharacterState.Attack;
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }

        if (currentState != previousState)
        {
            HandleStateChanged();
        }
        previousState = currentState;
    }

    private void ChasePlayer()
    {
        targetPosition = playerTransform.position;
        Direction newDirection = DetermineDirection(targetPosition);
        lastDirection = newDirection;

        if (lastDirection == Direction.Side)
        {
            FlipCharacter();
        }
        if (Time.time >= nextTurnTime)
        {
            switch (lastDirection)
            {
                case Direction.Front:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
                    attackRange = 2.5f;
                    break;
                case Direction.Back:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, 180);
                    attackRange = 1.5f;
                    break;
                case Direction.Side:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 90 : -90);
                    attackRange = 1.75f;
                    break;
            }
            Debug.Log("Turn");
            nextTurnTime = Time.time + turnCooldown;
        }

        if (MoveCharacter())
        {
            currentState = CharacterState.Run;
        }
        else
        {
            currentState = CharacterState.Idle;
        }
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
        // Calculate the distance to the target position
        float distanceToTarget = Vector3.Distance(targetPosition, transform.position);

        // If the distance is greater than the attack range and greater than a small threshold
        if (distanceToTarget > attackRange && distanceToTarget > 0.001f)
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
        // Add your attack logic here

        switch (bossAttack)
        {
            case 1:
                StartCoroutine(leftClawController.SwingClaw(lastDirection));
                break;
            case 2:
                StartCoroutine(rightClawController.SwingClaw(lastDirection));
                break;
            case 3:
                StartCoroutine(tailController.SwingClaw(lastDirection));
                break;
            default:
                break;
        }

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

        //Debug.Log(stateName);
        skeletonAnimation.AnimationState.SetAnimation(0, stateName, loop);
    }

    private bool IsPlayerInRange()
    {
        bool inRange = false;
        bossAttack = 0;
        if ((Vector3.Distance(leftClaw.transform.position, playerTransform.position) - 0.3f) <= attackRange)
        {
            inRange = true;
            bossAttack = 1;
        }
        if ((Vector3.Distance(rightClaw.transform.position, playerTransform.position) - 0.3f) <= attackRange)
        {
            inRange = true;
            bossAttack = 2;
        }
        if ((Vector3.Distance(tail.transform.position, playerTransform.position) - 0.3f) <= attackRange)
        {
            inRange = true;
            bossAttack = 3;
        }
        return inRange;
    }
}