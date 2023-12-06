using UnityEngine;

public class AttachToBoss : MonoBehaviour
{
    public string bossTag = "BossCenter";

    private Transform bossTransform;

    void Start()
    {
        // Find the GameObject with the specified tag
        GameObject bossObject = GameObject.FindGameObjectWithTag(bossTag);

        if (bossObject != null)
        {
            // Get the transform of the boss object
            bossTransform = bossObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag '" + bossTag + "' found in the scene.");
        }
    }

    void Update()
    {
        // If bossTransform is not null, set the position of this object to the boss center position
        if (bossTransform != null)
        {
            transform.position = bossTransform.position;
        }
    }
}
