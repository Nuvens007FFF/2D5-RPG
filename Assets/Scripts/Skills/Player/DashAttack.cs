using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public float dashSpeed = 10f;
    public Vector3 dashDirection;
    public float dashDuration = 0.5f;

    private void Start()
    {
        dashDirection = transform.right; // Initial dash direction (right)
        Destroy(gameObject, dashDuration); // Destroy the object after a certain duration
    }

    private void Update()
    {
        // Move the skill in the dash direction
    }
}
