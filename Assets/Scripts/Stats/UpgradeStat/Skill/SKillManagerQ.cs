
using System;
using System.IO;
using UnityEngine;
 
public class SKillManagerQ : SkillManager<FormSkillUpgrade_Q>
{

    private void Awake()
    {
        if (!File.Exists(GetFilePathSkillUpgrade(skillName)))
        {
            CreatFileFormSkillUpgrade();
        }
        LoadSkillUpgradeData();
        Debug.Log("lv = " + lv + " / upgrade = " + upgraded.ToString());
    }
    private void Start()
    {
        LevelSkillQManager.upgradeSkillComplete += UpLevel;
    }
    private void OnDestroy()
    {
        LevelSkillQManager.upgradeSkillComplete -= UpLevel;
    }
}
