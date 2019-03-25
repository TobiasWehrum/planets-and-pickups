using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A planet with a radius (and gravity as computed by the <see cref="PhysicsHelper"/>.
    /// </summary>
    public class Planet : MonoBehaviour
    {
        [Inject] PhysicsHelper physicsHelper;
        
        public float Radius { get; private set; }
        public Vector3 Position { get; private set; }

        void Awake()
        {
            Radius = transform.localScale.x / 2f;
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