using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Vector2 direction;
    public float speed = 1.0f;
    public float bounceBackSpeed = 1.0f;
    public float randomXMin = 0.1f;
    public float randomXMax = 0.2f;
    public float randomYMin = 0.3f;
    public float randomYMax = 0.5f;

    private void Start()
    {
        // Initialize initial scrolling direction
        direction = new Vector2(Random.Range(randomXMin, randomXMax), Random.Range(randomYMin, randomYMax)).normalized;
    }

    private void Update()
    {
        // Move the sprite based on the direction and speed
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if the sprite has reached the specified borders
        if (transform.position.x > 3.0f || transform.position.y > 6.0f || transform.position.x < -3.0f || transform.position.y < -6.0f)
        {
            // Reverse direction and set random values
            direction = new Vector2(Random.Range(randomXMin, randomXMax), Random.Range(randomYMin, randomYMax)).normalized;

            // Bounce back with reduced speed
            bounceBackSpeed = -bounceBackSpeed;
            direction *= bounceBackSpeed; // Use bounceBackSpeed to reverse the direction
        }
    }
}
