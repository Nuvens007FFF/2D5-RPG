
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{   
    public static CoinPool instance;
    public GameObject coin;
    public int countCoinInitial;
    public List<GameObject> listCoin = new List<GameObject>();
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
        for (int i = 0; i < countCoinInitial; i++)
        {
            var obj = AppearCoin();
            if(obj != null)
            {
                obj.transform.position = new Vector2(Random.Range(-8, 8), Random.Range(-9, 11));
                obj.SetActive(true);
            }
        }
    }
}
