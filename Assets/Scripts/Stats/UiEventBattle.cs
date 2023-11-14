
using System;
using TMPro;
using UnityEngine;

public class UiEventBattle : MonoBehaviour
{
    public static event Action animationCoinEvent;
    public GameObject pannelSummary;
    public TextMeshProUGUI  coinIndex;
    private void Start()
    {
        HealthManager.CharacterDied += SwitchPannelSummary;
        CoinSystem.CoinUpdatedUI += CoinTextUpdate;
    }
    void SwitchPannelSummary()
    {
        Debug.Log("SwitchPannelSummary");
        if (pannelSummary != null)
        {
            pannelSummary.gameObject.SetActive(true);
        }
    }
    void CoinTextUpdate(float coinInBattle)
    {
        coinIndex.text = coinInBattle.ToString();
        if(animationCoinEvent != null) { animationCoinEvent(); }
    }
    private void OnDestroy()
    {
        HealthManager.CharacterDied -= SwitchPannelSummary;
        CoinSystem.CoinUpdatedUI -= CoinTextUpdate;
    }
}
