using UnityEngine;
using System.Collections;

public class SwordParticleSystemTrail : MonoBehaviour
{
    public ParticleSystem trail;

    private void Start()
    {
        //trail = GetComponent<ParticleSystem>();

        //// Adjust Particle System settings
        //ParticleSystem.MainModule main = trail.main;
        //main.duration = 0.4f;
        //main.startSize = 0.5f; // Reduce the size of the particles
        //main.startSizeMultiplier = 1.0f;
        //main.startLifetime = 1.0f;
        //main.startSpeed = 1.0f;

        //ParticleSystem.ShapeModule shape = trail.shape;
        //shape.radius = 0.05f; // Reduce the radius of the emission shape

        //ParticleSystem.EmissionModule emission = trail.emission;
        //emission.rateOverTime = 25; // Reduce the rate of particle emission

        //ParticleSystemRenderer renderer = trail.GetComponent<ParticleSystemRenderer>();
        //renderer.material.mainTextureScale = new Vector2(2.0f, 1.0f); // Increase the tiling of the texture

        //trail.Stop();
        //=========(quoctien229 Fix)================//
        trail = GetComponent<ParticleSystem>();
        trail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule main = trail.main;
        main.duration = 0.4f;
        main.startSize = 0.5f; // Reduce the size of the particles
        main.startSizeMultiplier = 1.0f;
        main.startLifetime = 1.0f;
        main.startSpeed = 1.0f;

        ParticleSystem.ShapeModule shape = trail.shape;
        shape.radius = 0.05f; // Reduce the radius of the emission shape

        ParticleSystem.EmissionModule emission = trail.emission;
        emission.rateOverTime = 25; // Reduce the rate of particle emission

        ParticleSystemRenderer renderer = trail.GetComponent<ParticleSystemRenderer>();
        renderer.material.mainTextureScale = new Vector2(2.0f, 1.0f);
    }

    public void StartTrail()
    {
        trail.Play();
    }

    public void EndTrail()
    {
        trail.Stop();
    }

    public void ClearTrail()
    {
        trail.Clear();
    }
}
