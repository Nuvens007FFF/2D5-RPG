
using System;
using UnityEngine;

public class coin : MonoBehaviour
{
    public static event Action<float> takeCoin;
    public float coinIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Player"))
        {   
            if(takeCoin != null) { takeCoin(coinIndex); }
            this.gameObject.SetActive(false);

        } 
    }
}
