using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIBattleItem : MonoBehaviour
{
    public TextMeshProUGUI potion;
    public TextMeshProUGUI manaItem;

    void Start()
    {
        ItemBattle.UsePotionEvent += UsePotionItem;
        ItemBattle.UseManaEvent += UseManaItem;
    }
    private void OnDestroy()
    {
        ItemBattle.UsePotionEvent -= UsePotionItem;
        ItemBattle.UseManaEvent -= UseManaItem;
    }
    void UsePotionItem(float potionAmount)
    {
        potion.text = potionAmount.ToString();
    }
    void UseManaItem(float manaAmount)
    {
        manaItem.text = manaAmount.ToString();
    }

}
