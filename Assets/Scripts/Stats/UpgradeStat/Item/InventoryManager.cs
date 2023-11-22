using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static event Action<string> MessageEvent;

    [SerializeField] private float _potionAmount = 0;
    [SerializeField] private float _manaAmount = 0;
    public int _maxMana = 3;
    public int _maxPotion = 3;

    public Image[] items; 
    private void Start()
    {
        GetComponentStart();
        FileManager();
        LoadItemsOwned(_potionAmount, _manaAmount);

        StoreManager.OnBuying += AddItem;
    }
    void GetComponentStart()
    {
        items = GetComponentsInChildren<Image>().Where(img => img.gameObject.name == "item").ToArray();
    }
    void FileManager()
    {
        if (!File.Exists(GetFilePath()))
        {
            CreateFileAndSaveData();
        }
        LoadFile();
    }
    private void OnDestroy()
    {
       StoreManager.OnBuying -= AddItem;
    }
    private void LoadItemsOwned(float potionAmount, float manaAmount)
    {
        if (potionAmount == 0 && manaAmount == 0) { return; }
        if (potionAmount != 0)
        {
            for (int i = 0; i < potionAmount; i++)
            {
                TakeItem("Potion");
            }
        }
        if(manaAmount != 0)
        {
            for (int i = 0; i < manaAmount; i++)
            {
                TakeItem("ManaItem");
            }
        }
    }
    private void AddItem(string name, float cost)
    {
       // Debug.Log("nameItem = " + name);
        if(name == "Potion")
        {   
            if(_potionAmount >= _maxPotion)
            {
                var message = "Bạn chỉ mua tối đa được 3 bình máu";
                SendMessageText(message);   
                return ;
            }
            else
            {
                CoinSystem.instance.GetCoin(cost);
                TakeItem(name);
                _potionAmount++;
                //Debug.Log("Inventory add potion have " + _potionAmount );
                SaveFile();
            }
        }else if(name == "ManaItem")
        {
            if (_manaAmount >= _maxMana)
            {
                var  message = "Bạn chỉ mua tối đa được 3 bình mana";
                SendMessageText(message);
                return;
            }
            else
            {
                CoinSystem.instance.GetCoin(cost);
                TakeItem(name);
                _manaAmount++;
               // Debug.Log("Inventory add potion have " + _manaAmount);
                SaveFile();
            }
        }
    }
    void SendMessageText(string message)
    {   
        if (MessageEvent != null) MessageEvent(message);

    }
    private void TakeItem(string name)
    {   
        if(items != null)
        {
            Sprite filePathItem = Resources.Load<Sprite>("UI/Item/" + name);
            if(filePathItem != null)
            {
                var image =  GetSlotEmty();
                image.sprite = filePathItem;
                //Set color
                Color currentColor = image.color;
                Color newColorAlpha = new Color(currentColor.r, currentColor.b, currentColor.g, currentColor.a * 255f);
                image.color = newColorAlpha;
            }
        }
    }
    private Image GetSlotEmty()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].sprite == null)
            {
                return items[i];
            }
        }
        return null;
    }
    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "FormInventory.Json");
    }
    private void CreateFileAndSaveData()
    {
        FormInventory formInventory = new FormInventory
        {
             potionCount = _potionAmount,
            manaItemCount = _manaAmount,
            maxManaItem = _maxMana,
            maxPotion   =  _maxPotion,
        };
        string jsonInventory = JsonUtility.ToJson(formInventory, true);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, jsonInventory);
    }
    private void LoadFile()
    {
        if (File.Exists(GetFilePath()))
        {
            string jsonInventory = File.ReadAllText(GetFilePath());
            FormInventory formInventory = JsonUtility.FromJson<FormInventory>(jsonInventory);
            _potionAmount = formInventory.potionCount;
            _manaAmount = formInventory.manaItemCount;
            _maxMana = formInventory.maxManaItem;
            _maxPotion = formInventory.maxPotion;

            //Debug.Log()
        }
    }
    private void SaveFile()
    {
        CreateFileAndSaveData();
    }
}
