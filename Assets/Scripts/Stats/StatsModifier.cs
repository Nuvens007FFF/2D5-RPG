
public enum StatModType
{
    Flat,
    Percent,
}

public class StatsModifier
{
    public readonly float Value;
    public readonly StatModType Type;

    public StatsModifier(float value, StatModType type)
    {
        Value = value;
        Type = type;
    }
}
