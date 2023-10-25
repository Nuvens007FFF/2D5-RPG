using UnityEngine;
using System.Collections;
using CharacterEnums;

public class SwordController : MonoBehaviour
{
    public SwordTrail swordTrail;
    public IEnumerator SwingSword(Direction attackDirection, GameObject frontPivot, GameObject backPivot, GameObject rightPivot, GameObject defaultPivot)
    {
        float totalSwingDuration = 0.3f;
        float anticipationDuration = 0.02f; // adjust this to a lower value
        float swingDuration = (totalSwingDuration - anticipationDuration*2) / 3;

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

        gameObject.transform.SetParent(pivot.transform, true);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        float lastEndAngle = 0;

        for (int i = 0; i < 3; i++)
        {
            float startAngle = Random.Range(70.0f, 135.0f);
            float endAngle = Random.Range(-135.0f, -70.0f);

            if (i == 1) // Second swing (reverse of the first)
            {
                endAngle = startAngle;
                startAngle = lastEndAngle;
            }
            else if (i == 2)// Third swing (same as the first)
            {
                startAngle = lastEndAngle;
                endAngle = Random.Range(-70.0f, -135.0f);
            }

            // Rotate slightly in the opposite direction for anticipation
            float anticipationAngle = startAngle - 10.0f;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, anticipationAngle);

            // Start the trail effect
            swordTrail.StartTrail();

            yield return new WaitForSeconds(anticipationDuration);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / swingDuration;
                float angle = Mathf.Lerp(startAngle, endAngle, t * t * (3f - 2f * t)); // Ease-in-out interpolation
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
                yield return null;
            }

            // Remember the last end angle for the next swing
            lastEndAngle = endAngle;

            swordTrail.EndTrail();
            yield return new WaitForSeconds(swingDuration);
        }

        gameObject.transform.SetParent(defaultPivot.transform, true);
        gameObject.transform.localPosition = new Vector3(-1, 3.9f, 0);
        gameObject.transform.localRotation = Quaternion.identity;
    }
}
