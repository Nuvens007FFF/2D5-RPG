using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UpdateRequiredForSlot : MonoBehaviour
{
    public static event Action<string,float> SatisfyUpdgradeRequired;
    public static event Action<string,float> SatisfyDowngradeRequired;
    public static event Action<string> SendMessageEvent ;

    [SerializeField] private TMP_Text CoinRequired;

    private float _coinRequired = 10.0f;
    private float maxStat = 30.0f;
    private float maxStatAgi = 10.0f;
    private float statPlus = 1.0f;
    private float currentStatPlus = 0.0f;
    private float percentRise = 0.3f;
    private float percentRiseAgi = 0.6f;
    [SerializeField] private int countMaxPrice = -1;
    public List<float> history = new List<float>();
    public string nameTag;
    private void Start()
    {
        if (CheckTag())
        {
            if (!File.Exists(GetFilePath(nameTag)))
            {
                CreateFile(nameTag);
            }
            LoadStatData(nameTag);
        }
        CoinRequiredUpdate(_coinRequired);
    }
    private void CoinRequiredUpdate(float newCoinValue)
    {   
        _coinRequired = (float)Math.Round(newCoinValue);
        CoinRequired.text = _coinRequired.ToString();
    }
    public bool CheckTag()
    {
        if (gameObject.tag != null)
        {
            nameTag = gameObject.tag;
            return true;
        }
        return false;
    }
    void CreateFile(string nameForm)
    {
        switch (nameForm)
        {
            case "ATK":
                ATK atkData = new ATK();
                atkData.coinRequired = _coinRequired;
                atkData.maxStat = maxStat;
                atkData.statPlus = statPlus;
                atkData.currentStatPlus = currentStatPlus;
                atkData.percentRise = percentRise;
                atkData.countMaxPrice = countMaxPrice;
                atkData.history = history;

                string jsonATK = JsonUtility.ToJson(atkData, true);
                string filePathATK = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathATK, jsonATK);
                break;

            case "HP":
                HP hpData = new HP();
                hpData.coinRequired = _coinRequired;
                hpData.maxStat = maxStat;
                hpData.statPlus = statPlus;
                hpData.currentStatPlus = currentStatPlus;
                hpData.percentRise = percentRise;
                hpData.countMaxPrice = countMaxPrice;
                hpData.history = history;

                string jsonHP = JsonUtility.ToJson(hpData, true);
                string filePathHP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathHP, jsonHP);
                break;

            case "MP":
                MP mpData = new MP();
                mpData.coinRequired = _coinRequired;
                mpData.maxStat = maxStat;
                mpData.statPlus = statPlus;
                mpData.currentStatPlus = currentStatPlus;
                mpData.percentRise = percentRise;
                mpData.countMaxPrice = countMaxPrice;
                mpData.history = history;

                string jsonMP = JsonUtility.ToJson(mpData, true);
                string filePathMP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathMP, jsonMP);
                break;

            case "RegenMP":
                RegenMP regenMPData = new RegenMP();
                regenMPData.coinRequired = _coinRequired;
                regenMPData.maxStat = maxStat;
                regenMPData.statPlus = statPlus;
                regenMPData.currentStatPlus = currentStatPlus;
                regenMPData.percentRise = percentRise;
                regenMPData.countMaxPrice = countMaxPrice;
                regenMPData.history = history;

                string jsonRegenMP = JsonUtility.ToJson(regenMPData, true);
                string filePathRegenMP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathRegenMP, jsonRegenMP);
                break;
            case "AGI":
                AGI agiData = new AGI();
                agiData.coinRequired = _coinRequired;
                agiData.maxStat = maxStatAgi;
                agiData.statPlus = statPlus;
                agiData.currentStatPlus = currentStatPlus;
                agiData.percentRise = percentRiseAgi;
                agiData.countMaxPrice = countMaxPrice;
                agiData.history = history;

                string jsonAGI = JsonUtility.ToJson(agiData, true);
                string filePathAGI = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathAGI, jsonAGI);
                break;
        }
    }
    public void ClickUpgrade()
    {
        LoadStatData(nameTag);
        if (currentStatPlus >= maxStat)
        {
            var message = "Bạn đã nâng cấp kỹ năng này tối đa !";
            if (SendMessageEvent != null) SendMessageEvent(message);
            return;
        }
        if (CoinSystem.instance.GetCoin(_coinRequired))
        {
            if (SatisfyUpdgradeRequired != null) SatisfyUpdgradeRequired(nameTag,statPlus);
            History(_coinRequired);
            IncreasePrice();
            SaveData(nameTag);
        }
        else
        {
            var message = "Bạn không đủ tiền để nâng cấp kỹ năng này !";
            if(SendMessageEvent != null) SendMessageEvent(message);
        }
    }
    public void ClickDowngrade()
    {
        LoadStatData(nameTag);
        if(currentStatPlus <= 0)
        {
            var message = "Bạn không thể giảm thêm chỉ số !";
            if (SendMessageEvent != null) SendMessageEvent(message);
            return;
        }
        else
        {
            if (SatisfyUpdgradeRequired != null) SatisfyUpdgradeRequired(nameTag,-statPlus);
            DecreasePrice();
            var coinRefund =  RefundCoin();
            DeleteHistory();
            CoinSystem.instance.TakeCoin(coinRefund);
            SaveData(nameTag);
        }
    }
    void DecreasePrice()
    {
        currentStatPlus -= statPlus;
        if(countMaxPrice > 0)
        {
            countMaxPrice--;
            _coinRequired = 500f;
            CoinRequiredUpdate(_coinRequired);

        }else if (countMaxPrice == 0)
        {
            countMaxPrice--;
            if (nameTag == "AGI") _coinRequired = 438f; 
            else _coinRequired = 400f; 
            CoinRequiredUpdate(_coinRequired);
        }
        else
        {   
            _coinRequired /= (1 + percentRise );
            CoinRequiredUpdate(_coinRequired);
        }
        //Debug.Log("currentStatPlus = " + currentStatPlus + " / CoinRequired = " + _coinRequired);
    }
    float RefundCoin()
    {
        Debug.Log("CoinRefund = " + history[history.Count - 1]);
        return history[history.Count - 1];
    }
    void History(float coin)
    {
        Debug.Log("CoinBuyed = " + coin);
        history.Add(coin);
    }
    void DeleteHistory()
    {
        history.RemoveAt(history.Count - 1);
    }
    void IncreasePrice()
    {
        currentStatPlus += statPlus;
        if (_coinRequired < 500)
        {
            _coinRequired *= (1 + percentRise);
            if(_coinRequired > 500)
            {
                countMaxPrice++;
                _coinRequired = 500;
                CoinRequiredUpdate(_coinRequired);
            }
            else
            {
                CoinRequiredUpdate(_coinRequired);
            }
        }else if(_coinRequired >= 500)
        {
            countMaxPrice++;
            _coinRequired = 500f;
            CoinRequiredUpdate(_coinRequired);
        }

    }
    public void LoadStatData(string nameForm)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
        string json = File.ReadAllText(filePath);
        switch (nameForm)
        {
            case "ATK":
                ATK atkData = JsonUtility.FromJson<ATK>(json);
                _coinRequired = atkData.coinRequired;
                maxStat = atkData.maxStat;
                statPlus = atkData.statPlus;
                currentStatPlus = atkData.currentStatPlus;
                percentRise = atkData.percentRise;
                countMaxPrice = atkData.countMaxPrice;
                history = atkData.history;
                break;

            case "HP":
                HP hpData = JsonUtility.FromJson<HP>(json);
                statPlus = hpData.statPlus;
                _coinRequired = hpData.coinRequired;
                maxStat = hpData.maxStat;
                currentStatPlus = hpData.currentStatPlus;
                percentRise = hpData.percentRise;
                countMaxPrice = hpData.countMaxPrice;
                history = hpData.history;
                break;

            case "MP":
                MP mpData = JsonUtility.FromJson<MP>(json);
                statPlus = mpData.statPlus;
                _coinRequired = mpData.coinRequired;
                maxStat = mpData.maxStat;
                currentStatPlus = mpData.currentStatPlus;
                percentRise = mpData.percentRise;
                countMaxPrice = mpData.countMaxPrice;
                history = mpData.history;
                break;

            case "RegenMP":
                RegenMP regenMPData = JsonUtility.FromJson<RegenMP>(json);
                statPlus = regenMPData.statPlus;
                _coinRequired = regenMPData.coinRequired;
                maxStat = regenMPData.maxStat;
                currentStatPlus = regenMPData.currentStatPlus;
                percentRise = regenMPData.percentRise;
                countMaxPrice = regenMPData.countMaxPrice;
                history = regenMPData.history;
                break;

            case "AGI":
                AGI agiData = JsonUtility.FromJson<AGI>(json);
                statPlus = agiData.statPlus;
                _coinRequired = agiData.coinRequired;
                maxStat = agiData.maxStat;
                currentStatPlus = agiData.currentStatPlus;
                percentRise = agiData.percentRise;
                countMaxPrice = agiData.countMaxPrice;
                history = agiData.history;
                break;
        }
    }
    void SaveData(string nameForm)
    {
        CreateFile(nameForm);
    }
    string GetFilePath(string nameForm)
    {
        return Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
    }
}
