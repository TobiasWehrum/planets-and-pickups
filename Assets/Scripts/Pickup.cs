using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] float extraDespawnDistance = 40f;

        [Inject] Constants constants;
        
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
                Destroy(gameObject);
            }
        }
        
        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}