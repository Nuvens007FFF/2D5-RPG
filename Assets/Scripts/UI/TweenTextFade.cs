using UnityEngine;
using System.Collections;
using TMPro;

public class TweenTextFade : MonoBehaviour
{
    public float duration = 2.0f;
    public float minOpacity = 0.2f;
    public float maxOpacity = 1.0f;

    private TextMeshProUGUI textMesh;
    private bool isPingPongForward = true;

    void Start()
    {
        // Get the TextMeshPro component
        textMesh = GetComponent<TextMeshProUGUI>();

        // Start the opacity change coroutine
        StartCoroutine(ChangeOpacityOverTime());
    }

    IEnumerator ChangeOpacityOverTime()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time / duration, 1.0f);

            // Ping-pong between minOpacity and maxOpacity
            float targetOpacity = Mathf.Lerp(minOpacity, maxOpacity, t);

            // Set the text opacity
            Color currentColor = textMesh.color;
            textMesh.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);

            yield return null;
        }
    }
}
