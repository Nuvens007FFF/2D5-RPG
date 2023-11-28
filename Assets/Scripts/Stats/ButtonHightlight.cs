
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonHightlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MouseText mouseText;

    private void OnValidate()
    {
        mouseText = FindObjectOfType<MouseText>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {   
        ShowText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideText();
    }
    void ShowText()
    {   
        var name = gameObject.name;
        var position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,transform.position.z);
        mouseText.ShowInfor(name);
    }
    void HideText()
    {
        mouseText.HidePannel();
    }
}
