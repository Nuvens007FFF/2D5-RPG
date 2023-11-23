using UnityEngine;
using System.Collections;

public class SpriteDisplayOverTime : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fillDuration = 2.0f;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            // Assuming the SpriteRenderer is on the same GameObject
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start the display animation
        StartCoroutine(DisplayOverTime());
    }

    private IEnumerator DisplayOverTime()
    {
        float elapsedTime = 0f;
        Color initialColor = spriteRenderer.color;

        while (elapsedTime < fillDuration)
        {
            // Calculate the fill amount based on the elapsed time
            float fillAmount = elapsedTime / fillDuration;

            // Lerp the alpha channel to control visibility
            Color currentColor = new Color(initialColor.r, initialColor.g, initialColor.b, fillAmount);
            spriteRenderer.color = currentColor;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the sprite is fully displayed at the end
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
    }
}
