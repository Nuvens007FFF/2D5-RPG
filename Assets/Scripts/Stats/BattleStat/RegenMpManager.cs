using System;
using UnityEngine;

public class RegenMpManager : MonoBehaviour
{
    public static event Action<float> UpdateRegenMp;

    private float _regenMp;

    private float currentTime;
    private float timeRegenMp = 1f;
    private bool isDied;

    private void Start()
    {
        _regenMp = UpdateStatCharacter.instance.RegenMp;
        //Debug.Log("RegenMp = " + _regenMp);
        currentTime = timeRegenMp;
        HealthManager.CharacterDied += CharacterDied;
    }
    private void CharacterDied()
    {
        isDied = true;
    }

    private void Update()
    {
        if (isDied) return;
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            if (UpdateRegenMp != null) { UpdateRegenMp(_regenMp); }
            currentTime = timeRegenMp;
        }
    }
}
