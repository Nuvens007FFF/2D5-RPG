
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
        //Debug.Log("MaxHealth = " + maxHealth + " / " + "Health = " + Health);
    }

    public void TakeDamage(float damage)
    {
        if (Health <= 0) 
        {  
            if(CharacterDied != null) { CharacterDied(); }
            return; 
        }
        Health -= damage;
        //Debug.Log("Health = " + Health);
        CalculatePercent();
    }

    private void CalculatePercent()
    {
        float percent = (Health / maxHealth);
        if (HealthUpdate != null) { HealthUpdate(percent);}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5f);
        }
    }
}
