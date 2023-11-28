using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;
    public static event Action<Dictionary<string, float>> OnUpgrade;

    private Dictionary<string,float> Stats = new Dictionary<string,float>();

    public CharacterStats hpStat;
    public CharacterStats atkStat;
    public CharacterStats MpStat;
    public CharacterStats RegenMpStat;
    public CharacterStats AgiStat;

    private float baseHp = 10.0f;
    private float baseAtk = 10.0f;
    private float baseMp = 10.0f;
    private float baseRegenMp = 10.0f;
    private float baseAGI = 5.0f;

    private void Awake()
    {
        Instance = this;
    }
    string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
    }
    private void Start()
    {   
        LoadData();
        UpdateRequiredForSlot.SatisfyUpdgradeRequired += AddNewStat;

    }
    private void OnDestroy()
    {
        UpdateRequiredForSlot.SatisfyUpdgradeRequired -= AddNewStat;
    }
    void LoadData()
    {
        if (!File.Exists(GetFilePath()))
        {
            CreateFilePath();
        }
        string filePath = Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
        string jsonStat = File.ReadAllText(filePath);

        FormStatBase formStat = JsonUtility.FromJson<FormStatBase>(jsonStat);

        baseAtk = formStat.baseATK;
        Stats.Add("Atk",baseAtk);
        baseHp = formStat.baseHp;
        Stats.Add("Hp",baseHp);
        baseMp = formStat.baseMp;
        Stats.Add("Mp",baseMp);
        baseRegenMp = formStat.baseRegenMp; 
        Stats.Add("RegenMp",baseRegenMp);
        baseAGI = formStat.baseAGI;
        Stats.Add("Agi",baseAGI);

        hpStat = new CharacterStats(baseHp);
        atkStat = new CharacterStats(baseAtk);
        MpStat = new CharacterStats(baseMp);
        RegenMpStat = new CharacterStats(baseRegenMp);
        AgiStat = new CharacterStats(baseAGI);
        if (OnUpgrade != null) OnUpgrade(Stats);
    }
    public void AddNewStat(string nameStat, float statPlus)
    {
        switch(nameStat)
        {
            case "ATK":
                StatsModifier ATK = new StatsModifier(statPlus, StatModType.Flat);
                atkStat.AddModifier(ATK);
                baseAtk = atkStat.Value;
                Stats["Atk"] = baseAtk;
                SaveData();
                break;
            case "HP":
                StatsModifier HP = new StatsModifier(statPlus, StatModType.Flat);
                hpStat.AddModifier(HP);
                baseHp = hpStat.Value;
                Stats["Hp"] = baseHp;
                SaveData();
                break;
            case "MP":
                StatsModifier MP = new StatsModifier(statPlus, StatModType.Flat);
                MpStat.AddModifier(MP);
                baseMp = MpStat.Value;
                Stats["Mp"] = baseMp;
                SaveData();
                break;
            case "RegenMP":
                StatsModifier RegenMP = new StatsModifier(statPlus, StatModType.Flat);
                RegenMpStat.AddModifier(RegenMP);
                baseRegenMp = RegenMpStat.Value;
                Stats["RegenMp"] = baseRegenMp;
                SaveData();
                break;
            case "AGI":
                StatsModifier AGI = new StatsModifier(statPlus, StatModType.Flat);
                AgiStat.AddModifier(AGI);
                baseAGI = AgiStat.Value;
                Stats["Agi"] = baseAGI;
                SaveData();
                break;
            default:
                break;
        }
        //Debug.Log("base = " + baseAtk + " /" + baseHp + " /" + baseMp + " /" + baseRegenMp + " /" + baseAGI);
        if (OnUpgrade != null) OnUpgrade(Stats); 
    }
    private void SaveData()
    {
        FormStatBase dataStat = new FormStatBase();

        dataStat.baseATK = baseAtk;
        dataStat.baseHp = baseHp;
        dataStat.baseMp = baseMp;
        dataStat.baseRegenMp = baseRegenMp;
        dataStat.baseAGI = baseAGI;

        string json = JsonUtility.ToJson(dataStat, true);
        string filePath = Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
        File.WriteAllText(filePath, json);
    }
    private void CreateFilePath()
    {
        FormStatBase formStatBase = new FormStatBase();
        formStatBase.baseATK = baseAtk;
        formStatBase.baseHp = baseHp;
        formStatBase.baseMp = baseMp;
        formStatBase.baseRegenMp = baseRegenMp;
        formStatBase.baseAGI = baseAGI;

        string jsonStatBase = JsonUtility.ToJson(formStatBase, true);
        string filePath = Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
        File.WriteAllText(filePath, jsonStatBase);

    }
}
