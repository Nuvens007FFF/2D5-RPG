using UnityEngine;
using System.IO;

public class UpdateStatCharacter : MonoBehaviour
{   
    public static UpdateStatCharacter instance;
    private void  Awake()
    {
        instance = this;
        LoadDataStat();
        LoadDataLevelSkill(); 
        LoadDifficult();
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
    public float CoolDownQ
    {
        get { return coolDownQ; }
        private set { coolDownQ = value;}
    }
    public float CoolDownE
    {
        get { return coolDownE; }
        private set { coolDownE = value; }
    }  
    public float DurationW
    {
        get { return durationW; }
        private set { durationW = value; }
    }
    public bool UnLockSkillR
    {
        get { return _unlockSkillR; }
        private set { _unlockSkillR = value;}
    }
    public int Difficult
    {
        get { return _difficult; }
        private set { _difficult = value; }
    }

    private int _difficult;

    private float _atk;
    private float _hp;
    private float _mp;
    private float _regenMp;
    private float _agi;

    private int level_Q;
    private int level_W;
    private int level_E;
    private int level_R;
    private bool _unlockSkillR;

    private float coolDownQ= 4.5f;
    private float coolDownE = 4.5f;
    private float durationW = 5f;
    
    void LoadDifficult()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "FormDifficult.Json");
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            FormDifficult formDifficult = JsonUtility.FromJson<FormDifficult>(json);

            Difficult = formDifficult.difficultLevel;
        }
    }

    void LoadDataStat()
    {   
        string filePath = Path.Combine(Application.persistentDataPath, "FormStatBase.Json");
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            FormStatBase formStatBase = JsonUtility.FromJson<FormStatBase>(json);

            Atk = formStatBase.baseATK;
            Hp = formStatBase.baseHp;
            Mp = formStatBase.baseMp;
            RegenMp = formStatBase.baseRegenMp;
            Agi = formStatBase.baseAGI;

            //Debug.Log("base = " + Atk + " /" + Hp + " /" + Mp + " /" + RegenMp + " /" + Agi);
        }
    }
    void LoadDataLevelSkill()
    {
        string QSkillFilePath = Path.Combine(Application.persistentDataPath, "FormSkill_Q.Json");
        string WSkillFilePath = Path.Combine(Application.persistentDataPath, "FormSkill_W.Json");
        string ESkillFilePath = Path.Combine(Application.persistentDataPath, "FormSkill_E.Json");
        string RSkillFilePath = Path.Combine(Application.persistentDataPath, "FormSkill_R.Json");
        if(File.Exists (QSkillFilePath) && File.Exists(WSkillFilePath) && File.Exists(ESkillFilePath) && File.Exists(RSkillFilePath))
        {
            string jsonQ = File.ReadAllText(QSkillFilePath);
            string jsonW = File.ReadAllText(WSkillFilePath);
            string jsonE = File.ReadAllText(ESkillFilePath);
            string jsonR = File.ReadAllText(RSkillFilePath);

            FormSkillUpgrade_Q formSkillUpgrade_Q = JsonUtility.FromJson<FormSkillUpgrade_Q>(jsonQ);
            FormSkillUpgrade_W formSkillUpgrade_W = JsonUtility.FromJson<FormSkillUpgrade_W>(jsonW);
            FormSkillUpgrade_E formSkillUpgrade_E = JsonUtility.FromJson<FormSkillUpgrade_E>(jsonE);
            FormSkillUpgrade_R formSkillUpgrade_R = JsonUtility.FromJson<FormSkillUpgrade_R>(jsonR);

            level_Q = formSkillUpgrade_Q.skillLevel;
            level_W = formSkillUpgrade_W.skillLevel;
            level_E = formSkillUpgrade_E.skillLevel;
            level_R = formSkillUpgrade_R.skillLevel;
            UnLockSkillR = formSkillUpgrade_R.Upgraded;

            Debug.Log(level_Q + "/" + level_W + "/" + level_E + "/" + UnLockSkillR);
            ModifierSkill(level_Q, level_W, level_E);
        }

    }
    void ModifierSkill(int levelQ, int levelE, int levelW)
    {
        CoolDownQ -= levelQ;
        CoolDownE -= levelE;
        DurationW += levelW;
    }
}
