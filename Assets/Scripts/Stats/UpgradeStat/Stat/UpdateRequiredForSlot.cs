using System;
using System.IO;
using TMPro;
using UnityEngine;

public class UpdateRequiredForSlot : MonoBehaviour
{
    public static event Action<string,float> SatisfyUpdateRequired;
    public static event Action<string> SendMessageEvent ;

    [SerializeField] private TMP_Text CoinRequired;
    private  float _coinRequired = 10.0f;
    private float maxStat = 30.0f;
    private float maxStatAgi = 10.0f;
    private float statPlus = 1.0f;
    private float currentStatPlus = 0.0f;
    private float percentRise = 0.3f;
    private float percentRiseAgi = 0.9f;
    private string nameTag;

    

    private void Start()
    {
        if (CheckTag())
        {
            if (!File.Exists(GetFilePath(nameTag)))
            {
                CreateFormForStatSlot(nameTag);
            }
            LoadStatData(nameTag);
        }
        CoinRequiredUpdate(_coinRequired);
        //if(coinRequiredAfterUpdate != null) { coinRequiredAfterUpdate(coinRequired); }
        //Debug.Log(nameTag + ": coinRequied: " + coinRequired + ", maxStat: " + maxStat + " statPlus: " + statPlus + " currentStatPlus: " +currentStatPlus + " PercentRise: " +percentRise);
    }
    private void CoinRequiredUpdate(float newCoinValue)
    {   
        var roundCoinValue = (float)Math.Round(newCoinValue);
        CoinRequired.text = roundCoinValue.ToString();
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
    void CreateFormForStatSlot(string nameForm)
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

                string jsonAGI = JsonUtility.ToJson(agiData, true);
                string filePathAGI = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathAGI, jsonAGI);
                break;
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
                break;

            case "HP":
                HP hpData = JsonUtility.FromJson<HP>(json);
                statPlus = hpData.statPlus;
                _coinRequired = hpData.coinRequired;
                maxStat = hpData.maxStat;
                currentStatPlus = hpData.currentStatPlus;
                percentRise = hpData.percentRise;
                break;

            case "MP":
                MP mpData = JsonUtility.FromJson<MP>(json);
                statPlus = mpData.statPlus;
                _coinRequired = mpData.coinRequired;
                maxStat = mpData.maxStat;
                currentStatPlus = mpData.currentStatPlus;
                percentRise = mpData.percentRise;
                break;

            case "RegenMP":
                RegenMP regenMPData = JsonUtility.FromJson<RegenMP>(json);
                statPlus = regenMPData.statPlus;
                _coinRequired = regenMPData.coinRequired;
                maxStat = regenMPData.maxStat;
                currentStatPlus = regenMPData.currentStatPlus;
                percentRise = regenMPData.percentRise;
                break;

            case "AGI":
                AGI agiData = JsonUtility.FromJson<AGI>(json);
                statPlus = agiData.statPlus;
                _coinRequired = agiData.coinRequired;
                maxStat = agiData.maxStat;
                currentStatPlus = agiData.currentStatPlus;
                percentRise = agiData.percentRise;
                break;
        }
    }
    public void ClickUpgrade()
    {
        if (currentStatPlus >= maxStat)
        {
            var message = "Bạn đã nâng cấp kỹ năng này tối đa !";
            if (SendMessageEvent != null) SendMessageEvent(message);
            return;
        }
        if (CoinSystem.instance.GetCoin(_coinRequired))
        {
            if (SatisfyUpdateRequired != null) SatisfyUpdateRequired(nameTag,statPlus);
            UpRequiredAfterUpgrade();
            SaveStatData(nameTag);
        }
        else
        {
            var message = "Bạn không đủ tiền để nâng cấp kỹ năng này !";
            if(SendMessageEvent != null) SendMessageEvent(message);
        }
    }
    void UpRequiredAfterUpgrade()
    {
        currentStatPlus += statPlus;
        _coinRequired *= (1 + percentRise);
        if(_coinRequired < 500)
        {
            CoinRequiredUpdate(_coinRequired);
        }else if(_coinRequired >= 500)
        {
            _coinRequired = 500f;
            CoinRequiredUpdate(_coinRequired);
        }
        Debug.Log("currentStatPlus = " + currentStatPlus + " / CoinRequired = " + _coinRequired);

    }
    void SaveStatData(string nameForm)
    {
        switch (nameForm)
        {
            case "ATK":
                ATK atk = new ATK();
                atk.currentStatPlus = currentStatPlus;
                atk.coinRequired = _coinRequired;
                atk.statPlus = statPlus;
                atk.maxStat = maxStat;
                atk.percentRise = percentRise;

                string ATK = JsonUtility.ToJson(atk,true);
                string filePath = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePath, ATK);
                break;

            case "HP":
                HP hp = new HP();
                hp.currentStatPlus = currentStatPlus;
                hp.coinRequired = _coinRequired;
                hp.statPlus = statPlus;
                hp.maxStat = maxStat;
                hp.percentRise = percentRise;

                string HP = JsonUtility.ToJson(hp, true);
                string filePathHP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathHP, HP);
                break;

            case "MP":
                MP mp = new MP();
                mp.currentStatPlus = currentStatPlus;
                mp.coinRequired = _coinRequired;
                mp.statPlus = statPlus;
                mp.maxStat = maxStat;
                mp.percentRise = percentRise;

                string MP = JsonUtility.ToJson(mp, true);
                string filePathMP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathMP, MP);
                break;

            case "RegenMP":
                RegenMP regenMP = new RegenMP();
                regenMP.currentStatPlus = currentStatPlus;
                regenMP.coinRequired = _coinRequired;
                regenMP.statPlus = statPlus;
                regenMP.maxStat = maxStat;
                regenMP.percentRise = percentRise;

                string RegenMP = JsonUtility.ToJson(regenMP, true);
                string filePathRegenMP = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathRegenMP, RegenMP);
                break;

            case "AGI":
                AGI agi = new AGI();
                agi.currentStatPlus = currentStatPlus;
                agi.coinRequired = _coinRequired;
                agi.statPlus = statPlus;
                agi.maxStat = maxStatAgi;
                agi.percentRise = percentRiseAgi;

                string AGI = JsonUtility.ToJson(agi, true);
                string filePathAGI = Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
                File.WriteAllText(filePathAGI, AGI);
                break;
        }

    }
    string GetFilePath(string nameForm)
    {
        return Path.Combine(Application.persistentDataPath, "FormStat_" + nameForm + ".Json");
    }
}
