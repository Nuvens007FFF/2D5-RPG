using System.Collections;
using UnityEngine;

public class PlayBossGlobalAttackAudio : MonoBehaviour
{
    public AudioClip skillActivationSound;
    public float delay = 2.0f; // Adjust the delay time as needed

    void Start()
    {
        // Check if AudioManager exists in the scene
        if (AudioManager.instance != null)
        {
            // Start the DelayedPlay coroutine
            StartCoroutine(DelayedPlay());
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene. Make sure AudioManager is present.");
        }
    }

    IEnumerator DelayedPlay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Play the skill activation sound through AudioManager
        AudioManager.instance.PlayBossSFX2(skillActivationSound);
    }
}
