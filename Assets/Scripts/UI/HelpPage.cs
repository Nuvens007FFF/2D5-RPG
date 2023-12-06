using UnityEngine;
using UnityEngine.UI;

public class HelpPage : MonoBehaviour
{
    public Button ButtonLeft;
    public Button ButtonRight;
    public Button ButtonClose;

    public GameObject page1;
    public GameObject page2;

    void Start()
    {
        // Attach the event handlers to the buttons
        ButtonLeft.onClick.AddListener(ShowPage1);
        ButtonRight.onClick.AddListener(ShowPage2);
        ButtonClose.onClick.AddListener(CloseHelp);

        // Initially, show page 1 and hide page 2
        ShowPage1();
    }

    // Event handler for the left button
    void ShowPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }

    // Event handler for the right button
    void ShowPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }

    void CloseHelp()
    {
        Destroy(gameObject);
    }
}