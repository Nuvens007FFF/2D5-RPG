using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class UiStatManager : MonoBehaviour
{   

    [SerializeField] private TMP_Text Atk;
    [SerializeField] private TMP_Text Hp;
    [SerializeField] private TMP_Text Mp;
    [SerializeField] private TMP_Text RegenMp;
    [SerializeField] private TMP_Text Agi;
    [Space]
    [SerializeField] private TMP_Text Coin;

    private float atkIndexLast;
    private float hpIndexLast;
    private float mpIndexLast;
    private float regenMpIndexLast;
    private float agiIndexLast;
    private void Awake()
    {
        CoinSystem.CoinUpdated += CoinUpdatedListener;
        StatManager.OnUpgrade += UpdateUiStat;
    }
    private void Start()
    {
        
    }

    public void UpdateUiStat(Dictionary<string,float> stats)
    {
        string[] statNames = { "Atk", "Hp", "Mp", "RegenMp", "Agi" };

        for (int i = 0; i < statNames.Length; i++)
        {
            string statName = statNames[i];
            float statValue;

            if (stats.TryGetValue(statName, out statValue))
            {
                switch (statName)
                {
                    case "Atk":
                        Atk.text = statValue.ToString();
                        if (atkIndexLast != statValue) 
                        {
                            AniamtionIndexUpgrade(Atk);
                            atkIndexLast = statValue;
                        }
                        break;
                    case "Hp":
                        Hp.text = statValue.ToString();
                        if (hpIndexLast != statValue)
                        {
                            AniamtionIndexUpgrade(Hp);
                            hpIndexLast = statValue;
                        }
                        break;
                    case "Mp":
                        Mp.text = statValue.ToString();
                        if (mpIndexLast != statValue)
                        {
                            AniamtionIndexUpgrade(Mp);
                            mpIndexLast = statValue;
                        }
                        break;
                    case "RegenMp":
                        RegenMp.text = statValue.ToString();
                        if (regenMpIndexLast != statValue)
                        {
                            AniamtionIndexUpgrade(RegenMp);
                            regenMpIndexLast = statValue;
                        }
                        break;
                    case "Agi":
                        Agi.text = statValue.ToString();
                        if (agiIndexLast != statValue)
                        {
                            AniamtionIndexUpgrade(Agi);
                            agiIndexLast = statValue;
                        }
                        break;
                }
            }
        }
    }
    private void CoinUpdatedListener(float newCoinValue)
    {
        Coin.text = newCoinValue.ToString();
    }
    void AniamtionIndexUpgrade(TMP_Text name)
    {
        name.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.25f)
            .SetEase(Ease.OutQuad) 
            .OnComplete(() =>
            {
                name.transform.DOScale(Vector3.one, 0.25f)
                    .SetEase(Ease.InQuad); 
            });
        name.DOColor(Color.yellow, 0.25f)
           .SetEase(Ease.OutQuad)
           .OnComplete(() =>
           {
               name.DOColor(Color.white, 0.25f)
                   .SetEase(Ease.InQuad);
           });
    }
    private void OnDestroy()
    {
        CoinSystem.CoinUpdated -= CoinUpdatedListener;
        StatManager.OnUpgrade -= UpdateUiStat;
    }
}




