using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoss : MonoBehaviour
{
    private float maxHealth;
    private float _health;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }
    private void Start()
    {
        Health = 100f;
    }
    public void TakeDamage(float damage)
    {
        if (_health <= 0)
        {
            return;
        }
        Health -= damage;
    }
}
