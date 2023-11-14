using UnityEngine;

public class Skill : MonoBehaviour
{
    public float explosionForce = 500f;
    public float explosionRadius = 3f;

    void Start()
    {
        // Explode after 2 seconds
        Invoke("Explode", 2f);
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
                    rb.AddForce(explosionDirection * explosionForce);
                }
            }
        }

        // Destroy the skill object
        Destroy(gameObject);
    }
}