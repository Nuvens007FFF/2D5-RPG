using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] prefabsToLoad;

    void Start()
    {
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        foreach (GameObject prefab in prefabsToLoad)
        {
            if (prefab != null)
            {
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
            }
        }
    }
}
