using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/SkillData")]
public class SkillSO : ScriptableObject
{
    public string nameSkill;
    public float damage;
    public GameObject image;
}
