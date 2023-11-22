using UnityEngine;
using System.Collections;

public class AutoTurnOffEmission : MonoBehaviour
{
    public float turnOffDelay = 3.0f; // Set the duration after which emission will be turned off
    private ParticleSystem particle;

    private void Start()
    {
        // Get the ParticleSystem component attached to the GameObject
        particle = GetComponent<ParticleSystem>();

        // Turn on emission when the game starts
        TurnOnEmission();

        // Start the emission coroutine
        StartCoroutine(TurnOffEmissionCoroutine());
    }

    private IEnumerator TurnOffEmissionCoroutine()
    {
        // Wait for the specified duration before turning off emission
        yield return new WaitForSeconds(turnOffDelay);

        // Turn off emission
        SetEmission(false);
    }

    private void SetEmission(bool isEnabled)
    {
        // Get the main module of the ParticleSystem
        var mainModule = particle.main;

        // Enable or disable emission based on the provided parameter
        mainModule.simulationSpeed = isEnabled ? 1.0f : 0.0f;

        // Restart the emission if enabling
        if (isEnabled)
        {
            particle.Play();
        }
    }

    // Function to manually turn on emission (can be called from other scripts)
    public void TurnOnEmission()
    {
        SetEmission(true);
    }
}
