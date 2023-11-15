using UnityEngine;
using Spine.Unity;
using System.Collections;
using CharacterEnums;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private float mana = 10f;
    public SkeletonAnimation skeletonAnimation;
    private Vector3 targetPosition;
    private Direction lastDirection;
    public ParticleSystem dashTrail;

    public enum CharacterState { Idle, Run, Attack };
    private CharacterState currentState = CharacterState.Idle;
    private CharacterState previousState;
    private bool isAttacking = false;
    private bool isDied = false;
    private bool isDashing = false;
    private HealthManager healthManager;
    private ManaManager manaManager;
    private GameObject bossNian;
    private BossController bossNianController;
    private float currentBossHP = 0;
    private float previousBossHP = 99999f;

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

    private void Start()
    {
        if (skeletonAnimation == null) Debug.LogError("skeletonAnimation is not assigned!");
        if (swordController == null) Debug.LogError("swordController is not assigned!");
        dashTrail.Stop();
        dashTrail.Clear();

        // Find the player GameObject with the "Boss" tag
        bossNian = GameObject.FindWithTag("Boss");

        // Check if the boss GameObject was found
        if (bossNian != null)
        {
            bossNianController = bossNian.GetComponent<BossController>();
            currentBossHP = bossNianController.CurrentHP;
        }
        else
        {
            // Log an error if the boss GameObject was not found
            Debug.LogError("Boss GameObject not found in scene!");
        }

        targetPosition = transform.position;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;

        HealthManager.CharacterDied += CharacterDied;
        moveSpeed = UpdateStatCharacter.instance.Agi * 0.3f;
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
        if (isDied) { return; }
        if (Input.GetMouseButtonDown(1) && !isAttacking) // Right mouse button clicked
        {
            SetNewTargetPosition();
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
            nextSkill1Time = Time.time + skill1Cooldown;
        }


        // Press W to use Skill_2
        if (Input.GetKeyDown(KeyCode.W) && Time.time >= nextSkill2Time)
        {
            UseSkill_2();

            // Set the time for the next skill 1
            nextSkill2Time = Time.time + skill2Cooldown;
        }

        // Press E to use Skill_3
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking && Time.time >= nextSkill3Time)
        {
            if (comboQEDuration > 0 && allowComboQE)
            {
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
            nextSkill3Time = Time.time + skill3Cooldown;
        }

        // Update previousBossHP for the next frame
        previousBossHP = currentBossHP;

        // Update currentBossHP
        currentBossHP = bossNianController.CurrentHP;

        // Check if the currentBossHP has changed
        if (currentBossHP < previousBossHP && !isSkill4Active)
        {
            // Boss has been hit, increase Skill_4 energy
            skill4Energy += skill4EnergyGainPerHit;

            // Ensure energy doesn't exceed the threshold
            skill4Energy = Mathf.Clamp(skill4Energy, 0f, skill4EnergyThreshold);

            // Check if Skill_4 can be activated
            allowSkill4 = skill4Energy >= skill4EnergyThreshold;
            Debug.Log("Energy: " + skill4Energy);
        }

        // Press W to use Skill_4
        if (Input.GetKeyDown(KeyCode.R) && allowSkill4)
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
                yield return new WaitForSeconds(0.15f);
            }

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

    private IEnumerator SlidePlayerBackward(float angle)
    {
        // Set the distance to slide backward (adjust as needed)
        float slideDistance = 5f;

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
            skillInstance.transform.parent = transform;

            // Set the local position relative to the player (adjust as needed)
            skillInstance.transform.localPosition = new Vector3(0f, 0f, 0f);

            // Set the scale of the skillInstance
            skillInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
        float rigidbodyEnableDelay = 0.5f;

        // Save the initial position of the player
        Vector3 initialPosition = transform.position;

        //Enable trail
        dashTrail.Play();

        // Get the player's Rigidbody2D component
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

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
                            playerRigidbody.simulated = false;
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
            playerRigidbody.simulated = true;

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
        float rigidbodyEnableDelay = 0.5f;
        float spawnInterval = 0.1f;

        // Save the initial position of the player
        Vector3 initialPosition = transform.position;

        // Enable trail
        dashTrail.Play();

        // Get the player's Rigidbody2D component
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

        playerRigidbody.simulated = false;

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
                    }
                }
                //if (collider.CompareTag("Coin"))
                //{
                //    //Pick up the Coin
                //}
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
            playerRigidbody.simulated = true;

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
            GameObject skillInstance3 = Instantiate(FireMark, bossNian.transform.position, Quaternion.identity);           

            // Set it as a child of the player
            skillInstance.transform.parent = transform;
            skillInstance2.transform.parent = transform;
            //Set mark as a child of boss
            skillInstance3.transform.parent = bossNian.transform;
            Transform centerTransform = bossNian.transform.Find("AttackPoint/Center");
            skillInstance3.transform.parent = centerTransform;
            skillInstance3.transform.localPosition = new Vector3(0f, 0f, 0f);

            // Set the local position relative to the player (adjust as needed)
            skillInstance.transform.localPosition = new Vector3(0.2f, 0.2f, 0f);

            // Set the scale of the skillInstance
            skillInstance.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

            // Set isSkill4Active to true
            isSkill4Active = true;

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
        }
        else
        {
            if (Skill_4 == null)
            {
                Debug.LogError("Skill_4 prefab not assigned in the PlayerController!");
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