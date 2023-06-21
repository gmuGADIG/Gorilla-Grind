using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates particles moving in the background as the player moves past them.
/// Attach it to an object with a ParticleSystem.
/// </summary>
public class BoardParticles : MonoBehaviour
{
    private ParticleSystem particles;
    
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        var vol = particles.velocityOverLifetime;
        vol.x = -PlayerMovement.CurrentHorizontalSpeed;
    }
}
