using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float disableTime = 5f; // Adjust this value to set the time before auto-disabling

    private void Start()
    {
        Invoke("DisableGameObject", disableTime);
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
