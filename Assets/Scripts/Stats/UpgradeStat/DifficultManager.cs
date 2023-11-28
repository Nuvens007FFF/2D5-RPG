using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class DifficultManager : MonoBehaviour
{
    public GameObject TabDifficutlt;
    public GameObject TabStat;
    public Text text;
    
    private int count;
    private int difficultID;
    private SwitchScene switchScene;
    private GameObject buttonStart;

    private void Start()
    {
        buttonStart = GameObject.Find("Button_Start");
        switchScene = FindObjectOfType<SwitchScene>();
        TabDifficutlt.SetActive(false);
        if (!File.Exists(GetPathFile()))
        {
            CreateFile();
        }
        else LoadFile();
    }
    void SaveFile()
    {
        CreateFile();
    }
    void CreateFile()
    {
        FormDifficult formDif = new FormDifficult { difficultLevel = difficultID };
        string jsonCoin = JsonUtility.ToJson(formDif, true);
        string filePath = GetPathFile();
        File.WriteAllText(filePath, jsonCoin);
    }
    void LoadFile()
    {
        if (File.Exists(GetPathFile()))
        {
            string formDif = File.ReadAllText(GetPathFile());
            FormDifficult formDifficult = JsonUtility.FromJson<FormDifficult>(formDif);
            difficultID = formDifficult.difficultLevel;
        }
    }
    public void Show()
    {   
        if(count == 0)
        {
            TabDifficutlt.SetActive(true);
            TabStat.SetActive(false);
            //text.text = "Bắt đầu";
            //count++;
        }else if(count == 1)
        {
            switchScene.LoadScene(2);
        }
    }
    public void StartBattle()
    {
        switchScene.LoadScene(2);
    }
     public void ChooseDifficult(int difficult)
    {
        Debug.Log("difficult = " + difficult);
        if (difficult == 1)
        {
            Debug.Log("easy");
            difficultID = difficult;
            SaveFile();
        }else if(difficult == 2)
        {
            Debug.Log("normal");
            difficultID = difficult;
            SaveFile();
        }
        else
        {
            return ;
        }
    }
    string GetPathFile()
    {
        return Path.Combine(Application.persistentDataPath, "FormDifficult.Json");
    }
}
