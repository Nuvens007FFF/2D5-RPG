using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    public float explosionForce = 500f;
    public float explosionDamage = 20f;
    public float explosionDelay = 3f;
    public float slowDuration = 1f; // Adjust the duration as needed

    private GameObject player;
    private PlayerController playerController;

    private Collider2D explosionCollider;

    void Start()
    {
        // Get the collider attached to this game object
        explosionCollider = GetComponent<Collider2D>();
        // Find the player GameObject with the "Player" tag
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        // Explode after the specified delay
        Invoke("Explode", explosionDelay);
    }

    private void Update()
    {
        if (player == null || playerController == null)
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Explode()
    {
        if (explosionCollider == null)
        {
            Debug.LogWarning("No collider found on the Explosion object.");
            return;
        }

        // Find all colliders within the explosion collider
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        Physics2D.OverlapCollider(explosionCollider, contactFilter, colliders);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the "PlayerFoot" tag
            if (collider.CompareTag("PlayerFoot"))
            {
                // Check if the collider has a parent
                if (collider.transform.parent != null)
                {
                    // Get the parent's Rigidbody
                    Rigidbody2D rb = collider.transform.parent.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Calculate the direction from the explosion to the player
                        Vector2 explosionDirection = rb.position - (Vector2)transform.position;
                        explosionDirection.Normalize();

                        // Apply the explosion force
                        rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

                        // Start a coroutine to gradually reduce the force over time                       
                        if ((playerController.slowDuration <= slowDuration || playerController.slowTime <= slowDuration) && (!playerController.canNotBeSlow))
                        {
                            playerController.actualMoveSpeed = playerController.moveSpeed / 3;
                            playerController.slowDuration = slowDuration;
                            playerController.isSlowed = true;
                            playerController.slowTime = 0;
                        }
                    }

                    playerController.TakeDamage(explosionDamage);
                }
            }
        }
    }
}