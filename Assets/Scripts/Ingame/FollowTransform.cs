using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Follows another transform smoothly. Used for the camera following the player.
    /// </summary>
    public class FollowTransform : MonoBehaviour
    {
        
        [Inject] Player player;
        [SerializeField] float followMultiplier = 0.05f;
        private Transform target;
        
        void Awake()
        {
            target = player.gameObject.transform;
            var targetPosition = target.position;
            targetPosition.z = transform.position.z;
            transform.position = targetPosition;
        }
        
        void FixedUpdate()
        {
            var position = transform.position;
            var targetPosition = target.position;
            targetPosition.z = position.z;

            transform.position = Vector3.Lerp(position, targetPosition, followMultiplier);
        }
    }
}