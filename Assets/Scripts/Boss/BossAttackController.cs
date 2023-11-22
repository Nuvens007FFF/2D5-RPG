using UnityEngine;
using System.Collections;
using CharacterEnums;

public class BossAttackController : MonoBehaviour
{
    public BossAttackTrail clawTrail1;
    public BossAttackTrail clawTrail2;
    public BossAttackTrail clawTrail3;
    private Collider2D weaponCollider;

    private void Start()
    {
        // Get the Collider2D component on the weapon GameObject
        weaponCollider = GetComponent<Collider2D>();
        if (weaponCollider == null) Debug.LogError("Collider2D not found on weapon!");
    }

    public void EnableTriggerCollider()
    {
        // Enable the trigger collider on the weapon
        weaponCollider.enabled = true;
    }

    public void DisableTriggerCollider()
    {
        // Disable the trigger collider on the weapon
        weaponCollider.enabled = false;
    }
    public IEnumerator SwingClaw(Direction attackDirection, bool left)
    {
        float totalSwingDuration = 0.35f;
        float anticipationDuration = 0.02f; // adjust this to a lower value
        float swingDuration = totalSwingDuration - anticipationDuration;

        switch (attackDirection)
        {
            case Direction.Front:
                break;
            case Direction.Back:
                break;
            case Direction.Side:
                break;
        }

        //float startAngle = Random.Range(70.0f, 135.0f);
        //float endAngle = Random.Range(-135.0f, -70.0f);
        float startAngle = 90f;
        float endAngle = -90f;
        if (left)
        {
            //startAngle = Random.Range(-135.0f, -70.0f);
            //endAngle = Random.Range(70.0f, 135.0f);
            startAngle = -90f;
            endAngle = 90f;
        }

        // Rotate slightly in the opposite direction for anticipation
        float anticipationAngle = startAngle - 10.0f;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, anticipationAngle);

        // Start the trail effect
        clawTrail1.StartTrail();
        clawTrail2.StartTrail();
        clawTrail3.StartTrail();

        yield return new WaitForSeconds(anticipationDuration);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / swingDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t * t * (3f - 2f * t)); // Ease-in-out interpolation
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        clawTrail1.EndTrail();
        clawTrail2.EndTrail();
        clawTrail3.EndTrail();
        yield return new WaitForSeconds(swingDuration);

        gameObject.transform.localRotation = Quaternion.identity;
    }
}
