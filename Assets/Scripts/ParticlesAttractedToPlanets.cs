using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesAttractedToPlanets : MonoBehaviour
    {
        [Inject] PhysicsHelper physicsHelper;
        
        ParticleSystem particleSystem;
        
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
                
                particle.velocity += physicsHelper.GetGravityAtPosition(particle.position) * Time.deltaTime;
                particles[index] = particle;
            }

            particleSystem.SetParticles(particles);
        }
    }
}