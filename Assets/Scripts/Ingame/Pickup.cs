using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A pickup to be collected by the player. Despawns itself when too far away from the playing field.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        [SerializeField] float extraDespawnDistance = 40f;
        [SerializeField] ParticleSystem collectedParticleSystem;

        [Inject] Constants constants;
        [Inject] Pool pool;
        
        float despawnDistanceSqr;

        void Awake()
        {
            despawnDistanceSqr = Mathf.Pow(constants.playfieldRadius + extraDespawnDistance, 2);
        }
        
        void Update()
        {
            var distanceFromCenterSqr = transform.position.sqrMagnitude;
            if (distanceFromCenterSqr >= despawnDistanceSqr)
            {
                pool.Release(gameObject);
            }
        }
        
        public void Collect()
        {
            if (collectedParticleSystem != null)
            {
                var particleSystem = pool.Get(collectedParticleSystem, transform.position, transform.rotation, transform.parent);
                particleSystem.Play();
            }

            pool.Release(gameObject);
        }
    }
}