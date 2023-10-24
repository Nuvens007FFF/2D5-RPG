using System.Collections;
using UnityEngine;

public class FloatingSword : MonoBehaviour
{
    public float floatSpeed = 2.0f;
    public float floatAmplitude = 0.1f;

    private void Update()
    {
        // Make the sword float up and down
        float floatingOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude * 0.0015f;
        transform.position += new Vector3(0, floatingOffset, 0);
    }
}
