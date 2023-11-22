using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossViewRange : MonoBehaviour
{
    public bool inViewRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player enter view range");
            inViewRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exit view range");
            inViewRange = false;
        }
    }
}
