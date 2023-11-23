using UnityEngine;

public class AutoMoveTowardsBoss : MonoBehaviour
{
    public string bossTag = "Boss";
    public float moveSpeed = 3f;

    private Transform bossTransform;
    public GameObject hitVFX;

    private void Start()
    {
        FindBoss();
    }

    private void Update()
    {
        if (bossTransform != null)
        {
            MoveTowardsBoss();
        }
        else
        {
            FindBoss();
        }
    }

    private void FindBoss()
    {
        // Find the GameObject with the specified tag
        GameObject bossObject = GameObject.FindGameObjectWithTag(bossTag);

        // Check if the boss object is found
        if (bossObject != null)
        {
            // Get the Transform component of the boss object
            bossTransform = bossObject.transform;
        }
        else
        {
            Instantiate(hitVFX, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    private void MoveTowardsBoss()
    {
        // Calculate the direction towards the boss
        Vector3 direction = bossTransform.position - transform.position;
        direction.Normalize();

        // Move towards the boss
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if collided with the player
        if (other.CompareTag("Player"))
        {
            // Destroy the object on collision with the player
            Instantiate(hitVFX, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }

        // Check if collided with the boss
        if (other.CompareTag("Boss"))
        {
            // Call the GainEnergy method on BossController
            BossController bossController = other.GetComponentInParent<BossController>();
            if (bossController != null)
            {
                Instantiate(hitVFX, gameObject.transform.position, gameObject.transform.rotation);
                bossController.GainEnergy();
            }
            else
            {
                Debug.Log("Ni");
            }

            // Destroy the object on collision with the boss
            Destroy(gameObject);
        }
    }
}
