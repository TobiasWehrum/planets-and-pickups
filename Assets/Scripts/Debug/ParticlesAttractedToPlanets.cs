using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that uses the attraction of the planets to set the velocity of particles. Originally planned as an ingame
    /// effect, it proved to me too distracting, but it's still useful as a debug visualization.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesAttractedToPlanets : MonoBehaviour
    {
        [Inject] PhysicsHelper physicsHelper;
        
        new ParticleSystem particleSystem;
        
        ParticleSystem.Particle[] particles;

        void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
            
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        void Update()
        {
            particleSystem.GetParticles(particles);

            for (var index = 0; index < particles.Length; index++)
            {
                var particle = particles[index];
                if (particle.remainingLifetime <= 0f)
                    continue;
                
                particle.velocity += physicsHelper.GetGravityAtPosition(particle.position, 0f) * Time.deltaTime;
                particles[index] = particle;
            }

            particleSystem.SetParticles(particles);
        }
    }
}