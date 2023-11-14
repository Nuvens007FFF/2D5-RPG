using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionForce = 500f;
    public float explosionRadius = 3f;
    public int explosionDamage = 10;

    void Start()
    {
        // Explode after 2 seconds
        Invoke("Explode", 3f);
    }

    void Explode()
    {
        // Find all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the "Player" tag
            if (collider.CompareTag("Player"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Calculate the direction from the explosion to the player
                    Vector2 explosionDirection = rb.position - (Vector2)transform.position;
                    explosionDirection.Normalize();

                    // Apply the explosion force
                    rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
                }

                // Reduce the player's health
                //PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                //if (playerHealth != null)
                //{
                //    playerHealth.TakeDamage(explosionDamage);
                //}
            }
        }

        // Destroy the skill object
        Destroy(gameObject);
    }
}
