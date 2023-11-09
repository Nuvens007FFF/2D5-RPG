
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public GameObject coinPrefabs;
    public int coinCount;
    public List<GameObject> coinList = new List<GameObject>();
    void Start()
    {
        for(int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefabs);
            coin.SetActive(false);
            coinList.Add(coin);
        }
        GameObject coin1 = GetCoin();
        coin1.transform.position = new Vector2(0, 2);
    }

    public GameObject GetCoin()
    {
        for(int i = 0;i < coinList.Count; i++)
        {
            if (!coinList[i].activeInHierarchy)
            {
                coinList[i].SetActive(true);
                return coinList[i];
            }
        }
        return null;
    }
}
