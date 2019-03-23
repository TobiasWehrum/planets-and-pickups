using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Planet : MonoBehaviour
    {
        [Inject] PhysicsHelper physicsHelper;
        
        public float Radius { get; private set; }

        void Awake()
        {
            Radius = transform.localScale.x;
        }
    }
}