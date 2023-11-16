using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    public Image image;
    public TextMeshProUGUI indexHealth;

    public virtual void Start()
    {
        HealthManager.HealthUpdate += UpdateIndexBar;
        UpdateIndexBar(1f);
    }
    
    public virtual void UpdateIndexBar(float percent)
    {   
        if(percent > 1f ) percent = 1f;else if(percent < 0f) percent = 0f;
        var roundPercent =  Mathf.RoundToInt((percent * 100f)) ;
        if(image != null)
        {
            image.fillAmount = percent;
        }
        if (indexHealth != null)
        {
            indexHealth.text = roundPercent.ToString() + " %";
        }
    }
    private void OnDestroy()
    {
        HealthManager.HealthUpdate -= UpdateIndexBar;
    }
}
