using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattle : MonoBehaviour
{
    public GameObject pannelDie;
    void Start()
    {
        HealthManager.CharacterDied += OnPannel;
    }

   void OnPannel()
    {   
        if(pannelDie != null) 
        pannelDie.SetActive(true);
    }
}
