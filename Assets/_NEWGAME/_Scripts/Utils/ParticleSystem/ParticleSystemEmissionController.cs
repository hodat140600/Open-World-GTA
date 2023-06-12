using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ParticleSystemEmissionController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _pss;

    private List<float> PresetRandomRemainingLifetime = new() { 0f, .1f, .2f, .3f };

    // performance purpose
    private float                     remainingLifetime;
    private ParticleSystem.Particle[] particles;
    private int                       numParticlesAlive;

    public void Stop()
    {
        foreach (ParticleSystem system in _pss)
        {
            particles         = new ParticleSystem.Particle[system.main.maxParticles];
            numParticlesAlive = system.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                remainingLifetime              = PresetRandomRemainingLifetime[i % 4];
                particles[i].remainingLifetime = remainingLifetime;
            }

            system.SetParticles(particles, numParticlesAlive);
            ParticleSystem.EmissionModule em = system.emission;
            em.enabled = false;
        }
    }

    public void Start()
    {
        foreach (ParticleSystem system in _pss)
        {
            ParticleSystem.EmissionModule em = system.emission;
            em.enabled = true;
        }
    }
}