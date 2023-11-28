using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using CharacterEnums;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 5f;
    public float skillCooldown = 10f;
    public float globalSkillCooldown = 10f;
    public float maxHP = 500f;
    private float currentHP;
    private float initialWaitTime = 3f; // Adjust the initial delay as needed
    private float currentWaitTime = 0f;
    private float lastPercentTage;
    public Image healthBarImage;
    public Image energyBarImage;

    public float CurrentHP
    {
        get { return currentHP; }
        private set { currentHP = value; }
    }

    public SkeletonAnimation skeletonAnimation;
    private Vector3 targetPosition;
    private Direction lastDirection;
    private bool isAttacking = false;
    private bool isSpecialAttack = false;
    private bool bossIntro = true;
    private float nextAttackTime = 0f;
    private float nextGlobalSkillTime = 0f;
    private float nextSkillTime = 0f;
    public Transform playerTransform;
    private GameObject player;
    public GameObject land;
    private PlayerController playerController;

    public enum CharacterState { Idle, Run, Attack };
    private CharacterState currentState = CharacterState.Idle;
    private CharacterState previousState;
    private int bossAttack;
    public float turnCooldown = 20f;
    private float nextTurnTime = 0f;
    private bool isPhase2 = false;
    private bool isPhase3 = false;

    public BossAttackController leftClawController;
    public BossAttackController rightClawController;
    public GameObject leftClaw;
    public GameObject rightClaw;
    public GameObject attackPoint;
    public BossViewRange viewRange;
    public GameObject chargeRange;
    public GameObject attackRangeCollider;

    public ParticleSystem hitParticleSystem;
    public static event Action DropCoinEvent;
    public GameObject BossRoar;
    public GameObject RainVFX;
    public GameObject Skill1;
    public GameObject indicatorSkill2;
    public GameObject WaterSplashSkill2;
    public GameObject chargeColliderSkill2;
    public GameObject Skill3;
    public GameObject Skill4;
    public GameObject Skill5;
    public GameObject Skill6;
    public GameObject Skill6Buff;
    public GameObject Skill6Spirit;

    public float normalAttackDamage = 5f;
    public float skill1Dmg = 10f;
    public float skill2Dmg = 50f;
    public float skill2CD = 10f;
    private float nextSkill2Time = 0f;
    public float skill3ATKSPD = 0.5f;
    public float skill3SPD = 5f;
    public float skill3Duration = 5f;
    public float skill3FadeOutTime = 4f;
    public float skill3Delay = 0.5f;
    private float skill3DelayCountdown = 0;
    public float skill4Duration = 3f;
    public float skill4Interval = 1f;
    public float skill5Duration = 3f;
    public float skill5PushForce = 10f;
    public float skill5ForceDuration = 0.25f;
    public float skill5Interval = 0.5f;
    public float skill6Energy = 100f;
    public float skill6MaxEnergy = 100f;
    public float skill6OrbEnergy = 2.5f;
    private float energyOrbSpawnTimer = 0f;
    public float energyOrbSpawnInterval = 1f;

    private int consecutiveSkill3Count = 0;
    private int consecutiveSkill4Count = 0;
    private int consecutiveSkill5Count = 0;

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");

        // Find the player GameObject with the "Player" tag
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        Skill3.SetActive(false);

        // Check if the player GameObject was found
        if (player != null)
        {
            // Assign the transform of the player GameObject to playerTransform
            playerTransform = player.transform;
        }
        else
        {
            // Log an error if the player GameObject was not found
            Debug.LogError("Player GameObject not found in scene!");
        }

        targetPosition = transform.position;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;

        // Initialize currentHP to maxHP
        currentHP = maxHP;
        lastPercentTage = currentHP / currentHP * 100f;

        // Initialize Status Bar
        GameObject healthBarObject = GameObject.Find("BossHPBar");
        healthBarImage = healthBarObject.GetComponent<Image>();
        GameObject energyBarObject = GameObject.Find("BossMPBar");
        energyBarImage = energyBarObject.GetComponent<Image>();
    }

    private void Update()
    {
        currentWaitTime += Time.deltaTime;
        skill3DelayCountdown += Time.deltaTime;
        if(playerController.isDied)
        {
            return;
        }

        if(bossIntro)
        {
            BossIntro();
            bossIntro = false;
        }

        // Check if the initial delay has passed
        if (currentWaitTime < initialWaitTime)
        {
            // Do nothing during the initial delay
            return;
        }

        if (skill3DelayCountdown < skill3Delay)
        {
            // Do nothing during the delay
            return;
        }

        UpdateHealthBar(currentHP, maxHP);

        if ((currentHP <= (maxHP * 0.7f)) && !isPhase2 && !isAttacking)
        {
            isPhase2 = true;
            globalSkillCooldown = globalSkillCooldown / 2f;
            skill2CD = skill2CD + 5f;
            UseSkill5();
            Debug.Log("Phase 2");
        }

        if ((currentHP <= (maxHP * 0.4f)) && !isPhase3 && !isAttacking)
        {
            isPhase3 = true;
            UseSkill6();
            Debug.Log("Phase 3");
        }

        skill6Energy = Mathf.Clamp(skill6Energy, 0f, skill6MaxEnergy);
        UpdateEnergyBar(skill6Energy, skill6MaxEnergy);
        if (skill6Energy >= skill6MaxEnergy && isPhase3 && !isAttacking && !isSpecialAttack)
        {
            UseSkill6();
        }

        // Check if it's time to spawn an energy orb
        energyOrbSpawnTimer += Time.deltaTime;
        if (energyOrbSpawnTimer >= energyOrbSpawnInterval && skill6Energy < skill6MaxEnergy)
        {
            // Call the method to spawn an energy orb
            SpawnEnergyOrb();
            energyOrbSpawnTimer = 0f; // Reset the timer
        }

        //Debug.Log(isAttacking + " " + (IsPlayerInRange() + " " + (Time.time >= nextAttackTime)));
        if (!isAttacking)
        {
            ChasePlayer();
        }

        if ((IsPlayerInRange()) && !isAttacking && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }

        if (Time.time >= nextGlobalSkillTime && !isSpecialAttack)
        {
            // Call the method to handle random skill selection
            UseRandomGlobalSkill();

            nextGlobalSkillTime = Time.time + globalSkillCooldown;
        }

        if (!IsPlayerInRange() && !isAttacking && Time.time >= nextSkillTime)
        {
            // Call the method to handle random skill selection
            UseRandomSkill();

            nextSkillTime = Time.time + skillCooldown;
        }

        if (IsPlayerInChargeRange() && !isAttacking && Time.time >= nextSkill2Time)
        {
            // Call the method to handle random skill selection
            UseSkill2();


            nextSkill2Time = Time.time + skill2CD + (UnityEngine.Random.Range(-5f, 5f));
        }

        if (currentState != previousState)
        {
            HandleStateChanged();
        }
        previousState = currentState;

        //Check Dead
        Dead();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Calculate the health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Update the health bar image fill amount
        healthBarImage.fillAmount = healthPercentage;
    }

    public void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        // Calculate the health percentage
        float energyPercentage = currentEnergy / maxEnergy;

        // Update the health bar image fill amount
        energyBarImage.fillAmount = energyPercentage;
    }

    private void BossIntro()
    {
        StartCoroutine(IntroRoar());
    }

    private IEnumerator IntroRoar()
    {
        yield return new WaitForSeconds(1f);
        GameObject Skill5VFX = Instantiate(Skill5, attackPoint.transform.position, attackPoint.transform.rotation);
        yield return new WaitForSeconds(2f);
        Destroy(Skill5VFX);
    }

    private void ChasePlayer()
    {
        targetPosition = playerTransform.position;
        Direction newDirection = DetermineDirection(targetPosition);
        lastDirection = newDirection;

        if (Time.time >= nextTurnTime)
        {
            if (lastDirection == Direction.Side)
            {
                FlipCharacter();
            }
            switch (lastDirection)
            {
                case Direction.Front:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
                    attackRange = 4.5f;
                    break;
                case Direction.Back:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, 180);
                    attackRange = 3f;
                    break;
                case Direction.Side:
                    attackPoint.transform.rotation = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 90 : -90);
                    attackRange = 3.5f;
                    break;
            }
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
        if (targetPosition.x > attackPoint.transform.position.x)
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
        float distanceToTarget = Vector3.Distance(targetPosition, attackPoint.transform.position);

        if (viewRange.inViewRange)
        {
            // If the distance is greater than the attack range and greater than a small threshold
            if (!IsPlayerInRange() && distanceToTarget > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                return true;
            }
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
                StartCoroutine(leftClawController.SwingClaw(lastDirection, true));
                break;
            case 2:
                StartCoroutine(rightClawController.SwingClaw(lastDirection, false));
                break;
            default:
                break;
        }

        Collider2D attackCollider = attackRangeCollider.GetComponent<Collider2D>();

        // Check if the collider exists
        if (attackCollider != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCollider.bounds.center, attackCollider.bounds.extents.x);

            foreach (Collider2D collider in colliders)
            {
                // Check if the collider has the "Player" tag
                if (collider.CompareTag("Player"))
                {
                    Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Calculate the direction from the explosion to the player
                        Vector2 pushDirection = rb.position - (Vector2)attackPoint.transform.position;
                        pushDirection.Normalize();
                        // Apply the explosion force
                        StartCoroutine(ApplyExplosionForce(rb, pushDirection, 10f));
                    }
                    playerController.TakeDamage(normalAttackDamage);
                }
            }
        }

        // Start the attack timeout coroutine
        StartCoroutine(AttackTimeout());
    }

    private void UseRandomGlobalSkill()
    {
        // Set the probabilities for each skill
        float probabilitySkill4 = 0.3f; // 30% chance
        float probabilitySkill1 = 0.7f; // 70% chance

        // Generate a random value between 0 and 1
        float randomValue = UnityEngine.Random.value;

        // Execute the chosen special skill based on probabilities
        if (randomValue < probabilitySkill4)
        {
            UseSkill1();
        }
        else
        {
            UseSkill1();
        }
    }

    private void UseRandomSkill()
    {
        float randomValue = UnityEngine.Random.value;
        Debug.Log("Random Value: " + randomValue);
        // Adjust the probabilities as needed
        if (randomValue < 0.2f && consecutiveSkill5Count < 1 && isPhase3) // 20% chance
        {
            UseSkill5();
            consecutiveSkill3Count = 0;
            consecutiveSkill4Count = 0;
            consecutiveSkill5Count++;
        }
        else if (randomValue < 0.6f && consecutiveSkill3Count < 2) // 40% chance
        {
            UseSkill3();
            consecutiveSkill3Count++;
            consecutiveSkill4Count = 0;
            consecutiveSkill5Count = 0;
        }
        else if (consecutiveSkill4Count < 2)// 40% chance
        {
            UseSkill4();
            consecutiveSkill3Count = 0;
            consecutiveSkill4Count++;
            consecutiveSkill5Count = 0;
        }
    }

    private void UseSkill1()
    {
        // Editable parameters
        int numberOfBombs = 25; // Adjust the number of bombs
        float gridSize = 2.5f; // Editable grid size

        List<Vector2> bombPositions = new List<Vector2>();
        int maxAttempts = 100; // Limit the number of attempts to avoid freezing

        for (int i = 0; i < numberOfBombs; i++)
        {
            int attempts = 0;
            Vector2 bombPosition = Vector2.zero;

            // Try to find a valid position that doesn't overlap with existing bombs
            while (attempts < maxAttempts)
            {
                // Calculate grid coordinates
                int gridX = Mathf.FloorToInt(UnityEngine.Random.Range(-7.5f, 7.5f) / gridSize);
                int gridY = Mathf.FloorToInt(UnityEngine.Random.Range(-7.75f, 7.75f) / gridSize);

                // Calculate the position of the bomb at the center of the grid cell with random offset
                bombPosition = new Vector2(
                    gridX * gridSize + gridSize / 2f,
                    gridY * gridSize + gridSize / 2f
                );

                // Check if the bomb position is already occupied
                if (!bombPositions.Contains(bombPosition))
                {
                    bombPositions.Add(bombPosition);
                    break;
                }

                attempts++;
            }

            // Instantiate the bomb at the valid position
            Instantiate(Skill1, bombPosition, Quaternion.identity);
        }
    }

    private void UseSkill2()
    {
        isAttacking = true;
        // Editable parameters
        float chargeSpeed = 20f; // Adjust the charge speed
        float borderSize = 7f; // Adjust the size of the border
        float ignoreBorderTime = 0.25f; // Time to ignore the border conditions at the beginning
        int numberOfCharges = 1;
        if (isPhase2)
        {
            numberOfCharges = 3;
        }

        // Move the boss toward the target position
        StartCoroutine(MultiCharge(numberOfCharges, chargeSpeed, borderSize, ignoreBorderTime));
    }

    private IEnumerator MultiCharge(int numberOfCharges, float chargeSpeed, float borderSize, float ignoreBorderTime)
    {
        bool skill2Finish = false;
        GameObject indicator = null;
        Collider2D chargeCollider = chargeColliderSkill2.GetComponent<Collider2D>();
        Instantiate(WaterSplashSkill2, attackPoint.transform.position, attackPoint.transform.rotation);
        Instantiate(BossRoar, attackPoint.transform.position, attackPoint.transform.rotation);
        yield return new WaitForSeconds(0.5f);

        for (int chargeCount = 0; chargeCount < numberOfCharges; chargeCount++)
        {
            if (!skill2Finish)
            {
                // Calculate the direction from the boss to the player
                Vector3 chargeDirection = (playerTransform.position - attackPoint.transform.position).normalized;

                // Check if the boss and player are on the same axis
                if (Mathf.Abs(chargeDirection.x) > Mathf.Abs(chargeDirection.y))
                {
                    // Align with the x-axis
                    chargeDirection.y = 0f;
                }
                else
                {
                    // Align with the y-axis
                    chargeDirection.x = 0f;
                }

                // Calculate the target position based on the border size with an additional 1 unit margin
                Vector3 targetPosition = attackPoint.transform.position + chargeDirection * 100f; // 100f is a magic number

                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, chargeDirection);

                //Rotate Boss
                float zRotation = 0f;
                if (-rotation.eulerAngles.z == 180f || -rotation.eulerAngles.z == -180f)
                {
                    zRotation = 0f;
                }
                else if (-rotation.eulerAngles.z == 0f)
                {
                    zRotation = 180f;
                }
                else
                {
                    zRotation = -rotation.eulerAngles.z;
                }

                attackPoint.transform.rotation = Quaternion.Euler(0, 0, zRotation);

                // Instantiate the indicator sprite
                indicator = Instantiate(indicatorSkill2, attackPoint.transform.position, rotation);

                yield return new WaitForSeconds(0.5f); // Wait for sec before charge

                float elapsedTime = 0f;

                while (Vector3.Distance(attackPoint.transform.position, targetPosition) > 0.1f || elapsedTime < ignoreBorderTime)
                {
                    // Move towards the target position
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);

                    // Check if the boss is outside the borders after the ignoreBorderTime
                    if (elapsedTime >= ignoreBorderTime &&
                        (attackPoint.transform.position.x > (borderSize + 1f) || attackPoint.transform.position.x < (-borderSize - 1f) || attackPoint.transform.position.y > (borderSize + 1f) || attackPoint.transform.position.y < (-borderSize - 1f)))
                    {
                        // Teleport the boss to a valid position
                        if (numberOfCharges > 1)
                        {
                            Instantiate(WaterSplashSkill2, attackPoint.transform.position, rotation);
                            transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                            yield return new WaitForSeconds(0.5f);
                            if (chargeCount == 2)
                            {
                                transform.localScale = new Vector3(1f, 1f, 1f);
                                playerController.RestoreFireMarkScale();
                                transform.position = new Vector3(0, 0, 0);
                                Instantiate(WaterSplashSkill2, attackPoint.transform.position, rotation);
                            }
                            else
                            {
                                transform.localScale = new Vector3(1f, 1f, 1f);
                                playerController.RestoreFireMarkScale();
                                transform.position = GetValidTeleportPosition(borderSize, playerTransform.position);
                                Instantiate(WaterSplashSkill2, attackPoint.transform.position, rotation);
                            }
                        }
                        break;
                    }

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(chargeCollider.bounds.center, chargeCollider.bounds.extents.x);

                    foreach (Collider2D collider in colliders)
                    {
                        // Check if the collider has the "Player" tag
                        if (collider.CompareTag("PlayerFoot"))
                        {
                            StartCoroutine(leftClawController.SwingClaw(lastDirection, true));
                            StartCoroutine(rightClawController.SwingClaw(lastDirection, false));

                            Rigidbody2D rb = collider.transform.parent.GetComponent<Rigidbody2D>();
                            if (rb != null)
                            {
                                // Calculate the direction from the explosion to the player
                                Vector2 pushDirection = rb.position - (Vector2)attackPoint.transform.position;
                                pushDirection.Normalize();
                                // Apply the explosion force
                                StartCoroutine(ApplyExplosionForce(rb, pushDirection, 15f));
                            }
                            if (!skill2Finish)
                            {
                                playerController.TakeDamage(skill2Dmg);
                                Debug.Log("Player take: " + skill2Dmg);
                                skill2Finish = true;
                            }
                        }
                    }

                    if (skill2Finish)
                    {
                        break;
                        yield return new WaitForSeconds(1f);
                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            // Add a delay between charges if needed
            Destroy(indicator);
        }

        // Boss has completed charges, perform any additional logic
        isAttacking = false;
        skill3DelayCountdown = 0f;
    }

    // Helper function to get a valid teleport position within the borders

    private void Teleport(Vector3 position)
    {
        isAttacking = true;
        isSpecialAttack = true;
        StartCoroutine(ActualTeleport(position));
    }

    private IEnumerator ActualTeleport(Vector3 position)
    {
        if (position != null)
        {
            Instantiate(WaterSplashSkill2, attackPoint.transform.position, attackPoint.transform.rotation);
            transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            yield return new WaitForSeconds(1f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            playerController.RestoreFireMarkScale();
            transform.position = position;
            Instantiate(WaterSplashSkill2, attackPoint.transform.position, attackPoint.transform.rotation);
        }

        yield return null;
    }
    private Vector3 GetValidTeleportPosition(float borderSize, Vector3 playerPosition)
    {
        //float randomX = UnityEngine.Random.Range(0, 2) == 0 ? -playerPosition.x : playerPosition.x;
        //float randomY = UnityEngine.Random.Range(0, 2) == 0 ? -playerPosition.y : playerPosition.y;
        float randomX = playerPosition.x;
        float randomY = playerPosition.y;

        if (UnityEngine.Random.value < 0.5f)
        {
            //randomX = UnityEngine.Random.Range(0, 2) == 0 ? (-borderSize) : (borderSize);
            randomX = borderSize;
        }
        else
        {
            //randomY = UnityEngine.Random.Range(0, 2) == 0 ? (-borderSize) : (borderSize);
            randomY = borderSize;
        }

        if (randomX >= borderSize)
        {
            randomX = (borderSize);
        }

        if (randomX <= -borderSize)
        {
            randomX = (-borderSize);
        }

        if (randomY >= borderSize)
        {
            randomY = (borderSize);
        }

        if (randomY <= -borderSize)
        {
            randomY = (-borderSize);
        }

        Debug.Log("Tele Position: " + randomX + " " + randomY);

        Vector3 telePosition = new Vector3(randomX, randomY, 0f);
        return telePosition;
    }

    private void UseSkill3()
    {
        StartCoroutine(ActivateSkill3(skill3ATKSPD, skill3SPD, skill3Duration, skill3FadeOutTime));
    }

    private IEnumerator ActivateSkill3(float newAttackCooldown, float newMoveSpeed, float duration, float fadeOutTime)
    {
        // Save the current values
        float originalAttackCooldown = attackCooldown;
        float originalMoveSpeed = moveSpeed;

        // Apply the new values
        attackCooldown = newAttackCooldown;
        moveSpeed = newMoveSpeed;

        // Activate Skill3
        Skill3.SetActive(true);

        // Wait for the main duration
        yield return new WaitForSeconds(duration);

        // Revert to the original values
        attackCooldown = originalAttackCooldown;
        moveSpeed = originalMoveSpeed;

        // Deactivate Skill3
        Skill3.SetActive(false);

        //SpawnSkill4CustomPattern(12, 6f);
    }

    private void SpawnSkill4CustomPattern(int arms, float circleSize)
    {
        // Assuming Skill4Prefab is the prefab you want to spawn
        GameObject skill4Prefab = Skill4;

        // Get the boss position as the center
        Vector3 bossPosition = transform.position;

        // Create an empty GameObject to act as a parent for the spawned prefabs
        GameObject gridParent = new GameObject("Skill4Grid");
        gridParent.transform.position = bossPosition;

        // Spawn Skill4 prefabs in a custom pattern
        for (int i = 0; i < arms/2; i++)
        {
            float angle = i * 360f / (arms/2);
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);

            // Scale the direction vector to adjust the circle size
            direction *= (circleSize/2);

            GameObject skill4Instance = Instantiate(skill4Prefab, bossPosition + direction, Quaternion.identity);

            skill4Instance.transform.parent = gridParent.transform;
        }
        for (int i = 0; i < arms; i++)
        {
            float angle = i * 360f / arms;
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);

            // Scale the direction vector to adjust the circle size
            direction *= circleSize;

            GameObject skill4Instance = Instantiate(skill4Prefab, bossPosition + direction, Quaternion.identity);

            skill4Instance.transform.parent = gridParent.transform;
        }
        for (int i = 0; i < arms*1.5f; i++)
        {
            float angle = i * 360f / (arms*1.5f);
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);

            // Scale the direction vector to adjust the circle size
            direction *= (circleSize*1.5f);

            GameObject skill4Instance = Instantiate(skill4Prefab, bossPosition + direction, Quaternion.identity);

            skill4Instance.transform.parent = gridParent.transform;
        }
    }



    private void UseSkill4()
    {
        StartCoroutine(ActivateSkill4());
    }

    private IEnumerator ActivateSkill4()
    {
        float elapsedTime = 0f;
        if (!isPhase2)
        {
            isAttacking = true;
        }

        while (elapsedTime < skill4Duration)
        {
            // Instantiate Skill4Prefab at player's position
            GameObject skillInstance = Instantiate(Skill4, GetPlayerPosition(), Quaternion.identity);
            if (!isPhase2)
            {
                ChasePlayer();
            }

            // Wait for the interval before spawning the next Skill4Prefab
            yield return new WaitForSeconds(skill4Interval);

            // Update the elapsed time
            elapsedTime += skill4Interval;
        }
        if (!isPhase2)
        {
            isAttacking = false;
        }
    }

    private Vector3 GetPlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }

        // If player is not found, return a default position
        return Vector3.zero;
    }

    private void UseSkill5()
    {
        StartCoroutine(ActivateSkill5());
    }

    private IEnumerator ActivateSkill5()
    {
        Teleport(new Vector3(0, 0, 0));
        isSpecialAttack = false;
        yield return new WaitForSeconds(2f);
        ChasePlayer();
        // Instantiate the Skill5 visual effect
        GameObject Skill5VFX = Instantiate(Skill5, attackPoint.transform.position, attackPoint.transform.rotation);

        // Push the player back repeatedly for the specified duration
        float elapsedTime = 0f;

        while (elapsedTime < skill5Duration)
        {
            PushPlayer();

            // Wait for a short interval before the next push
            yield return new WaitForSeconds(skill5Interval);

            elapsedTime += skill5Interval;
        }
        if (!isPhase3)
        {
            Instantiate(RainVFX, new Vector3(0, 0, 0), Quaternion.identity);
        }

        // Stop pushing the player back
        Destroy(Skill5VFX);
        isAttacking = false;
    }

    private void UseSkill6()
    {
        StartCoroutine(ActivateSkill6());
    }

    private IEnumerator ActivateSkill6()
    {
        Teleport(new Vector3(0, 0, 0));
        //Rotate boss
        attackPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
        attackRange = 4.5f;

        yield return new WaitForSeconds(2f);
        Skill6Buff.SetActive(true);
        yield return new WaitForSeconds(3f);

        CreateGrid(1, 0, 0);

        yield return new WaitForSeconds(0.25f);

        CreateGrid(2, 0, 0);

        yield return new WaitForSeconds(0.25f);

        CreateGrid(3, 0, 0);

        yield return new WaitForSeconds(0.25f);
        CreateGrid(4, 0, 0);

        yield return new WaitForSeconds(0.25f);
        CreateGrid(5, 0, 0);

        yield return new WaitForSeconds(0.25f);
        CreateGrid(6, 0, 0);

        yield return new WaitForSeconds(0.25f);
        CreateGrid(7, 0, 0);

        //yield return new WaitForSeconds(0.25f);
        //CreateGrid(8, 0, 0);

        //yield return new WaitForSeconds(0.25f);
        //CreateGrid(9, 0, 0);

        yield return new WaitForSeconds(3f);
        Skill6Buff.SetActive(false);
        isAttacking = false;
        isSpecialAttack = false;
        skill6Energy = 0;
    }

    private void CreateGrid(int size, float xOffset, float yOffset, float gridSize = 4.2f)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Check if the current position is on the edge
                if (i == 0 || i == size - 1 || j == 0 || j == size - 1)
                {
                    float posX = (i - size / 2.0f + 0.5f) * gridSize + xOffset;
                    float posY = (j - size / 2.0f + 0.5f) * gridSize + yOffset;

                    GameObject instantiatedObject = Instantiate(Skill6, new Vector2(posX, posY), Quaternion.identity);

                    // Set the scale of the instantiated object
                    instantiatedObject.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                }
            }
        }
    }

    public void GainEnergy()
    {
        skill6Energy += skill6OrbEnergy;
    }

    private void SpawnEnergyOrb()
    {
        // Calculate a random angle in radians
        float randomAngle = UnityEngine.Random.Range(0f, 360f);
        float radians = Mathf.Deg2Rad * randomAngle;

        // Calculate the spawn position in a circular pattern
        float spawnX = transform.position.x + 14f * Mathf.Cos(radians);
        float spawnY = transform.position.y + 14f * Mathf.Sin(radians);

        // Spawn the energy orb at the calculated position
        Instantiate(Skill6Spirit, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
    }

    // Add the following methods for pushing the player
    private void PushPlayer()
    {
        if (player != null)
        {
            // Calculate the direction from the boss to the player
            Vector2 pushDirection = player.transform.position - transform.position;
            pushDirection.Normalize();

            // Stop the existing velocity to prevent stacking
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.velocity = Vector2.zero;

            // Apply the new push force to the player
            playerRb.AddForce(pushDirection * skill5PushForce, ForceMode2D.Impulse);

            // Expire the force after a short duration
            StartCoroutine(ExpireForce(playerRb, skill5ForceDuration));
        }
    }

    private IEnumerator ExpireForce(Rigidbody2D rb, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Reset the player's velocity after the force expires
        rb.velocity = Vector2.zero;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        Debug.Log("Take: " + damage + " Boss HP: " + CurrentHP);
        // DropCoin
        var currentHpPercent = currentHP / maxHP * 100;
        if (currentHpPercent <= lastPercentTage - 10f)
        {
            if (DropCoinEvent != null) DropCoinEvent();
            lastPercentTage -= 10f;
        }

        // Play the particle system
        if (hitParticleSystem != null)
        {
            // Instantiate the particle system at the boss's position with a random offset
            Vector3 bossPosition = transform.position;
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0f);
            Vector3 particleSystemPosition = bossPosition + randomOffset;

            // Instantiate the particle system
            ParticleSystem instantiatedParticleSystem = Instantiate(hitParticleSystem, particleSystemPosition, Quaternion.identity);

            // Play the instantiated particle system
            instantiatedParticleSystem.Play();
        }
    }

    public void Dead()
    {
        if (currentHP <= 0)
        {
            Debug.Log("Boss Defeated");
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyExplosionForce(Rigidbody2D rb, Vector2 pushDirection, float force = 5f)
    {
        // Apply the initial explosion force
        rb.AddForce(pushDirection * force, ForceMode2D.Impulse);

        // Wait for a specified amount of time
        yield return new WaitForSeconds(0.25f); // Adjust the delay as needed

        // Reduce the force over time
        while (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity *= 0.9f; // Adjust the reduction factor as needed
            yield return new WaitForFixedUpdate();
        }

        // Stop the Rigidbody
        rb.velocity = Vector2.zero;
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
        if (Mathf.Abs(targetPosition.x - attackPoint.transform.position.x) > Mathf.Abs(targetPosition.y - attackPoint.transform.position.y))
        {
            return Direction.Side;
        }
        else
        {
            return targetPosition.y > attackPoint.transform.position.y ? Direction.Back : Direction.Front;
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

        // Assuming attackRangeCollider is a GameObject with a Collider2D component attached
        Collider2D attackCollider = attackRangeCollider.GetComponent<Collider2D>();

        // Check if the collider exists
        if (attackCollider != null)
        {
            float distanceToPlayer = Vector3.Distance(attackPoint.transform.position, playerTransform.position);

            if (attackCollider.bounds.Contains(playerTransform.position) && distanceToPlayer < attackRange)
            {
                inRange = true;
                //Debug.Log("Distance:" + distanceToPlayer);

                // Determine which claw is in range based on distance
                float distanceToLeftClaw = Vector3.Distance(leftClaw.transform.position, playerTransform.position);
                float distanceToRightClaw = Vector3.Distance(rightClaw.transform.position, playerTransform.position);

                if (distanceToLeftClaw < distanceToRightClaw)
                {
                    bossAttack = 1;
                }
                else
                {
                    bossAttack = 2;
                }
            }
        }

        return inRange;
    }

    private bool IsPlayerInChargeRange()
    {
        bool inRange = false;
        if (chargeRange != null)
        {
            // Get the BoxCollider2D attached to chargeRange
            BoxCollider2D chargeCollider = chargeRange.GetComponent<BoxCollider2D>();

            // Check if any collider with the "Player" tag is within the charge range collider
            Collider2D[] colliders = Physics2D.OverlapBoxAll(chargeCollider.bounds.center, chargeCollider.bounds.size, 0f);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // The collider belongs to an object with the "Player" tag
                    inRange = true;
                }
            }
        }

        return inRange;
    }
}
