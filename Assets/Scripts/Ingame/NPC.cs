using System;
using UnityEngine;
using Zenject;
using Zchfvy.Plus;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A pickup to be collected by the player. Despawns itself when too far away from the playing field.
    /// </summary>
    public class NPC : MonoBehaviour
    {
        [SerializeField] float DespawnDistanceFactor = 1.3f;
        [SerializeField] ParticleSystem collectedParticleSystem;

        [Inject] GameArea gameArea;
        [Inject] Pool pool;
        
        float despawnDistance;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (gameArea != null)
            {
                GizmosPlus.Circle(gameArea.Center,  Vector3.back * despawnDistance);
                GizmosPlus.Text(gameArea.Center + Vector3.up*despawnDistance, "Despawn Radius");
            }

            
        }
        
        
        void Update()
        {
            despawnDistance = gameArea.Radius*DespawnDistanceFactor;
            var distanceFromCenterSqr = Vector3.Distance(transform.position, gameArea.Center);
            if (distanceFromCenterSqr >= despawnDistance)
            {
                pool.Release(gameObject);
            }
        }
        
        public void RemoveNPC()
        {
            if (collectedParticleSystem != null)
            {
                var particleSystem = pool.Get(collectedParticleSystem, transform.position, transform.rotation, transform.parent);
                particleSystem.Play();
            }

            pool.Release(gameObject);
        }


        void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Removing NPC");
            GameObject otherGo = other.gameObject;
            if (gameObject.CompareTag(Tag.Enemy) && otherGo.CompareTag(Tag.Enemy))
            {

                otherGo.GetComponent<NPC>().RemoveNPC();
                this.RemoveNPC();
            }
        }
    }
}