using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public GameObject dialogPagePrefab;
    public GameObject BlackMask;
    public GameObject HelpPage;
    private GameObject currentDialogPage;

    private string currentDialogText;
    private int currentLineIndex;
    private int currentCharIndex;

    private bool isWaitingForInput;
    private bool isMouseClicked;

    public AudioClip UpgradeTheme;

    [SerializeField] private bool _isIntroPlayed;
    [SerializeField] private bool _isBossDefeatPlayed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadFile();
        Debug.Log("Intro: " + _isIntroPlayed);
    }

    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "DialogStatus.Json");
    }

    private void SaveFile()
    {
        DialogStatus status = new DialogStatus
        {
            isIntroPlayed = _isIntroPlayed,
            isBossDefeatPlayed = _isBossDefeatPlayed,
        };
        string jsonInventory = JsonUtility.ToJson(status, true);
        string filePath = GetFilePath();
        File.WriteAllText(filePath, jsonInventory);
    }

    private void LoadFile()
    {
        if (File.Exists(GetFilePath()))
        {
            string statusFile = File.ReadAllText(GetFilePath());
            DialogStatus status = JsonUtility.FromJson<DialogStatus>(statusFile);
            _isIntroPlayed = status.isIntroPlayed;
            _isBossDefeatPlayed = status.isBossDefeatPlayed;
        }
    }

    public void PlayDialog(string dialogText, string dialogKey)
    {
        // Check if there's an existing dialog page, destroy it if it exists
        if (currentDialogPage != null)
        {
            Destroy(currentDialogPage);
        }

        // Instantiate a new dialog page from the prefab
        currentDialogPage = Instantiate(dialogPagePrefab, Camera.main.transform);

        // Set the dialog text
        currentDialogText = dialogText;
        currentLineIndex = 0;
        currentCharIndex = 0;
        isWaitingForInput = false;

        // Display the first line
        StartCoroutine(DisplayNextCharacter(dialogKey));
    }

    // Display the next character of the dialog
    IEnumerator DisplayNextCharacter(string dialogKey)
    {
        TextMeshProUGUI dialogTextMeshPro = currentDialogPage.GetComponentInChildren<TextMeshProUGUI>();

        if (dialogTextMeshPro != null)
        {
            while (currentLineIndex < currentDialogText.Split('\n').Length)
            {
                // Get the next line
                string[] lines = currentDialogText.Split('\n');
                string currentLine = lines[currentLineIndex];

                // Display characters one by one
                while (currentCharIndex < currentLine.Length)
                {
                    // Get the next character
                    char nextChar = currentLine[currentCharIndex];

                    // Append the character to the displayed text
                    dialogTextMeshPro.text += nextChar;

                    // Increment the character index
                    currentCharIndex++;

                    // Wait for a short time before displaying the next character
                    yield return new WaitForSeconds(0.02f);
                }

                // Move to the next line
                currentLineIndex++;
                currentCharIndex = 0;

                // Set a flag to check for user input
                isWaitingForInput = true;

                // Wait for user input before proceeding to the next line
                yield return null;

                // Wait until the user clicks the mouse button
                yield return new WaitUntil(() => isMouseClicked);

                // Reset the flag
                isWaitingForInput = false;

                // Clear the text for the next line
                dialogTextMeshPro.text = "";
            }

            // Dialog is complete, destroy the dialog page
            if (!_isIntroPlayed && dialogKey == "Intro")
            {
                AudioManager.instance.PlayMusic(UpgradeTheme, 0.25f);
                _isIntroPlayed = true;
                SaveFile();
                Instantiate(HelpPage);
            }
            if(!_isBossDefeatPlayed && dialogKey == "BossDefeated")
            {
                _isBossDefeatPlayed = true;
                SaveFile();
            }
            Destroy(currentDialogPage);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on DialogPage prefab.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for user input to progress the dialog
        if (isWaitingForInput && Input.GetMouseButtonDown(0))
        {
            isMouseClicked = true;
        }
        else
        {
            isMouseClicked = false; // Reset the flag if no input detected
        }
    }

    public void PlayIntroDialog()
    {
        string formattedText = "Tương truyền từ thời cổ xưa có một loài quái thú tên “Niên”, nửa bò nửa rồng, mặt sư tử, trên đầu mọc sừng, hết sức hung dữ.\n“Niên” quanh năm sống sâu dưới đáy biển, đến giao thừa thì lên bờ phá hoại, ăn thịt súc vật, thương tổn mạng người.\nCho nên cứ đến ba mươi Tết hằng năm, già trẻ gái trai trong các làng trại đều phải chạy vào rừng sâu núi thẳm trong sợ hãi để khỏi bị thú dữ “Niên” làm hại.\n Cho đến 1 ngày...";
        if (!_isIntroPlayed)
        {
            PlayDialog(formattedText, "Intro");
        }
        else
        {
            AudioManager.instance.PlayMusic(UpgradeTheme, 0.25f);
        }
    }

    public void PlayBossDefeatedDialog()
    {
        if (!_isBossDefeatPlayed)
        {                 
            StartCoroutine(PlayVictory());
        }
    }

    private IEnumerator PlayVictory()
    {
        yield return new WaitForSeconds(3f); // Adjust the delay as needed
        GameObject blackMaskObj = Instantiate(BlackMask, Camera.main.transform);
        yield return new WaitForSeconds(3f); // Adjust the delay as needed
        Destroy(blackMaskObj);
        Debug.Log("Obj: " + blackMaskObj);

        string formattedText = "Sau khi bị đánh bại, “Niên” may mắn kịp thời trốn thoát\n Nhưng nỗi ám ảnh về người đã đánh bại nó vẫn còn đó, từ đó nó trở nên cực kì sợ hãi trước Lửa, Âm Thanh Lớn và Màu Đỏ\n Dân làng nhận ra điểm yếu này và nhanh chóng truyền bá cho người khác, sáng sớm hôm sau còn đi chào hỏi thân quyến bạn bè, chúc mừng họ vượt qua chuỗi ngày áp bức của Niên\nTừ đó về sau mọi người đều biết phương pháp đuổi Niên, giao thừa hàng năm nhà nhà dán câu đối đỏ đốt pháo, hộ hộ ánh nến sáng trưng, nhìn canh giờ chờ tuổi mới.\n Phong tục này truyền bá càng lúc càng rộng, cuối cùng trở thành ngày lễ truyền thống long trọng nhất dân gian: “Tết”";
        PlayDialog(formattedText, "BossDefeated");
    }
}

[System.Serializable]
public class DialogStatus
{
    public bool isIntroPlayed;
    public bool isBossDefeatPlayed;
}
