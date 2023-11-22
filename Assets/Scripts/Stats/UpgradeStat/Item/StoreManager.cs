using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class StoreManager : MonoBehaviour
{
    const string _POTION = "Potion";
    const string _MANA_ITEM = "ManaItem";

    public static event Action< string, float > OnBuying; 
    public static event Action<string> MessageStoreEvent;

    [SerializeField] private float _manaItemCost = 500f;
    [SerializeField] private float _potionCost = 500f;

    public Image[] potions;
    public Image[] manaItems;

    private void Start()
    {   
        potions = GetComponentsInChildren<Image>().Where(img => img.gameObject.name == "Potion").ToArray() ;
        manaItems = GetComponentsInChildren<Image>().Where(img => img.gameObject.name == "ManaItem").ToArray();
        if (!File.Exists(GetFilePath()))
        {
            CreateFileAndSaveData();
        }
        LoadFile();
        LoadItemUpStore();
    }
    public void OnClickBuying(string item)
    {
        if(item == _POTION)
        {
            var enoughCoin = CoinSystem.instance.CheckCoin(_potionCost);
            if (enoughCoin)
            {
                if (OnBuying != null) OnBuying( _POTION, _potionCost);
            }
            else
            {
                var message = "Bạn không đủ tiền để mua vật phẩm này !";
                SendMessageEvent(message);
            }
        }
        else if(item == _MANA_ITEM)
        {
            var enoughtCoin = CoinSystem.instance.CheckCoin(_manaItemCost);
            if (enoughtCoin)
            {
                if (OnBuying != null) OnBuying(_MANA_ITEM, _manaItemCost);
            }
            else
            {
                var message = "Bạn không đủ tiền để mua vật phẩm này !";
                SendMessageEvent(message);
            }
        }
    }
    private void SendMessageEvent(string message)
    {
        if (MessageStoreEvent != null) MessageStoreEvent(message);
    }
    void LoadItemUpStore()
    {
        if (potions != null)
        {
            Sprite filePathPotion  = Resources.Load<Sprite>("UI/Item/Potion");
            for (int i = 0; i < potions.Length; i++)
            {
                if (filePathPotion != null)
                {
                    potions[i].sprite = filePathPotion;
                    SetColor(potions[i]);
                }
            }
        } if(manaItems != null)
        {
            Sprite filePathMana = Resources.Load<Sprite>("UI/Item/ManaItem");
            for (int i = 0; i < manaItems.Length; i++)
            {
                if (filePathMana != null)
                {
                    manaItems[i].sprite = filePathMana;
                    SetColor(potions[i]);
                }
            }
        }
    }
    void SetColor(Image image)
    {
        Color currentColor = image.color;
        Color newColorAlpha = new Color(currentColor.r, currentColor.b, currentColor.g, currentColor.a * 255f);
        image.color = newColorAlpha;
    }
    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "FormStore.Json");
    }
    private void CreateFileAndSaveData()
    {
        FormStore formStore = new FormStore
        {
            manaItemCost = _manaItemCost, potionCost = _potionCost
        };
        string jsonStore = JsonUtility.ToJson(formStore, true);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, jsonStore);
    }
    private void LoadFile()
    {
        if (File.Exists(GetFilePath()))
        {
            string jsonStore = File.ReadAllText(GetFilePath());
            FormStore formStore = JsonUtility.FromJson<FormStore>(jsonStore);   
            _manaItemCost = formStore.manaItemCost;
            _potionCost= formStore.potionCost;
        }
    }
    private void SaveFile()
    {
        CreateFileAndSaveData();
    }

}
