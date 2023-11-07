
using System;
using System.Collections.Generic;

public class CharacterStats
{   
    public float BaseValue;

    public float Value 
    {
        get 
        {
            if (isDirty)
            {
                return CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        } 
    }

    private bool isDirty = true;
    private float _value;

    private readonly List<StatsModifier> statModifiers;

    public CharacterStats(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatsModifier>();
    }

    public void AddModifier(StatsModifier modifier)
    {
        isDirty = true;
        statModifiers.Add(modifier);
    }
    
    public bool RemoveModifier(StatsModifier modifier)
    {   
        isDirty = true;
        return statModifiers.Remove(modifier);
    }

    public float CalculateFinalValue()
    {
        var finalValue = BaseValue;

        for (int i = 0; i < statModifiers.Count ; i++)
        {
            StatsModifier mod = statModifiers[i];

            if (mod.Type == StatModType.Flat)
            {
                finalValue += statModifiers[i].Value;
            }
            else if (mod.Type == StatModType.Percent)
            {
                finalValue *= 1 + mod.Value; // base Value = 10 increase by 10% --> 10 * 1.1 = 11
            }
        }

        return (float)Math.Round(finalValue,4);
    }
}
