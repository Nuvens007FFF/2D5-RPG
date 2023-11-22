
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiSkillTree : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
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
        text.text = "co the nang cap";
    }
    void HideText()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
