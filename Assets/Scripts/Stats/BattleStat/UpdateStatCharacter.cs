using UnityEngine;
using System.IO;

public class UpdateStatCharacter : MonoBehaviour
{   
    public static UpdateStatCharacter instance;
    private void  Awake()
    {
        instance = this;
        UpdateStat();
    }
    public float Atk
    {
        get { return _atk; }
        private set { _atk = value; }
    }
    public float Hp
    {
        get { return _hp; }
        private set { _hp = value; }
    }
    public float Mp
    {
        get { return _mp; }
        private set { _mp = value; }
    }
    public float RegenMp
    {
        get { return _regenMp; }
        private set { _regenMp = value; }
    }
    public float Agi
    {
        get { return _agi; }
        private set { _agi = value; }
    }

    private float _atk;
    private float _hp;
    private float _mp;
    private float _regenMp;
    private float _agi;
    private void Start()
    {

    }
    void UpdateStat()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
        string json = File.ReadAllText(filePath);

        FormStatBase formStatBase = JsonUtility.FromJson<FormStatBase>(json);

        Atk = formStatBase.baseATK;
        Hp = formStatBase.baseHp;
        Mp = formStatBase.baseMp;
        RegenMp = formStatBase.baseRegenMp;
        Agi = formStatBase.baseAGI;

        Debug.Log("base = " + Atk + " /" + Hp + " /" + Mp + " /" + RegenMp + " /" + Agi);
    }
}
