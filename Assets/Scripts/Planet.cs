using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Planet : MonoBehaviour
    {
        [Inject] PhysicsHelper physicsHelper;
        
        public float Radius { get; private set; }
        public Vector3 Position { get; private set; }

        void Awake()
        {
            Radius = transform.localScale.x;
            Position = transform.position;
        }

        void OnEnable()
        {
            physicsHelper.RegisterPlanet(this);
        }

        void OnDisable()
        {
            physicsHelper.DeregisterPlanet(this);
        }
    }
}