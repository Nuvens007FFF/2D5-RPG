using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UiEventBattle : MonoBehaviour
{
    public static event Action animationCoinEvent;
    public GameObject pannelSummary;
    public TextMeshProUGUI coinIndex;

    public AudioClip VictoryTheme;
    public AudioClip LoseTheme;

    private void Start()
    {
        HealthManager.CharacterDied += HandlePlayerDefeated;
        CoinSystem.CoinUpdatedUI += CoinTextUpdate;
        BossController.BossDefeated += HandleBossDefeated;
    }

    void HandlePlayerDefeated()
    {
        Debug.Log("SwitchPannelSummary");
        UpdateSummaryText("Thất Bại");
        AudioManager.instance.PlayMusic(LoseTheme, 1f);
        StartCoroutine(OpenSummary(2f));
    }

    private void HandleBossDefeated()
    {
        // Handle actions when the boss is defeated
        Debug.Log("Boss is defeated!");
        UpdateSummaryText("Chiến Thắng");
        AudioManager.instance.PlayMusic(VictoryTheme, 1f);
        StartCoroutine(OpenSummary(7f));
    }

    private IEnumerator OpenSummary(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (pannelSummary != null)
        {
            pannelSummary.gameObject.SetActive(true);
        }
    }

    void CoinTextUpdate(float coinInBattle)
    {
        coinIndex.text = coinInBattle.ToString();
        if (animationCoinEvent != null) { animationCoinEvent(); }
    }

    private void UpdateSummaryText(string text)
    {
        // Assuming you have a TextMeshProUGUI component on the pannelSummary object
        TextMeshProUGUI summaryText = pannelSummary.GetComponentInChildren<TextMeshProUGUI>();
        if (summaryText != null)
        {
            summaryText.text = text;
        }
    }

    private void OnDestroy()
    {
        HealthManager.CharacterDied -= HandlePlayerDefeated;
        CoinSystem.CoinUpdatedUI -= CoinTextUpdate;
        BossController.BossDefeated -= HandleBossDefeated;
    }
}
