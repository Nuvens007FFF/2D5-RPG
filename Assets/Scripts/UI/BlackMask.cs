using UnityEngine;
using System.Collections;

public class BlackMask : MonoBehaviour
{
    private SpriteRenderer maskRenderer;
    private Color targetColor = new Color(0, 0, 0, 1); // Black with full opacity
    public float fadeTime = 3f;

    void Start()
    {
        // Assuming the script is attached to a GameObject with a SpriteRenderer component
        maskRenderer = GetComponent<SpriteRenderer>();

        // Set the initial opacity to 0
        Color initialColor = new Color(0, 0, 0, 0);
        maskRenderer.color = initialColor;

        // Start the transition
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startingColor = maskRenderer.color;

        while (elapsedTime < fadeTime)
        {
            // Lerp the color over time
            maskRenderer.color = Color.Lerp(startingColor, targetColor, elapsedTime / fadeTime);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the target color is set
        maskRenderer.color = targetColor;
    }
}
