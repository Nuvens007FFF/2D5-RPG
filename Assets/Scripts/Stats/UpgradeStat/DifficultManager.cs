using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class DifficultManager : MonoBehaviour
{
    public GameObject TabDifficutlt;
    public GameObject TabStat;
    public Text text;
    public Text diffInfo;

    private int count;
    private int difficultID;
    private SwitchScene switchScene;
    private GameObject buttonStart;
    public GameObject buttonrealStart;

    public Button normalMode;
    public Button easyMode;

    private void Start()
    {
        buttonStart = GameObject.Find("Button_Start");
        buttonrealStart.GetComponent<Button>().interactable = false;
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
            SetButtonState();
        }
    }
    void SetButtonState()
    {
        // Set the interactable state of buttons based on difficultID
        if (difficultID == 2)
        {
            ChangeButtonColor(normalMode, Color.yellow);
            ChangeButtonColor(easyMode, Color.white);
            diffInfo.text = "Niên Thú ở trạng thái mạnh nhất";
        }
        if (difficultID == 1)
        {
            ChangeButtonColor(easyMode, Color.yellow);
            ChangeButtonColor(normalMode, Color.white);
            diffInfo.text = "Sinh Lực và Công Kích các đòn cận chiến của Niên Thú yếu đi 50%";
        }

        // Set the interactable state of buttonStart based on difficultID
        buttonrealStart.GetComponent<Button>().interactable = (difficultID == 1 || difficultID == 2);
    }

    void ChangeButtonColor(Button button, Color color)
    {
        // Get the Image component in the Button object
        Image buttonImage = button.GetComponent<Image>();

        // Check if the Image component exists
        if (buttonImage != null)
        {
            // Change the color of the Image component
            buttonImage.color = color;
        }
        else
        {
            Debug.LogError("Image component not found in the Button object.");
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
        if (difficult == 1 || difficult == 2)
        {
            Debug.Log(difficult == 1 ? "easy" : "normal");
            difficultID = difficult;
            SaveFile();

            // Update the button states after changing difficultID
            SetButtonState();
        }
        else
        {
            return;
        }
    }
    string GetPathFile()
    {
        return Path.Combine(Application.persistentDataPath, "FormDifficult.Json");
    }
}
