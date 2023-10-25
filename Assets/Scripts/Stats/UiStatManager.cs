using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiStatManager : MonoBehaviour
{   

    [SerializeField] private TMP_Text Atk;
    [SerializeField] private TMP_Text Hp;
    [SerializeField] private TMP_Text Mp;
    [SerializeField] private TMP_Text RegenMp;
    [SerializeField] private TMP_Text Agi;
    [Space]
    [SerializeField] private TMP_Text Coin;
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
                        break;
                    case "Hp":
                        Hp.text = statValue.ToString();
                        break;
                    case "Mp":
                        Mp.text = statValue.ToString();
                        break;
                    case "RegenMp":
                        RegenMp.text = statValue.ToString();
                        break;
                    case "Agi":
                        Agi.text = statValue.ToString();
                        break;
                }
            }
        }
    }
    private void CoinUpdatedListener(float newCoinValue)
    {
        Coin.text = newCoinValue.ToString();
    }
}
