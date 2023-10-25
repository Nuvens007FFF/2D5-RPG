using System;
using System.IO;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{   
    public static CoinSystem instance;
    public static event Action<float> CoinUpdated;
    
    private float _coinIndex  ;
    public float CoinIndex
    {   
        set { _coinIndex = value; }
        get { return _coinIndex; }
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadCoinData();
    }
    
    public bool GetCoin(float coinRequired)
    {
        if(CoinIndex >= coinRequired)
        {
            CoinIndex -= (float)Math.Round(coinRequired);
            SaveCoinData();
            return true;
        }
        return false;
    }
    void LoadCoinData()
    {
        string jsonCOin = File.ReadAllText(Application.dataPath + "/FormCoin.Json");

        FormCoin formCoin = JsonUtility.FromJson<FormCoin>(jsonCOin);
        _coinIndex = formCoin.coinID;
        CoinUpdated?.Invoke(CoinIndex);
    }
    void SaveCoinData()
    {
        FormCoin formCoin = new FormCoin { coinID = CoinIndex };
        string jsonCoin = JsonUtility.ToJson(formCoin, true);
        File.WriteAllText(Application.dataPath + "/FormCoin.Json", jsonCoin);
        CoinUpdated?.Invoke(CoinIndex);
    }
}
