
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : HealthBar
{
    public override void Start()
    {
        ManaManager.UpdateManaBar += UpdateIndexBar;
        UpdateIndexBar(1f);
    }
    public override  void UpdateIndexBar(float percent)
    {
        Debug.Log("PercentMana = " + percent);
        base.UpdateIndexBar(percent);
    }
}
