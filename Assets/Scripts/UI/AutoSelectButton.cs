using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelectButton : MonoBehaviour
{
    private Button myButton;

    void Start()
    {
        // Assuming the script is attached to the same GameObject as the Button
        myButton = GetComponent<Button>();

        // Select the button when the scene starts
        SelectButton();
    }

    void SelectButton()
    {
        // Check if the Button component exists
        if (myButton != null)
        {
            // Use EventSystem to select the button
            EventSystem.current.SetSelectedGameObject(myButton.gameObject);

            // Optionally, you can also manually set the button to its "selected" state
            myButton.OnSelect(null);
        }
        else
        {
            Debug.LogError("Button component not found.");
        }
    }
}
