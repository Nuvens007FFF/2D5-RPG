using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemBattle : MonoBehaviour
{
    public static event Action<float> UsePotionEvent;
    public static event Action<float> UseManaEvent;

    [SerializeField] private float _potionAmount;
    [SerializeField] private float _manaAmount;
    [SerializeField] private int _maxMana;
    [SerializeField] private int _maxPotion;
    [SerializeField] private float _potionCooldown = 5f; // Cooldown time for potions in seconds
    private float _lastHPPotionUseTime; // Timestamp of the last potion use
    private float _lastMPPotionUseTime;

    public Button item1;
    public Button item2;

    public Text item1Text;
    public Text item2Text;
    public Text item1CDText;
    public Text item2CDText;

    private void Start()
    {
        FileManager();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _potionAmount > 0 && Time.time - _lastHPPotionUseTime >= _potionCooldown) 
        {
            UsePotion();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && _manaAmount > 0f && Time.time - _lastMPPotionUseTime >= _potionCooldown) 
        { 
            UseManaItem(); 
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        item1.interactable = _potionAmount > 0f && Time.time - _lastHPPotionUseTime >= _potionCooldown;
        item2.interactable = _manaAmount > 0f && Time.time - _lastMPPotionUseTime >= _potionCooldown;

        item1Text.text = "" + _potionAmount;
        item2Text.text = "" + _manaAmount;

        item1CDText.text = (_potionCooldown - (Time.time - _lastHPPotionUseTime)) > 0f ? (_potionCooldown - (Time.time - _lastHPPotionUseTime)).ToString("F1") : ""; 
        item2CDText.text = (_potionCooldown - (Time.time - _lastMPPotionUseTime)) > 0f ? (_potionCooldown - (Time.time - _lastMPPotionUseTime)).ToString("F1") : "";
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
        _potionAmount--;
        _lastHPPotionUseTime = Time.time; // Update the last potion use time
        if (UsePotionEvent != null) UsePotionEvent(_potionAmount);
        SaveFile();
    }

    public void UseManaItem()
    {
        if (_manaAmount <= 0) { return; }
        _manaAmount--;
        _lastMPPotionUseTime = Time.time; 
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
