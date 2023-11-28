using UnityEngine;

public class GamePause : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // Check for input to toggle pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // If the game is paused, set the timeScale to 0
        // If it's resumed, set it back to 1
        Time.timeScale = isPaused ? 0 : 1;
    }
}