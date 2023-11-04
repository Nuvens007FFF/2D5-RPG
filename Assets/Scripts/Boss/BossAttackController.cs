using UnityEngine;
using System.Collections;
using CharacterEnums;

public class BossAttackController : MonoBehaviour
{
    public BossAttackTrail clawTrail;
    public IEnumerator SwingClaw(Direction attackDirection)
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

        float startAngle = Random.Range(70.0f, 135.0f);
        float endAngle = Random.Range(-135.0f, -70.0f);

        // Rotate slightly in the opposite direction for anticipation
        float anticipationAngle = startAngle - 10.0f;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, anticipationAngle);

        // Start the trail effect
        clawTrail.StartTrail();

        yield return new WaitForSeconds(anticipationDuration);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / swingDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t * t * (3f - 2f * t)); // Ease-in-out interpolation
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        clawTrail.EndTrail();
        yield return new WaitForSeconds(swingDuration);

        gameObject.transform.localRotation = Quaternion.identity;
    }
}
