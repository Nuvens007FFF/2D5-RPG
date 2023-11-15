using System;
using UnityEngine;
using DG.Tweening;


public class Coin : MonoBehaviour
{
    public static event Action<float> TakeCoinInBattle;
    [Header("CoinIndex")]
    [SerializeField] private float coinAmount;
    [SerializeField] private float flySpeed;
    [SerializeField] private float radius;

    void Update()
    {   

        FindPlayer();
    }
    void FindPlayer()
    {   
        Collider2D[] coinColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var collider in coinColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 playerPosition = collider.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, playerPosition, Time.deltaTime * flySpeed);
                if (transform.position == playerPosition) OnFlyBackComplete();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
    private void OnFlyBackComplete()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
            if (TakeCoinInBattle != null) TakeCoinInBattle(coinAmount);
            //CoinSystem.instance.TakeCoinInBattle(coinAmount);
        }
    }
}
