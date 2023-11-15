using UnityEngine;

public class FollowBoss : MonoBehaviour
{
    public string bossTag = "Boss";
    public float followSpeed = 5.0f;

    private Transform bossTransform;

    private void Start()
    {
        // Find the GameObject with the specified tag
        GameObject bossObject = GameObject.FindGameObjectWithTag(bossTag);

        // Check if the bossObject is found
        if (bossObject != null)
        {
            // Get the Transform component of the bossObject
            bossTransform = bossObject.transform;
        }
        else
        {
            Debug.LogWarning("Boss not found. Make sure the boss has the specified tag.");
        }
    }

    private void Update()
    {
        // Check if the bossTransform is assigned
        if (bossTransform != null)
        {
            // Move towards the boss using Lerp for smooth movement
            transform.position = bossTransform.position;
        }
    }
}