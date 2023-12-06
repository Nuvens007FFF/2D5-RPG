
using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static event Action<float> HealthUpdate;
    public static event Action CharacterDied;

    private float maxHealth;
    private float _health;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    private void Start()
    {
        Health = UpdateStatCharacter.instance.Hp * 10f;
        maxHealth = Health;
        ItemBattle.UsePotionEvent += HealFromPotion;
        //Debug.Log("MaxHealth = " + maxHealth + " / " + "Health = " + Health);
    }
    private void OnDestroy()
    {
        ItemBattle.UsePotionEvent -= HealFromPotion;
    }
    public void HealFromPotion(float Amount)
    {
        if(Amount < 0) return;
        Health = maxHealth;
        CalculatePercent();
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        CalculatePercent();
        if (Health <= 0)
        {
            if (CharacterDied != null) { CharacterDied(); }
            return;
        }
        //Debug.Log("Health = " + Health);
    }

    private void CalculatePercent()
    {
        float percent = (Health / maxHealth);
        if (HealthUpdate != null) { HealthUpdate(percent);}
    }

    private void Update()
    {

    }
}
