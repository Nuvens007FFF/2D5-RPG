using System.IO;
using UnityEngine;

public class SkillManagerR : SkillManager<FormSkillUpgrade_R>
{
    private void Awake()
    {
        if (!File.Exists(GetFilePathSkillUpgrade(skillName)))
        {
            CreatFileFormSkillUpgrade();
        }
        LoadSkillUpgradeData();
    }
    private void Start()
    {
        LevelSkillRManager.upgradeSkillComplete_R += UpLevel;
    }
    private void OnDestroy()
    {
        LevelSkillRManager.upgradeSkillComplete_R -= UpLevel;
    }
}
