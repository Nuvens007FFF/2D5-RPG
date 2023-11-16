
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public GameObject coin;
    public int countCoinInitial;
    public List<GameObject> listCoin = new List<GameObject>();
    void Start()
    {
        CreateCoinPool();
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

            SplitOutCoin();
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
        for (int i = 0; i < countCoinInitial; i++)
        {
            var obj = AppearCoin();
            obj.transform.position = new Vector2(Random.Range(6, 10), Random.Range(0, 6));
            obj.SetActive(true);
        }
    }
}
