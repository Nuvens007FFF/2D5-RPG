using UnityEngine;
using Spine.Unity;
using System.Collections;
using CharacterEnums;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float actualMoveSpeed = 0f;
    public bool isSlowed = false;
    public float slowDuration = 0;
    public float slowTime = 0;
    public bool canNotBeSlow = false;
    public SkeletonAnimation skeletonAnimation;
    private Vector3 targetPosition;
    private Direction lastDirection;
    public ParticleSystem dashTrail;
    private Rigidbody2D rb;
    public SkillUIManager skillUI;
    public GameObject landObject;
    private Collider2D landCollider;
    private float initialWaitTime = 2.5f; // Adjust the initial delay as needed
    private float currentWaitTime = 0f;

    public enum CharacterState { Idle, Run, Attack };
    private CharacterState currentState = CharacterState.Idle;
    private CharacterState previousState;
    private bool isAttacking = false;
    public bool isDied = false;
    private float playDieVFX = 0;
    private bool isDashing = false;
    private bool allowMoving = true;
    private HealthManager healthManager;
    private ManaManager manaManager;
    private GameObject bossNian;
    private BossController bossNianController;
    private float currentBossHP = 0;
    private float previousBossHP = 99999f;
    private bool bossDied = false;

    public SwordController swordController;
    public GameObject frontPivot;
    public GameObject backPivot;
    public GameObject rightPivot;
    public GameObject defaultPivot;
    public GameObject Skill_1;
    public GameObject Skill_2;
    public GameObject Skill_3;
    public GameObject Skill_4;
    public GameObject Skill_1E;
    public GameObject Skill_3E;
    public GameObject Skill_3C;
    public GameObject Skill_3CE;
    public GameObject TeleportVFX; //Skill 2 VFX
    public GameObject FlameCircle; //Skill 4 VFX
    public GameObject FireMark; //Skill 4 VFX 
    public GameObject FireMarkExplosion; //Skill 4 VFX
    public GameObject MoveIndicator;
    public GameObject Debuff;
    public GameObject FallVFX;
    public float manaCostQ = 5f;
    public float manaCostW = 20f;
    public float manaCostE = 10f;
    public float manaCostR = 0;

    private bool isComboQETimeRunning = false;
    private bool isComboEQTimeRunning = false;
    private float comboQEDuration = 0f;
    private float comboEQDuration = 0f;
    public float comboTime = 1f;
    private bool allowComboQE = false;
    private bool allowComboEQ = false;

    public float attackCooldown = 1f; // Cooldown period in seconds
    private float nextAttackTime = 0f; // Time when the next attack can be performed
    public float skill1Damage = 10f;

    public float skill1Cooldown = 0.4f; // Cooldown period in seconds
    private float nextSkill1Time = 0f; // Time when the next attack can be performed

    public float skill2Cooldown = 10f; // Cooldown period in seconds
    public float skill2Duration = 5f; //Duration in seconds
    private float nextSkill2Time = 0f; // Time when the next skill can be performed
    private bool isSkill2Active = false;

    public float skill3Cooldown = 5f; // Cooldown period in seconds
    private float nextSkill3Time = 0f; // Time when the next attack can be performed
    public float skill3Damage = 20f; // Damage of Skill 3

    public float skill4Cooldown = 0f; // Cooldown period in seconds
    public float skill4Duration = 10f; //Duration in seconds
    private float skill4Energy = 0f; // Current Energy
    public float skill4EnergyGainPerHit = 5f; // Energy gained per hit to the boss
    public float skill4EnergyThreshold = 100f; // Energy threshold to activate Skill_4
    public float skill4DmgMult = 1.5f; //Skill 4 All Damage Buff
    private bool allowSkill4 = false;
    private bool isSkill4Active = false;
    private float skill4CurrentBossHP = 0;
    private bool UnlockSkill4;
    private bool applyInWaterDebuff = false;

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");
        if (swordController == null) Debug.LogError("swordController is not assigned!");
        dashTrail.Stop();
        dashTrail.Clear();

        // Find the player GameObject with the "BossObject" tag
        bossNian = GameObject.FindWithTag("BossObject");

        // Find the land GameObject with the "Land" tag
        landObject = GameObject.FindWithTag("Land");

        //Get player Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Check if the boss GameObject was found
        if (bossNian != null)
        {
            bossNianController = bossNian.GetComponent<BossController>();
            if (bossNianController != null)
            {
                currentBossHP = bossNianController.CurrentHP;
                Debug.Log("Boss HP: " + currentBossHP);
            }
            else
            {
                Debug.LogError("BossController component not found on " + bossNian.name);
            }
        }
        else
        {
            Debug.LogError("bossNian GameObject is null");
        }
        if (landObject != null)
        {
            landCollider = landObject.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogError("Land object not assigned!");
        }

        targetPosition = transform.position;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;

        HealthManager.CharacterDied += CharacterDied;
        //Update Stats
        moveSpeed = 2f + (UpdateStatCharacter.instance.Agi * 0.2f);
        actualMoveSpeed = moveSpeed;
        skill1Damage = UpdateStatCharacter.instance.Atk * 1f;
        skill3Damage = UpdateStatCharacter.instance.Atk * 1.5f;
        skill4EnergyGainPerHit = UpdateStatCharacter.instance.RegenMp * 0.25f;
        //Update Skill Upgrade
        skill1Cooldown = UpdateStatCharacter.instance.CoolDownQ;
        skill3Cooldown = UpdateStatCharacter.instance.CoolDownE;
        skill2Duration = UpdateStatCharacter.instance.DurationW;
        UnlockSkill4 = UpdateStatCharacter.instance.UnLockSkillR;

        GameObject healthBarObject = GameObject.Find("HealthBar");
        if (healthBarObject != null)
        {
            healthManager = healthBarObject.GetComponent<HealthManager>();
            if (healthManager == null)
            {
                Debug.LogError("HealthManager component not found on HealthBar!");
            }
        }
        else
        {
            Debug.LogError("HealthBar GameObject not found!");
        }

        GameObject manaBarObject = GameObject.Find("ManaBar");
        if (manaBarObject != null)
        {
            manaManager = manaBarObject.GetComponent<ManaManager>();
            if (manaManager == null)
            {
                Debug.LogError("ManaManager component not found on ManaBar!");
            }
        }
        else
        {
            Debug.LogError("ManaBar GameObject not found!");
        }

        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            skillUI = gameManagerObject.GetComponent<SkillUIManager>();
            if (skillUI == null)
            {
                Debug.LogError("ManaManager component not found on ManaBar!");
            }
        }
        else
        {
            Debug.LogError("ManaBar GameObject not found!");
        }
    }

    private void CharacterDied()
    {
        isDied = true;
    }

    public void TakeDamage(float damage)
    {
        // Apply reduced damage if UseSkill_2 is active
        if (isSkill2Active)
        {
            damage *= 0.5f; // Adjust the multiplier as needed
        }

        healthManager.TakeDamage(damage);
    }

    private void Update()
    {
        currentWaitTime += Time.deltaTime;
        if (currentWaitTime < initialWaitTime)
        {
            // Do nothing during the initial delay
            return;
        }

        if (isDied) 
        { 
            if(playDieVFX < 1)
            {
                Instantiate(TeleportVFX, transform.position, Quaternion.identity);
                transform.localScale = new Vector3(0.0001f, 0.0001f, 00001f);
                playDieVFX++;
            }
            return; 
        }

        if(isSlowed)
        {
            Debuff.SetActive(true);
            slowTime += Time.deltaTime;
            if (slowTime > slowDuration)
            {
                RestoreSpeed();
                slowTime = 0;
            }
        }

        if (IsOutsideLand())
        {
            ApplyDamageOverTime();
            if (!applyInWaterDebuff)
            {
                transform.Find("Model").localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                transform.Find("DefaultPivot/Weapon").localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                Instantiate(FallVFX, transform.position, Quaternion.identity);
                applyInWaterDebuff = true;
            }
        }
        else
        {
            if (applyInWaterDebuff)
            {
                transform.Find("Model").localScale = new Vector3(0.35f, 0.35f, 0.35f);
                transform.Find("DefaultPivot/Weapon").localScale = new Vector3(0.26f, 0.26f, 0.26f);
                applyInWaterDebuff = false;
            }
        }

        if (Input.GetMouseButtonDown(1) && !isAttacking && allowMoving) // Right mouse button clicked
        {
            SetNewTargetPosition(true);
            currentState = CharacterState.Run;
        }

        //Remove Normal Attack
        //if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime) // Left mouse button clicked
        //{
        //    SetNewTargetPosition();
        //    Attack();

        //    // Set the time for the next attack
        //    nextAttackTime = Time.time + attackCooldown;
        //}

        skillUI.EQCombo = isComboEQTimeRunning;
        skillUI.QECombo = isComboQETimeRunning;

        // Press Q to use Skill_1
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking && Time.time >= nextSkill1Time)
        {
            if (comboEQDuration > 0 && allowComboEQ)
            {
                UseComboSkill_1();
                comboEQDuration = 0;
            }
            else
            {
                // Start the combo timer or refresh if already active
                if (!isComboQETimeRunning)
                {
                    isComboQETimeRunning = true;                   
                }
                else
                {
                    comboQEDuration = comboTime; // Refresh the combo duration
                    Debug.Log("Combo QE time refreshed.");
                }

                // Use regular Skill_1
                UseSkill_1();
                comboQEDuration = comboTime;
            }

            // Set the time for the next skill 1
            skillUI.SkillQUsed();
            nextSkill1Time = Time.time + skill1Cooldown;
        }


        // Press W to use Skill_2
        if (Input.GetKeyDown(KeyCode.W) && Time.time >= nextSkill2Time)
        {
            UseSkill_2();

            // Set the time for the next skill 1
            skillUI.SkillWUsed();
            nextSkill2Time = Time.time + skill2Cooldown;
        }

        // Press E to use Skill_3
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking && Time.time >= nextSkill3Time)
        {
            if (comboQEDuration > 0 && allowComboQE)
            {
                rb.velocity = Vector2.zero;
                // Remove angular velocity (rotational forces)
                rb.angularVelocity = 0f;
                // Use combo skill if the combo is active
                UseComboSkill_3();
                comboQEDuration = 0;
            }
            else
            {
                // Start the combo timer or refresh if already active
                if (!isComboEQTimeRunning)
                {
                    isComboEQTimeRunning = true;
                }
                else
                {
                    comboEQDuration = comboTime; // Refresh the combo duration
                    Debug.Log("Combo EQ time refreshed.");
                }

                // Use regular Skill_3
                UseSkill_3();
                comboEQDuration = comboTime;
            }

            // Set the time for the next skill 3
            skillUI.SkillEUsed();
            nextSkill3Time = Time.time + skill3Cooldown;
        }

        // Update previousBossHP for the next frame
        previousBossHP = currentBossHP;

        if(currentBossHP <= 0)
        {
            bossDied = true;
        }

        // Update currentBossHP
        if (bossNianController != null)
        {
            currentBossHP = bossNianController.CurrentHP;
        }
        else
        {
            if (!bossDied)
            {
                Debug.LogError("Boss GameObject Controller not found!");
                //Update
                if (bossNian != null)
                {
                    bossNianController = bossNian.GetComponent<BossController>();
                }
            }
        }     

        // Check if the currentBossHP has changed
        if (currentBossHP < previousBossHP && !isSkill4Active)
        {
            // Boss has been hit, increase Skill_4 energy
            skill4Energy += skill4EnergyGainPerHit;
            skillUI.skillREnergy = skill4Energy;

            // Ensure energy doesn't exceed the threshold
            skill4Energy = Mathf.Clamp(skill4Energy, 0f, skill4EnergyThreshold);

            // Check if Skill_4 can be activated
            allowSkill4 = skill4Energy >= skill4EnergyThreshold;
            skillUI.isRready = allowSkill4;
            Debug.Log("Energy: " + skill4Energy);
        }

        // Press R to use Skill_4
        if (Input.GetKeyDown(KeyCode.R) && allowSkill4 && !isAttacking && UnlockSkill4)
        {
            UseSkill_4();
        }
        else if(Input.GetKeyDown(KeyCode.R) && !allowSkill4)
        {
            Debug.Log("Not enough Energy: " + allowSkill4);
        }
        if (isComboQETimeRunning)
        {
            comboQEDuration -= Time.deltaTime;

            // Check if the combo duration has expired
            if (comboQEDuration <= 0f)
            {
                isComboQETimeRunning = false;
                Debug.Log("Combo QE expired.");
                allowComboQE = false;
            }
        }

        if (isComboEQTimeRunning)
        {
            comboEQDuration -= Time.deltaTime;

            // Check if the combo duration has expired
            if (comboEQDuration <= 0f)
            {
                isComboEQTimeRunning = false;
                Debug.Log("Combo EQ expired.");
                allowComboEQ = false;
            }
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

    private bool IsOutsideLand()
    {
        // Check if the player is outside the land area
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

        return landCollider != null && !landCollider.bounds.Contains(transform.position) && playerRigidbody != null && playerRigidbody.velocity.magnitude < 5f;
    }

    private void ApplyDamageOverTime()
    {
        // Apply damage over time when outside the land area
        if (healthManager != null)
        {
            healthManager.TakeDamage(UpdateStatCharacter.instance.Hp * 1f * Time.deltaTime);
        }
    }

    private void SetNewTargetPosition(bool mouseClicked = false)
    {
        Vector3 newTargetPosition = GetMousePositionInWorldSpace();

        if (newTargetPosition != targetPosition)
        {
            if (mouseClicked)
            {
                Instantiate(MoveIndicator, newTargetPosition, Quaternion.identity);
            }
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
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, actualMoveSpeed * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void Attack()
    {
        isAttacking = true;
        currentState = CharacterState.Attack;

        // Enable the trigger collider on the weapon
        swordController.EnableTriggerCollider();

        // Start the swing sword coroutine
        StartCoroutine(swordController.SwingSword(lastDirection, frontPivot, backPivot, rightPivot, defaultPivot));

        // Start the attack timeout coroutine
        StartCoroutine(AttackTimeout());
    }

    private void UseSkill_1()
    {
        // Check if the skillPrefab is assigned
        if (Skill_1 != null && manaManager.Mana > manaCostQ)
        {
            SetNewTargetPosition();
            StartCoroutine(swordController.UseSkill(0.3f));
            allowComboQE = true;

            isAttacking = true;
            currentState = CharacterState.Attack;
            manaManager.MinusMana(manaCostQ);

            // Instantiate the Skill_1 prefab
            GameObject skillInstance = null;
            if (isSkill4Active)
            {
                skillInstance = Instantiate(Skill_1E, transform.position, Quaternion.identity);
                GoForward skillObject = skillInstance.GetComponent<GoForward>();
                skillObject.Damage = skill1Damage * skill4DmgMult;
            }
            else
            {
                skillInstance = Instantiate(Skill_1, transform.position, Quaternion.identity);
                GoForward skillObject = skillInstance.GetComponent<GoForward>();
                skillObject.Damage = skill1Damage;
            }

            // Set the direction of the skill (towards the mouse position)
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Calculate the angle based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Add some randomness to the rotation (adjust the range as needed)
            float randomRotationZ = Random.Range(-10f, 10f);
            angle += randomRotationZ;

            // Rotate the skillInstance to face the direction with randomness
            skillInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            // Set the scale of the skillInstance
            skillInstance.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);

            // Start the attack timeout coroutine
            StartCoroutine(AttackTimeout());
        }
        else
        {
            if (Skill_1 == null)
            {
                Debug.LogError("Skill prefab not assigned in the PlayerController!");
            }
        }
    }

    private void UseComboSkill_1()
    {
        StartCoroutine(ComboSkill_1());
    }

    private IEnumerator ComboSkill_1()
    {
        Debug.Log("ComboEQ");

        // Check if the skillPrefab is assigned
        if (Skill_1 != null && manaManager.Mana > (manaCostQ))
        {
            allowMoving = false;
            allowComboEQ = false;
            SetNewTargetPosition();
            StartCoroutine(swordController.UseSkill(0.6f));

            // Set the direction of the skill (towards the mouse position)
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Calculate the angle based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            isAttacking = true;
            currentState = CharacterState.Attack;
            manaManager.MinusMana(manaCostQ);
            TeleportPlayerBackward(angle);

            for (int i = 0; i < 4; i++)
            {
                if (i > 0)
                {
                    // Instantiate the Skill_1 prefab
                    GameObject skillInstance = null;
                    if (isSkill4Active)
                    {
                        skillInstance = Instantiate(Skill_1E, transform.position, Quaternion.identity);
                        GoForward skillObject = skillInstance.GetComponent<GoForward>();
                        skillObject.Damage = skill1Damage * skill4DmgMult;
                    }
                    else
                    {
                        skillInstance = Instantiate(Skill_1, transform.position, Quaternion.identity);
                        GoForward skillObject = skillInstance.GetComponent<GoForward>();
                        skillObject.Damage = skill1Damage;
                    }

                    // Add some randomness to the rotation (adjust the range as needed)
                    float randomRotationZ = Random.Range(-15f, 15f);
                    angle += randomRotationZ;

                    // Rotate the skillInstance to face the direction with randomness
                    skillInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                    // Set the scale of the skillInstance
                    skillInstance.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
                }

                // Add a delay between each instantiation
                yield return new WaitForSeconds(0.12f);
            }

            // Start the attack timeout coroutine
            StartCoroutine(AttackTimeout());
            allowMoving = true;
        }
        else
        {
            if (Skill_1 == null)
            {
                Debug.LogError("Skill prefab not assigned in the PlayerController!");
            }
        }
    }

    private IEnumerator SlidePlayerBackward(float angle)
    {
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.velocity = Vector2.zero;
        // Set the distance to slide backward (adjust as needed)
        float slideDistance = 3f;

        // Calculate the backward direction based on the angle
        Vector3 backwardDirection = Quaternion.Euler(0f, 0f, angle) * Vector3.left;

        // Calculate the target position for sliding
        Vector3 targetPosition = transform.position + backwardDirection * slideDistance;

        // Define the sliding duration
        float slideDuration = 0.1f;

        // Save the initial position of the player
        Vector3 initialPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            // Interpolate the player's position between initial and target positions
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / slideDuration);

            // Update elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the player reaches the exact target position
        transform.position = targetPosition;

        // Instantiate the TeleportVFX at the final position
        GameObject skillInstance2 = Instantiate(TeleportVFX, transform.position, Quaternion.identity);
    }

    private void TeleportPlayerBackward(float angle)
    {
        StartCoroutine(SlidePlayerBackward(angle));
    }

    private void UseSkill_2()
    {
        if (Skill_2 != null && manaManager.Mana > manaCostW)
        {
            // Reduce mana
            manaManager.MinusMana(manaCostW);

            // Instantiate the Skill_2 prefab
            GameObject skillInstance = Instantiate(Skill_2, transform.position, Quaternion.identity);

            // Set it as a child of the player
            skillInstance.transform.parent = transform.Find("Model");

            // Set the local position relative to the player (adjust as needed)
            skillInstance.transform.localPosition = new Vector3(0f, 0f, 0f);

            // Set the scale of the skillInstance
            skillInstance.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

            // Set isSkill2Active to true
            isSkill2Active = true;

            // Get the AutoDestroy component and set the destroyDelay
            AutoDestroy autoDestroy = skillInstance.GetComponent<AutoDestroy>();
            if (autoDestroy != null)
            {
                autoDestroy.destroyDelay = skill2Duration;
            }
            else
            {
                Debug.LogError("AutoDestroy component not found on Skill_2!");
            }

            // Start a coroutine to deactivate the skill after a certain duration (e.g., 5 seconds)
            StartCoroutine(DeactivateSkill2AfterDelay(skill2Duration));
        }
        else
        {
            if (Skill_2 == null)
            {
                Debug.LogError("Skill prefab not assigned in the PlayerController!");
            }
        }
    }

    private void UseSkill_3()
    {
        if (Skill_3 != null && manaManager.Mana > manaCostE)
        {
            SetNewTargetPosition();
            isAttacking = true;
            isDashing = true;
            currentState = CharacterState.Attack;
            manaManager.MinusMana(manaCostE);

            // Set the dash direction towards the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 dashDirection = (mousePosition - transform.position).normalized;
            StartCoroutine(Skill3Coroutine(dashDirection));
        }
        else
        {
            if (Skill_3 == null)
            {
                Debug.LogError("Skill_3 prefab not assigned in the PlayerController!");
            }
        }
    }

    private IEnumerator Skill3Coroutine(Vector3 dashDirection)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float dashDuration = 0.3f;
        float dashSpeed = 20f;
        float rigidbodyEnableDelay = 0.1f;

        // Save the initial position of the player
        Vector3 initialPosition = transform.position;

        //Enable trail
        dashTrail.Play();

        // Get the player's Rigidbody2D component
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

        // Store the player's original body type
        RigidbodyType2D originalBodyType = playerRigidbody.bodyType;

        while (elapsedTime < dashDuration)
        {
            // Calculate the target position based on the boss's position
            Vector3 targetPosition = initialPosition + dashDirection * dashSpeed * elapsedTime;

            // Move the player towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);

            // Update elapsed time
            elapsedTime = Time.time - startTime;

            // Check if hit Boss
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

            foreach (Collider2D collider in colliders)
            {
                // Check if the collider has the "Boss" tag
                if (collider.CompareTag("Boss"))
                {
                    Vector3 directionToBoss = (collider.transform.position - transform.position).normalized;
                    float dotProduct = Vector3.Dot(dashDirection.normalized, directionToBoss);

                    // Check if the player is moving towards the boss
                    if (dotProduct > 0)
                    {
                        StartCoroutine(swordController.UseSkill(0.3f));
                        Debug.Log("Skill3Hit");
                        // Instantiate the Skill_3 prefab
                        GameObject skillInstance = null;
                        if (isSkill4Active)
                        {
                            skillInstance = Instantiate(Skill_3E, transform.position, Quaternion.identity);
                            //Boss take damage
                            bossNianController.TakeDamage(skill3Damage*skill4DmgMult);
                        }
                        else
                        {
                            skillInstance = Instantiate(Skill_3, transform.position, Quaternion.identity);
                            //Boss take damage
                            bossNianController.TakeDamage(skill3Damage);
                        }
                        allowComboEQ = true;

                        // Set the direction of the skill 
                        Vector3 direction = (collider.transform.position - transform.position).normalized;

                        // Calculate the angle based on the direction
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                        // Rotate the skillInstance to face the direction
                        skillInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                        // Set the scale of the skillInstance
                        skillInstance.transform.localScale = new Vector3(1f, 1f, 1f);

                        // Disable the Rigidbody2D to prevent being pushed during dash
                        if (playerRigidbody != null)
                        {
                            playerRigidbody.velocity = Vector2.zero;
                            //playerRigidbody.simulated = false;

                            // Set the player's Rigidbody2D to kinematic during dash
                            playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
                        }

                        isDashing = false;
                    }
                }
            }

            if (!isDashing)
            {
                break;
            }

            yield return null;
        }

        //Disable trail
        dashTrail.Stop();

        // Delay before enabling the Rigidbody2D after dash
        yield return new WaitForSeconds(rigidbodyEnableDelay);

        // Enable user input after dash
        isAttacking = false;

        // Enable the Rigidbody2D after the delay
        if (playerRigidbody != null)
        {
            //playerRigidbody.simulated = true;
            playerRigidbody.bodyType = originalBodyType;

            //Also clear trail
            dashTrail.Clear();
        }

        // Start the attack timeout coroutine
        StartCoroutine(AttackTimeout());
    }

    private void UseComboSkill_3()
    {
        Debug.Log("ComboQE");

        if (Skill_3 != null && manaManager.Mana > manaCostE)
        {
            allowComboQE = false;
            SetNewTargetPosition();
            isAttacking = true;
            isDashing = true;
            currentState = CharacterState.Attack;
            manaManager.MinusMana(manaCostE);

            // Set the dash direction towards the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 dashDirection = (mousePosition - transform.position).normalized;
            StartCoroutine(Skill3ComboCoroutine(dashDirection));
            StartCoroutine(swordController.UseSkill(0.5f));
        }
        else
        {
            if (Skill_3 == null)
            {
                Debug.LogError("Skill_3 prefab not assigned in the PlayerController!");
            }
        }
    }

    private IEnumerator Skill3ComboCoroutine(Vector3 dashDirection)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;
        float dashDuration = 0.3f;
        float dashSpeed = 30f;
        float rigidbodyEnableDelay = 0.1f;
        float spawnInterval = 0.1f;

        Transform bossRigidBody = bossNian.transform.Find("AttackPoint");
        Rigidbody2D bossRigidbody = bossRigidBody.GetComponent<Rigidbody2D>();

        // Save the initial position of the player
        Vector3 initialPosition = transform.position;

        // Enable trail
        dashTrail.Play();

        // Get the player's Rigidbody2D component
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

       // playerRigidbody.simulated = false;
        // Store the player's original body type
        RigidbodyType2D originalBodyType = playerRigidbody.bodyType;

        // Set the player's Rigidbody2D to kinematic during dash
        playerRigidbody.bodyType = RigidbodyType2D.Kinematic;

        bool damageDealt = false;

        while (elapsedTime < dashDuration)
        {
            // Calculate the target position based on the dash direction and speed
            Vector3 targetPosition = initialPosition + dashDirection.normalized * dashSpeed * elapsedTime;

            // Move the player towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);

            // Update elapsed time
            elapsedTime = Time.time - startTime;

            // Check for collisions on the dash path
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

            foreach (Collider2D collider in colliders)
            {
                // Check if the collider is not the player
                if (collider.CompareTag("Boss"))
                {
                    if (!damageDealt)
                    {
                        // Boss take damage
                        if (isSkill4Active)
                        {
                            bossNianController.TakeDamage(skill3Damage * 1.5f * skill4DmgMult);
                        }
                        else
                        {
                            bossNianController.TakeDamage(skill3Damage * 1.5f);
                        }
                        damageDealt = true;

                        // Disable the boss's Rigidbody temporarily
                        if (bossRigidbody != null)
                        {
                            //bossRigidbody.simulated = false;
                        }
                    }
                }
            }

            // Spawn a prefab for each 0.1 seconds of dash duration
            if (elapsedTime >= spawnInterval)
            {
                // Instantiate the Skill_3 prefab
                GameObject skillInstance = null;
                if (isSkill4Active)
                {
                    skillInstance = Instantiate(Skill_3CE, transform.position, Quaternion.identity);
                }
                else
                {
                    skillInstance = Instantiate(Skill_3C, transform.position, Quaternion.identity);
                }

                // Set the direction of the skill
                Vector3 direction = dashDirection.normalized;

                // Calculate the angle based on the direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Rotate the skillInstance to face the direction
                skillInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Set the scale of the skillInstance
                skillInstance.transform.localScale = new Vector3(1f, 1f, 1f);

                spawnInterval += 0.075f; // Update spawn interval
            }

            yield return null;
        }

        // Disable trail
        dashTrail.Stop();

        // Delay before enabling the Rigidbody2D after dash
        yield return new WaitForSeconds(rigidbodyEnableDelay);

        // Enable user input after dash
        isAttacking = false;

        // Enable the Rigidbody2D after the delay
        if (playerRigidbody != null)
        {
            playerRigidbody.bodyType = originalBodyType;
            //playerRigidbody.simulated = true;

            // Also clear trail
            dashTrail.Clear();
        }

        // Start the attack timeout coroutine
        StartCoroutine(AttackTimeout());
    }

    private void UseSkill_4()
    {
        if (Skill_4 != null)
        {
            // Reduce energy
            skill4Energy = 0f;
            allowSkill4 = false;
            skill4CurrentBossHP = currentBossHP;

            // Instantiate the Skill_4 prefab
            GameObject skillInstance = Instantiate(Skill_4, transform.position, Quaternion.identity);
            GameObject skillInstance2 = Instantiate(FlameCircle, transform.position, Quaternion.identity);
            GameObject skillInstance3 = null;
            if (bossNian.transform.position != null)
            {
                skillInstance3 = Instantiate(FireMark, bossNian.transform.position, Quaternion.identity);
            }                   

            // Set it as a child of the player
            skillInstance.transform.parent = transform.Find("Model");
            skillInstance2.transform.parent = Camera.main.transform;
            skillInstance2.transform.localPosition = new Vector3(0f, 0f, 20f);
            //Set mark as a child of boss
            Transform centerTransform = bossNian.transform.Find("AttackPoint/Center");
            skillInstance3.transform.parent = centerTransform;
            skillInstance3.transform.localPosition = new Vector3(0f, 0f, 0f);

            // Set the local position relative to the player (adjust as needed)
            skillInstance.transform.localPosition = new Vector3(0.2f, 0.2f, 0f);

            // Set the scale of the skillInstance
            skillInstance.transform.localScale = new Vector3(2f, 2f, 2f);

            // Set isSkill4Active to true
            isSkill4Active = true;
            //canNotBeSlow = true;
            RestoreSpeed();
            actualMoveSpeed = moveSpeed + 1f;

            // Get the AutoDestroy component and set the destroyDelay
            AutoDestroy autoDestroy = skillInstance.GetComponent<AutoDestroy>();
            if (autoDestroy != null)
            {
                autoDestroy.destroyDelay = skill4Duration;
            }
            else
            {
                Debug.LogError("AutoDestroy component not found on Skill_4!");
            }

            // Start a coroutine to deactivate the skill after a certain duration (e.g., 10 seconds)
            StartCoroutine(DeactivateSkill4AfterDelay(skill4Duration));

            StartCoroutine(DestroyObjectsAroundPlayer());
        }
        else
        {
            if (Skill_4 == null)
            {
                Debug.LogError("Skill_4 prefab not assigned in the PlayerController!");
            }
        }
    }

    private IEnumerator DestroyObjectsAroundPlayer()
    {
        float radius = 20f; // Adjust the radius as needed
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                if (collider.CompareTag("Destroyable"))
                {
                    // Gradually destroy the object
                    float startTime = Time.time;
                    float destroyDuration = 0.01f; // Adjust the duration of destruction as needed

                    while (Time.time - startTime < destroyDuration)
                    {
                        float progress = (Time.time - startTime) / destroyDuration;
                        collider.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);
                        yield return null;
                    }

                    // Instantiate Flame at the destroyed object's position
                    Instantiate(TeleportVFX, collider.transform.position, Quaternion.identity);

                    // Destroy the object after the destruction animation
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    private IEnumerator DeactivateSkill2AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Set isSkill2Active to false after the delay
        isSkill2Active = false;
    }

    private IEnumerator DeactivateSkill4AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Set isSkill4Active to false after the delay
        isSkill4Active = false;

        //Deal damage at the end
        float bossDamageDuringSkill4 = skill4CurrentBossHP - currentBossHP;
        Debug.Log("Skill 4 Boss Hp Info:" + skill4CurrentBossHP + " - " + currentBossHP + " = " + bossDamageDuringSkill4);
        bossNianController.TakeDamage(bossDamageDuringSkill4 * 0.5f);
        GameObject skillInstance4 = Instantiate(FireMarkExplosion, bossNian.transform.position, Quaternion.identity);
        Transform centerTransform = bossNian.transform.Find("AttackPoint/Center");
        skillInstance4.transform.parent = centerTransform;
        skillInstance4.transform.localPosition = new Vector3(0f, 0f, 0f);

        canNotBeSlow = false;
        actualMoveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponentInParent<BossController>();
            if (boss != null)
            {
                // Deal damage to the target
                Debug.Log("BossHit");
            }
        }
    }

    private IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(0.4f); // Adjust to match the actual duration of your attack animation
        if (currentState == CharacterState.Attack)
        {
            //Enable debug as need
            //Debug.LogWarning("Attack animation did not complete. Reverting to Idle state.");
            isAttacking = false;
            currentState = CharacterState.Idle;
        }
    }

    private void RestoreSpeed()
    {
        Debuff.SetActive(false);
        actualMoveSpeed = moveSpeed;
        isSlowed = false;
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
}