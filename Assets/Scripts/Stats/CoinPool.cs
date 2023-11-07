using System.Collections;
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
        for (int i = 0; i < countCoinInitial; i++)
        {
            var _coin = Instantiate(coin, transform.position, Quaternion.identity);
            _coin.SetActive(false);
            listCoin.Add(_coin);
        }
        var obj = AppearCoin();
        obj.transform.position = new Vector2(2, 0);
        obj.SetActive(true);
    }
    public GameObject AppearCoin()
    {
        for(int i = 0;i < listCoin.Count; i++)
        {
            if (!listCoin[i].activeInHierarchy)
            {
                return listCoin[i];
            }

        }
        return null;
    }
}
