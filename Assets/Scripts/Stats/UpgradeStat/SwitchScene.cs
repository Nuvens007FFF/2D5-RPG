using UnityEngine.SceneManagement;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{   
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
