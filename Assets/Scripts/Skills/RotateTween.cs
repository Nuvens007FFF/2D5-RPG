using UnityEngine;
using System.Collections;

public class RotateTween : MonoBehaviour
{
    public float targetRotation = 90f;
    public float duration = 1f;

    void Start()
    {
        // Start the rotation tween when the script is enabled
        StartCoroutine(RotateOverTime());
    }

    IEnumerator RotateOverTime()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(0f, 0f, targetRotation);

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetQuaternion, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetQuaternion;

        Debug.Log("Rotation Tween Complete!");
    }
}
