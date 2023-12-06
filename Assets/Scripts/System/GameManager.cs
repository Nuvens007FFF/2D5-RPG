using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] prefabsToLoad;

    private GameObject bossPrefabInstance;
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
                // Check if the prefab has the tag "Player"
                if (prefab.CompareTag("Player"))
                {
                    Instantiate(prefab, new Vector3(0f, -3f, 0f), Quaternion.identity);
                }
                else if (prefab.CompareTag("BossObject"))
                {
                    bossPrefabInstance = Instantiate(prefab, new Vector3(0f, 3f, 0f), Quaternion.identity);
                }
                else
                {
                    // Instantiate other prefabs at the default position (Vector3.zero)
                    Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }
            }
        }
    }
}
