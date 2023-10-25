using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
//using Newtonsoft.Json;

public enum  StatTpye
{
    HP,
    ATK,
    SPD
}

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    public static event Action<Dictionary<string, float>> OnUpgrade;
    
    //private List<float> stats = new List<float>();
    private Dictionary<string,float> Stats = new Dictionary<string,float>();

    public CharacterStats hpStat;
    public CharacterStats atkStat;
    public CharacterStats MpStat;
    public CharacterStats RegenMpStat;
    public CharacterStats AgiStat;



    private int currentCoint;

    private float baseHp;
    private float baseAtk;
    private float baseMp;
    private float baseRegenMp;
    private float baseAGI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {   
        SetBaseStatForCharacter();
        UpdateRequiredForSlot.SatisfyUpdateRequired += UpdateNewStat;

    }

    void SetBaseStatForCharacter()
    {
        string jsonStat = File.ReadAllText(Application.dataPath + " /FormStatBase.Json");

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

    public void UpdateNewStat(string nameStat, float statPlus)
    {   
        switch(nameStat)
        {
            case "ATK":
                StatsModifier ATK = new StatsModifier(statPlus, StatModType.Flat);
                atkStat.AddModifier(ATK);
                baseAtk = atkStat.Value;
                Stats["Atk"] = baseAtk;
                SaveIndexStatToJson();
                break;
            case "HP":
                StatsModifier HP = new StatsModifier(statPlus, StatModType.Flat);
                hpStat.AddModifier(HP);
                baseHp = hpStat.Value;
                Stats["Hp"] = baseHp;
                SaveIndexStatToJson();
                break;
            case "MP":
                StatsModifier MP = new StatsModifier(statPlus, StatModType.Flat);
                MpStat.AddModifier(MP);
                baseMp = MpStat.Value;
                Stats["Mp"] = baseMp;
                SaveIndexStatToJson();
                break;
            case "RegenMP":
                StatsModifier RegenMP = new StatsModifier(statPlus, StatModType.Flat);
                RegenMpStat.AddModifier(RegenMP);
                baseRegenMp = RegenMpStat.Value;
                Stats["RegenMp"] = baseRegenMp;
                SaveIndexStatToJson();
                break;
            case "AGI":
                StatsModifier AGI = new StatsModifier(statPlus, StatModType.Flat);
                AgiStat.AddModifier(AGI);
                baseAGI = AgiStat.Value;
                Stats["Agi"] = baseAGI;
                SaveIndexStatToJson();
                break;
            default:
                break;
        }
        Debug.Log("base = " + baseAtk + " /" + baseHp + " /" + baseMp + " /" + baseRegenMp + " /" + baseAGI);
        if (OnUpgrade != null) OnUpgrade(Stats); 
    }
    private void  SaveIndexStatToJson()
    {
        FormStatBase dataStat = new FormStatBase();

        dataStat.baseATK = baseAtk;
        dataStat.baseHp = baseHp;
        dataStat.baseMp = baseMp;
        dataStat.baseRegenMp = baseRegenMp;
        dataStat.baseAGI = baseAGI;

        string json = JsonUtility.ToJson(dataStat, true);
        File.WriteAllText(Application.dataPath + " /FormStatBase.Json", json);
    }
}
