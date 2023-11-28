using System;

[System.Serializable]
public class FormStatBase 
{
    //Base Stat
    public float baseATK;
    public float baseHp;
    public float baseMp;
    public float baseRegenMp;
    public float baseAGI;
}
[System.Serializable]
public class FormDifficult
{
    public int difficultLevel;
}
[System.Serializable]
public class FormStore
{   
    public float potionCost;
    public float manaItemCost;
}
[System.Serializable]
public class FormInventory
{
    public float potionCount;
    public float manaItemCount;
    public int maxPotion;
    public int maxManaItem;
}
[System.Serializable]
public class FormCoin
{
    public float coinID;
}

[System.Serializable]
public class SkillUpgradeBase
{
    public int skillLevel;
    public bool Upgraded;
}

[System.Serializable]
public class FormSkillUpgrade_Q : SkillUpgradeBase { }

[System.Serializable]
public class FormSkillUpgrade_W : SkillUpgradeBase { }

[System.Serializable]
public class FormSkillUpgrade_E : SkillUpgradeBase { }

[System.Serializable]
public class FormSkillUpgrade_R : SkillUpgradeBase { }

    [System.Serializable]
public class BaseStat
{
    public float coinRequired;
    public float maxStat;
    public float statPlus;
    public float currentStatPlus;
    public float percentRise;
    public int countMaxPrice;
}
[System.Serializable]
public class ATK : BaseStat { }

[System.Serializable]
public class HP : BaseStat { }

[System.Serializable]
public class MP : BaseStat { }

[System.Serializable]
public class RegenMP : BaseStat { }

[System.Serializable]
public class AGI : BaseStat { }
