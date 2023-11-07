
using UnityEngine;

public class UiEventBattle : MonoBehaviour
{
    public GameObject pannelSummary;
    private void Start()
    {
        HealthManager.CharacterDied += SwitchPannelSummary;
    }
    void SwitchPannelSummary()
    {
        Debug.Log("SwitchPannelSummary");
        if (pannelSummary != null)
        {
            pannelSummary.gameObject.SetActive(true);
        }
    }
}
