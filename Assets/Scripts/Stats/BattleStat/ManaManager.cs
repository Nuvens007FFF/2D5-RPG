using System;
using System.Reflection;
using UnityEngine;

public class ManaManager : HealthManager
{
    public static event Action<float> UpdateManaBar;

    private float maxMana;
    private float _mana;
    public float Mana
    {
        get { return _mana; }
        set { _mana = value; }
    }
    private void Start()
    {
        Mana = UpdateStatCharacter.instance.Mp;
        maxMana = Mana;
        RegenMpManager.UpdateRegenMp += PlusMana;
        
        //Debug.Log("MaxMana = " + maxMana + " / " + "Mana = " + Mana);
    }

    public  void MinusMana(float manaIndex)
    {
        if (Mana <= 0) { return; }
        Mana -= manaIndex;
       // Debug.Log("ManaMinus = " + Mana);
        CalculatePercent();
    }
    public void PlusMana(float manaIndex)
    {   
        if(Mana >= maxMana) { Mana = maxMana; return; }
        Mana += manaIndex;
       // Debug.Log("ManaPlus = " + Mana);
        CalculatePercent();
    }

    private void CalculatePercent()
    {
        float percent =  (Mana / maxMana);
        if (UpdateManaBar != null) { UpdateManaBar(percent); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MinusMana(5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlusMana(3f);
        }
    }
}
   
