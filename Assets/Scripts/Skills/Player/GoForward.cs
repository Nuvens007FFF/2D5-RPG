using System.Collections;
using UnityEngine;

public class GoForward : MonoBehaviour
{
    public float speed = 10f;
    public float destroyDelay = 2f;
    private float damage = 10f;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    void Start()
    {
        // Start the coroutine to destroy the projectile after a delay
        StartCoroutine(DestroyAfterDelay());
    }

    void Update()
    {
        // Move the projectile forward using Translate
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    // OnTriggerEnter2D is called when another collider enters the trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider has a "Boss" tag
        if (other.CompareTag("Boss"))
        {
            // Debug log for verification
            Debug.Log("Hit");

            // Attempt to get the BossController script on the other GameObject
            BossController bossController = other.GetComponentInParent<BossController>();

            // Check if the script was found
            if (bossController != null)
            {
                // Call the TakeDamage method on the BossController
                bossController.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("BossController is null");
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
