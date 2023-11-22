using System.IO;
using UnityEngine;

public class SkillManagerE : SkillManager<FormSkillUpgrade_E>
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
        LevelSkillEManager.upgradeSkillComplete_E += UpLevel;
    }
    private void OnDestroy()
    {
        LevelSkillEManager.upgradeSkillComplete_E -= UpLevel;
    }
}