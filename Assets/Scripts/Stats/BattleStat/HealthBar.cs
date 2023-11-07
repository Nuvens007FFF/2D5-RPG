using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    public Image image;

    public virtual void Start()
    {
        HealthManager.HealthUpdate += UpdateIndexBar;
        UpdateIndexBar(1f);
    }
    
    public virtual void UpdateIndexBar(float percent)
    {   
        //Debug.Log("PercentHealth = " + percent);
        if(image != null)
        {
            image.fillAmount = percent;
        }
    }
}
