using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class MoveInDirectionWhileAvoidingPlanets : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] Vector2 mainDirection;
        [SerializeField] float planetAvoidanceMultiplier = 1f;
        [SerializeField] AnimationCurve borderAvoidanceCurve;
        [SerializeField] float borderAvoidanceMultiplier = 1f;
        [SerializeField] float playerAttractionMaxDistance = 2f;
        [SerializeField] float playerAttractionMultiplier;

        [Inject] PhysicsHelper physicsHelper;
        [Inject] Constants constants;
        [Inject] Player player;

        new Rigidbody2D rigidbody;
        
        Vector2 orthogonalDirection;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            mainDirection = -transform.position.normalized;
            
            orthogonalDirection = new Vector2(mainDirection.y, -mainDirection.x);
        }
        
        void Update()
        {
            var evasionGravity = -physicsHelper.GetGravityAtPosition(rigidbody.position, 0f) * planetAvoidanceMultiplier;

            var distanceToCenter = transform.position.magnitude;
            var directionToCenter = -(transform.position / distanceToCenter);
            var closenessToCenterPercent = Mathf.Clamp01(distanceToCenter / constants.playfieldRadius);
            var gravityTowardsCenter = borderAvoidanceCurve.Evaluate(closenessToCenterPercent) * borderAvoidanceMultiplier;
            evasionGravity += gravityTowardsCenter * directionToCenter;

            var evadeDirection = Vector3.Project(evasionGravity, orthogonalDirection);

            var extraGravity = Vector3.zero;
            var playerDelta = player.transform.position - transform.position;
            var playerDistanceSqr = playerDelta.sqrMagnitude;
            if (playerDistanceSqr <= (playerAttractionMaxDistance * playerAttractionMaxDistance))
            {
                var playerDistance = Mathf.Sqrt(playerDistanceSqr);
                var playerDirection = playerDelta.normalized;

                extraGravity += playerDirection * (playerDistance / playerAttractionMaxDistance) * playerAttractionMultiplier;
            }
            
            rigidbody.velocity = ((Vector3) mainDirection + evadeDirection).normalized * speed + extraGravity;
        }
    }
}