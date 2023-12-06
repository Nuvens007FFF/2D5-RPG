using System.Collections;
using UnityEngine;

public class PlayHeroUltimate : MonoBehaviour
{
    public AudioClip skillActivationSound;

    void Start()
    {
        // Check if AudioManager exists in the scene
        if (AudioManager.instance != null)
        {
            // Play the skill activation sound through AudioManager
            AudioManager.instance.PlayHeroSFX2(skillActivationSound);
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene. Make sure AudioManager is present.");
        }
    }
}
