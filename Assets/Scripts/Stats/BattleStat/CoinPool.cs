
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CoinPool : MonoBehaviour
{   
    public static CoinPool instance;
    public GameObject coin;
    public int countCoinInitial;
    public List<GameObject> listCoin = new List<GameObject>();

    public float minRadius = 1f;
    public float maxRadius = 3f;
    public float forceMagnitude = 3f;
    public float maxFlyTime = 1f; // Adjust this to control the maximum fly time
    void Start()
    {
        BossController.DropCoinEvent += SplitOutCoin;
        CreateCoinPool();
    }
    private void OnDestroy()
    {
        BossController.DropCoinEvent -= SplitOutCoin;
    }
    void CreateCoinPool()
    {
        if (listCoin == null || listCoin.Count == 0)
        {
            listCoin = new List<GameObject>();

            for (int i = 0; i < countCoinInitial; i++)
            {
                var _coin = Instantiate(coin, Vector3.zero, Quaternion.identity);
                _coin.SetActive(false);
                listCoin.Add(_coin);
            }

        }
    }
    public GameObject AppearCoin()
    {
        for (int i = 0; i < listCoin.Count; i++)
        {
            if (listCoin[i] != null && !listCoin[i].activeInHierarchy)
            {
                return listCoin[i];
            }

        }
        return null;
    }
    public void SplitOutCoin()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null)
        {
            Vector2 bossPosition = boss.transform.position;

            for (int i = 0; i < countCoinInitial; i++)
            {
                var obj = AppearCoin();
                if (obj != null)
                {
                    obj.transform.position = GetRandomPositionAroundBoss(bossPosition, 3f, 5f);

                    Rigidbody2D coinRb = obj.GetComponent<Rigidbody2D>();
                    if (coinRb != null)
                    {
                        obj.SetActive(true);
                        Vector2 direction = (Vector2)obj.transform.position - bossPosition;
                        coinRb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);

                        StartCoroutine(StopCoin(coinRb));

                        obj.transform.position = new Vector2(
                            Mathf.Clamp(obj.transform.position.x, -4f, 4f),
                            Mathf.Clamp(obj.transform.position.y, -4f, 4f)
                        );
                    }
                }
            }
        }
    }

    private IEnumerator StopCoin(Rigidbody2D coinRb)
    {
        yield return new WaitForSeconds(maxFlyTime);
        coinRb.velocity = Vector2.zero;
        coinRb.angularVelocity = 0f;
    }


    private Vector2 GetRandomPositionAroundBoss(Vector2 bossPosition, float minRadius, float maxRadius)
    {
        // Calculate a random angle
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate a random radius within the specified range
        float radius = Random.Range(minRadius, maxRadius);

        // Calculate the position around the boss using polar coordinates
        float x = bossPosition.x + radius * Mathf.Cos(angle);
        float y = bossPosition.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}
