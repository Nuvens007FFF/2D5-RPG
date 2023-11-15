using UnityEngine;
using System.Collections;
using CharacterEnums;

public class SwordController : MonoBehaviour
{
    public SwordParticleSystemTrail swordTrail;
    private Collider2D weaponCollider;

    private void Start()
    {
        // Get the Collider2D component on the weapon GameObject
        weaponCollider = GetComponent<Collider2D>();
        if (weaponCollider == null) Debug.LogError("Collider2D not found on weapon!");

        // Disable the trigger collider initially
        weaponCollider.enabled = false;
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

    public IEnumerator UseSkill(float duration = 0.3f)
    {
        //Temporary disable the sword sprite
        SpriteRenderer spriteRenderer = gameObject.transform.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(duration);
        spriteRenderer.enabled = true;
    }

    public IEnumerator SwingSword(Direction attackDirection, GameObject frontPivot, GameObject backPivot, GameObject rightPivot, GameObject defaultPivot)
    {
        float totalSwingDuration = 0.3f;
        float anticipationDuration = 0.01f;
        float swingDuration = (totalSwingDuration - anticipationDuration * 1) / 2;

        GameObject pivot = frontPivot;
        switch (attackDirection)
        {
            case Direction.Front:
                pivot = frontPivot;
                break;
            case Direction.Back:
                pivot = backPivot;
                break;
            case Direction.Side:
                pivot = rightPivot;
                break;
        }

        SpriteRenderer spriteRenderer = gameObject.transform.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        gameObject.transform.SetParent(pivot.transform, true);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < 2; i++)
        {
            float angleX = 45f;
            float angleY = -45f;
            float startZAngle = 135f;
            float endZAngle = -135f;

            if (i == 1) // Second swing (reverse of the first)
            {
                angleX = -45f;
                angleY = -45f;
                startZAngle = -135f;
                endZAngle = 135f;
            }
            //else if (i == 2)// Third swing (same as the first)
            //{
            //    angleX = 0f;
            //    angleY = -105f;
            //    startZAngle = 135f;
            //    endZAngle = -135f;
            //}

            // Rotate slightly in the opposite direction for anticipation
            float anticipationAngle = startZAngle - 10.0f;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, anticipationAngle);

            // Start the trail effect
            swordTrail.StartTrail();

            yield return new WaitForSeconds(anticipationDuration);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / swingDuration;
                float angleZ = Mathf.Lerp(startZAngle, endZAngle, t * t * (3f - 2f * t)); // Ease-in-out interpolation
                gameObject.transform.localRotation = Quaternion.Euler(angleX, angleY, angleZ);
                yield return null;
            }
            swordTrail.EndTrail();
            yield return new WaitForSeconds(swingDuration);
        }

        gameObject.transform.SetParent(defaultPivot.transform, true);
        spriteRenderer.enabled = true;
        gameObject.transform.localPosition = new Vector3(-0.75f, 2.85f, 0);
        gameObject.transform.localRotation = Quaternion.identity;
        swordTrail.ClearTrail();
        DisableTriggerCollider();
    }
}
