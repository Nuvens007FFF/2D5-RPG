using System;
using System.IO;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{   
    public static CoinSystem instance;
    public static event Action<float> CoinUpdated;
    public static event Action<float> CoinUpdatedUI;

    private float _coinIndex = 70000f  ;
    private float coinInBattle ;
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
        HealthManager.CharacterDied += SummaryCoin;
        Coin.TakeCoinInBattle += TakeCoinInBattle;
        coinInBattle = 0f;
        if (!File.Exists(GetCoinFilePath()))
        {
            CreateFileFormCoin();
        }
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
    private void OnDestroy()
    {
        HealthManager.CharacterDied -= SummaryCoin;
        Coin.TakeCoinInBattle -= TakeCoinInBattle;
    }
    public void TakeCoinInBattle(float coin)
    {
        coinInBattle += coin;
        if(CoinUpdatedUI != null) { CoinUpdatedUI(coinInBattle); }
        Debug.Log("coinInbatle =" + coinInBattle);
    }
    public void SummaryCoin()
    {
        if (coinInBattle == 0) return;
        Debug.Log("SummaryCoin");
        CoinIndex += coinInBattle;
        SaveCoinData();
        coinInBattle = 0;
    }
    void LoadCoinData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "FormCoin.Json");
        if (File.Exists(filePath))
        {
            try
            {
                string jsonCOin = File.ReadAllText(filePath);
                FormCoin formCoin = JsonUtility.FromJson<FormCoin>(jsonCOin);
                _coinIndex = formCoin.coinID;
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading coin data: " + e.Message);
            }
        }
        CoinUpdated?.Invoke(CoinIndex);
    }
    void SaveCoinData()
    {
        FormCoin formCoin = new FormCoin { coinID = CoinIndex };
        string jsonCoin = JsonUtility.ToJson(formCoin, true);
        File.WriteAllText(Application.persistentDataPath + "/FormCoin.Json", jsonCoin);
        CoinUpdated?.Invoke(CoinIndex);
    }
    void CreateFileFormCoin()
    {
        FormCoin formCoin = new FormCoin { coinID = CoinIndex };
        string jsonCoin = JsonUtility.ToJson(formCoin, true);
        string filePath = Path.Combine(Application.persistentDataPath, "FormCoin.Json");
        File.WriteAllText(filePath, jsonCoin);
    }
    private string GetCoinFilePath()
    {   
        return Path.Combine(Application.persistentDataPath, "FormCoin.Json");
    }
}
