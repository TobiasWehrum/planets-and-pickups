using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Follows another transform smoothly. Used for the camera following the player.
    /// </summary>
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float followMultiplier = 0.9f;

        void Awake()
        {
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