using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A particle system that goes back into the <see cref="Pool"/> once it's done.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ReleaseIntoPoolAfterPlaying : MonoBehaviour
    {
        [Inject] Pool pool;
        
        new ParticleSystem particleSystem;

        void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        
        void LateUpdate()
        {
            if (!particleSystem.isPlaying)
            {
                pool.Release(gameObject);
            }
        }
    }
}