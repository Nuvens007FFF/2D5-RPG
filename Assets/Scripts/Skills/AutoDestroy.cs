using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyDelay = 5f; // Set the duration after which the GameObject should be destroyed

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to destroy the GameObject after the specified delay
        StartCoroutine(DestroyAfterDelay());
    }

    // Coroutine to destroy the GameObject after a delay
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
