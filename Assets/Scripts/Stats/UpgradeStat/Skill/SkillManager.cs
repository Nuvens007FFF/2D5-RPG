
using System.IO;
using UnityEngine;
public class SkillManager<T> : MonoBehaviour where T : SkillUpgradeBase, new()
{
    public string skillName;
    public int lv = 0;
    public bool upgraded;

    protected virtual string GetFilePathSkillUpgrade(string skillNam)
    {
        return Path.Combine(Application.persistentDataPath, $"{skillName}.Json");
    }
    public virtual void CreatFileFormSkillUpgrade()
    {
        T formSkillUpgrade = new T
        {
            skillLevel = lv,
            Upgraded = upgraded
        };

        string jsonSkill = JsonUtility.ToJson(formSkillUpgrade, true);
        string filePath = GetFilePathSkillUpgrade(skillName);
        File.WriteAllText(filePath, jsonSkill);
    }
    public virtual void LoadSkillUpgradeData()
    {
        string filePath = GetFilePathSkillUpgrade(skillName);
        if (File.Exists(filePath))
        {
            string jsonUpgrade = File.ReadAllText(filePath);
            T formSkillUpgrade = JsonUtility.FromJson<T>(jsonUpgrade);
            lv = formSkillUpgrade.skillLevel;
            upgraded = formSkillUpgrade.Upgraded;
        }
    }
    public virtual void SaveSkillUpgradeSkill()
    {
        T formSkillUpgradeSkill = new T
        {
            skillLevel = lv,
            Upgraded = upgraded
        };

        string jsonUpgrade = JsonUtility.ToJson(formSkillUpgradeSkill, true);
        File.WriteAllText(GetFilePathSkillUpgrade(skillName), jsonUpgrade);

    }
    public virtual void UpLevel()
    {
        lv++;
        upgraded = true;
        SaveSkillUpgradeSkill();
    }
}
