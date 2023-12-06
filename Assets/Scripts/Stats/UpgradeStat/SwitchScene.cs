using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SwitchScene : MonoBehaviour
{
    public GameObject BlackMask;

    public void LoadScene(int scene)
    {
        StartCoroutine(goToScene(scene));
    }

    private IEnumerator goToScene(int scene)
    {
        GameObject blackMaskObj = Instantiate(BlackMask, Camera.main.transform);
        BlackMask blackMask = blackMaskObj.GetComponent<BlackMask>();
        blackMask.fadeTime = 0.75f;
        yield return new WaitForSeconds(0.75f); // Adjust the delay as needed
        SceneManager.LoadScene(scene);
    }
}
