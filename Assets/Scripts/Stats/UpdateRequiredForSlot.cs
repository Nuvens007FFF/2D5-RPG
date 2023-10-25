using System;
using System.IO;
using UnityEngine;

public class UpdateRequiredForSlot : MonoBehaviour
{
    public static event Action<string,float> SatisfyUpdateRequired;

    private  float coinRequired;
    private float maxStat;
    private float statPlus;
    private float currentStatPlus;
    private float percentRise ;
    private string nameTag;
    

    private void Start()
    {
        if (CheckTag())
        {
            LoadStatData(nameTag);
        }
        Debug.Log(nameTag + ": coinRequied: " + coinRequired + ", maxStat: " + maxStat + " statPlus: " + statPlus + " currentStatPlus: " +currentStatPlus + " PercentRise: " +percentRise);
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
        // Tạo một đối tượng dữ liệu tùy thuộc vào tên `nameForm`
        switch (nameForm)
        {
            case "ATK":
                ATK atkData = new ATK();
                atkData.coinRequired = coinRequired;
                atkData.maxStat = maxStat;
                atkData.statPlus = statPlus;
                atkData.currentStatPlus = currentStatPlus;
                atkData.percentRise = percentRise;

                string jsonATK = JsonUtility.ToJson(atkData, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", jsonATK);
                break;
            case "HP":
                HP hpData = new HP();
                hpData.coinRequired = coinRequired;
                hpData.maxStat = maxStat;
                hpData.statPlus = statPlus;
                hpData.currentStatPlus = currentStatPlus;
                hpData.percentRise = percentRise;

                string jsonHP = JsonUtility.ToJson(hpData, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", jsonHP);
                break;
            case "MP":
                MP mpData = new MP();
                mpData.coinRequired = coinRequired;
                mpData.maxStat = maxStat;
                mpData.statPlus = statPlus;
                mpData.currentStatPlus = currentStatPlus;
                mpData.percentRise = percentRise;

                string jsonMP = JsonUtility.ToJson(mpData, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", jsonMP);
                break;
            case "RegenMP":
                RegenMP regenMPData = new RegenMP();
                regenMPData.coinRequired = coinRequired;
                regenMPData.maxStat = maxStat;
                regenMPData.statPlus = statPlus;
                regenMPData.currentStatPlus = currentStatPlus;
                regenMPData.percentRise = percentRise;

                string jsonRegenMP = JsonUtility.ToJson(regenMPData, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", jsonRegenMP);
                break;
            case "AGI":
                AGI agiData = new AGI();
                agiData.coinRequired = coinRequired;
                agiData.maxStat = maxStat;
                agiData.statPlus = statPlus;
                agiData.currentStatPlus = currentStatPlus;
                agiData.percentRise = percentRise;

                string jsonAGI = JsonUtility.ToJson(agiData, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", jsonAGI);
                break;
        }
    }
    public void LoadStatData(string nameForm)
    {
        string json = File.ReadAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json");

        switch (nameForm)
        {
            case "ATK":
                ATK atkData = JsonUtility.FromJson<ATK>(json);
                coinRequired = atkData.coinRequired;
                maxStat = atkData.maxStat;
                statPlus = atkData.statPlus;
                currentStatPlus = atkData.currentStatPlus;
                percentRise = atkData.percentRise;
                break;

            case "HP":
                HP hpData = JsonUtility.FromJson<HP>(json);
                statPlus = hpData.statPlus;
                coinRequired = hpData.coinRequired;
                maxStat = hpData.maxStat;
                currentStatPlus = hpData.currentStatPlus;
                percentRise = hpData.percentRise;
                break;

            case "MP":
                MP mpData = JsonUtility.FromJson<MP>(json);
                statPlus = mpData.statPlus;
                coinRequired = mpData.coinRequired;
                maxStat = mpData.maxStat;
                currentStatPlus = mpData.currentStatPlus;
                percentRise = mpData.percentRise;
                break;

            case "RegenMP":
                RegenMP regenMPData = JsonUtility.FromJson<RegenMP>(json);
                statPlus = regenMPData.statPlus;
                coinRequired = regenMPData.coinRequired;
                maxStat = regenMPData.maxStat;
                currentStatPlus = regenMPData.currentStatPlus;
                percentRise = regenMPData.percentRise;
                break;

            case "AGI":
                AGI agiData = JsonUtility.FromJson<AGI>(json);
                statPlus = agiData.statPlus;
                coinRequired = agiData.coinRequired;
                maxStat = agiData.maxStat;
                currentStatPlus = agiData.currentStatPlus;
                percentRise = agiData.percentRise;
                break;
        }
    }
    public void ClickUpgrade()
    {
        if (currentStatPlus >= maxStat) return;
        if (CoinSystem.instance.GetCoin(coinRequired))
        {
            if (SatisfyUpdateRequired != null) SatisfyUpdateRequired(nameTag,statPlus);
            UpRequiredAfterUpgrade();
            SaveStatData(nameTag);
        }
    }
    void UpRequiredAfterUpgrade()
    {
        currentStatPlus += statPlus;
        coinRequired *= (1 + percentRise);
        Debug.Log("currentStatPlus = " + currentStatPlus + " / CoinRequired = " + coinRequired);

    }
    void SaveStatData(string nameForm)
    {
        switch (nameForm)
        {
            case "ATK":
                ATK atk = new ATK();
                atk.currentStatPlus = currentStatPlus;
                atk.coinRequired = coinRequired;
                atk.statPlus = statPlus;
                atk.maxStat = maxStat;
                atk.percentRise = percentRise;
                string ATK = JsonUtility.ToJson(atk,true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", ATK);
                break;

            case "HP":
                HP hp = new HP();
                hp.currentStatPlus = currentStatPlus;
                hp.coinRequired = coinRequired;
                hp.statPlus = statPlus;
                hp.maxStat = maxStat;
                hp.percentRise = percentRise;
                string HP = JsonUtility.ToJson(hp, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", HP);
                break;

            case "MP":
                MP mp = new MP();
                mp.currentStatPlus = currentStatPlus;
                mp.coinRequired = coinRequired;
                mp.statPlus = statPlus;
                mp.maxStat = maxStat;
                mp.percentRise = percentRise;
                string MP = JsonUtility.ToJson(mp, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", MP);
                break;

            case "RegenMP":
                RegenMP regenMP = new RegenMP();
                regenMP.currentStatPlus = currentStatPlus;
                regenMP.coinRequired = coinRequired;
                regenMP.statPlus = statPlus;
                regenMP.maxStat = maxStat;
                regenMP.percentRise = percentRise;
                string RegenMP = JsonUtility.ToJson(regenMP, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", RegenMP);
                break;

            case "AGI":
                AGI agi = new AGI();
                agi.currentStatPlus = currentStatPlus;
                agi.coinRequired = coinRequired;
                agi.statPlus = statPlus;
                agi.maxStat = maxStat;
                agi.percentRise = percentRise;
                string AGI = JsonUtility.ToJson(agi, true);
                File.WriteAllText(Application.dataPath + "/FormStat_" + nameForm + ".Json", AGI);
                break;
        }

    }
}
