using UnityEngine;

public class BossAttackTrail : MonoBehaviour
{
    public TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        // Adjust trail settings
        trailRenderer.time = 0.25f; // Adjust the lifetime of the trail points
        trailRenderer.widthMultiplier = 0.75f; // Adjust the width of the trail
        trailRenderer.startWidth = 0.5f; // Adjust the starting width of the trail
        trailRenderer.endWidth = 0.1f; // Adjust the ending width of the trail
        trailRenderer.emitting = false;
    }

    public void StartTrail()
    {
        trailRenderer.emitting = true;
    }

    public void EndTrail()
    {
        trailRenderer.emitting = false;
    }
}
