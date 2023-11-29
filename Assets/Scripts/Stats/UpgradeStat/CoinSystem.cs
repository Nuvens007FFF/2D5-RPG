using System;
using System.IO;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{   
    public static CoinSystem instance;
    public static event Action<float> CoinUpdated;
    public static event Action<float> CoinUpdatedUI;

    private float _coinIndex = 80000f  ;
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
        if (!File.Exists(GetFilePath()))
        {
            CreateFileAndSaveData();
        }
        LoadFile();
    }
    public bool CheckCoin(float coinRequired)
    {
        if (CoinIndex >= coinRequired)
        {
            return true;
        }
        return false;
    }
    public bool GetCoin(float coinRequired)
    {
        if(CoinIndex >= coinRequired)
        {
            CoinIndex -= coinRequired;
            //CoinIndex -= (float)Math.Round(coinRequired);
            Debug.Log("coinUp after round = " + CoinIndex);
            SaveFile();
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        HealthManager.CharacterDied -= SummaryCoin;
        Coin.TakeCoinInBattle -= TakeCoinInBattle;
    }
    public void TakeCoin(float coin)
    {
        CoinIndex += coin;
        // CoinIndex += (float)Math.Round(coin);
        Debug.Log("coinDown after round = " + CoinIndex);
        SaveFile();
    }
    public void TakeCoinInBattle(float coin)
    {
        coinInBattle += (float)Math.Round(coin);
        if(CoinUpdatedUI != null) { CoinUpdatedUI(coinInBattle); }
        Debug.Log("coinInbatle =" + coinInBattle);
    }
    public void SummaryCoin()
    {
        if (coinInBattle == 0) return;
        Debug.Log("SummaryCoin");
        CoinIndex += coinInBattle;
        SaveFile();
        coinInBattle = 0;
    }
    void LoadFile()
    {
        if (File.Exists(GetFilePath()))
        {
            try
            {
                string jsonCOin = File.ReadAllText(GetFilePath());
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
    void SaveFile()
    {
        CreateFileAndSaveData();
        CoinUpdated?.Invoke(CoinIndex);
    }
    void CreateFileAndSaveData()
    {
        FormCoin formCoin = new FormCoin { coinID = CoinIndex };
        string jsonCoin = JsonUtility.ToJson(formCoin, true);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, jsonCoin);
    }
    private string GetFilePath()
    {   
        return Path.Combine(Application.persistentDataPath, "FormCoin.Json");
    }
}
