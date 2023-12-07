using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotateSpeed = 30f; // Adjust the rotation speed as needed

    void Update()
    {
        // Rotate the object around its up axis (Y-axis) over time
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
