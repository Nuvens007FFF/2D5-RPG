using System.IO;
using UnityEngine;

public class SkillManagerW : SkillManager<FormSkillUpgrade_W>
{
    private void Awake()
    {
        if (!File.Exists(GetFilePathSkillUpgrade(skillName)))
        {
            CreatFileFormSkillUpgrade();
        }
        LoadSkillUpgradeData();
        Debug.Log("lv_W = " + lv + " / upgrade_W = " + upgraded.ToString());
    }
    private void Start()
    {
        LevelSkillWManager.upgradeSkillComplete_W += UpLevel;
    }
    private void OnDestroy()
    {
        LevelSkillWManager.upgradeSkillComplete_W -= UpLevel;
    }
}