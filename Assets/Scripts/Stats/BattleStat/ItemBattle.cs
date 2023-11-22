using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ItemBattle : MonoBehaviour
{
    public static event Action<float> UsePotionEvent;
    public static event Action<float> UseManaEvent;

    [SerializeField] private float _potionAmount ;
    [SerializeField] private float _manaAmount ;
    [SerializeField] private int _maxMana ;
    [SerializeField] private int _maxPotion ;

    private void Start()
    {
        FileManager();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){UsePotion();}
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { UseManaItem(); }
    }
    void FileManager()
    {
        if (!File.Exists(GetFilePath()))
        {
            return;
        }
        LoadFile();
    }
    private void LoadFile()
    {
        if (File.Exists(GetFilePath()))
        {
            string jsonInventory = File.ReadAllText(GetFilePath());
            FormInventory formInventory = JsonUtility.FromJson<FormInventory>(jsonInventory);
            _potionAmount = formInventory.potionCount;
            _manaAmount = formInventory.manaItemCount;
            _maxPotion = formInventory.maxPotion;
            _maxMana = formInventory.maxManaItem;
        }
        if (UsePotionEvent != null) UsePotionEvent(_potionAmount);
        if (UseManaEvent != null) UseManaEvent(_manaAmount);
    }
    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "FormInventory.Json");
    }
    public void UsePotion()
    {
        if (_potionAmount <= 0) { return; }
        _potionAmount--;
        if(UsePotionEvent != null) UsePotionEvent(_potionAmount);    
        SaveFile();
    }
    public void UseManaItem()
    {
        if (_manaAmount <= 0) { return; }
        _manaAmount--;
        if (UseManaEvent != null) UseManaEvent(_manaAmount);
        SaveFile();
    }
    private void SaveFile()
    {
        FormInventory formInventory = new FormInventory
        {
            potionCount = _potionAmount,
            manaItemCount = _manaAmount,
            maxManaItem = _maxMana,
            maxPotion = _maxPotion
        };
        string jsonInventory = JsonUtility.ToJson(formInventory, true);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, jsonInventory);
    }
}
