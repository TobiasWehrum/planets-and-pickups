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

        [Inject] PhysicsHelper physicsHelper;
        [Inject] Constants constants;        

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
            
            rigidbody.velocity = ((Vector3) mainDirection + evadeDirection).normalized * speed;
        }
    }
}