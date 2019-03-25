using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Moves the attached entity in a main direction while evading planets orthogonally. Optionally also being attracted by the player.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveInDirectionWhileAvoidingPlanets : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float planetAvoidanceMultiplier = 1f;
        [SerializeField] AnimationCurve borderAvoidanceCurve;
        [SerializeField] float borderAvoidanceMultiplier = 1f;
        [SerializeField] float playerAttractionMaxDistance;

        [Inject] PhysicsHelper physicsHelper;
        [Inject] Constants constants;
        [Inject] Player player;

        new Rigidbody2D rigidbody;
        
        Vector2 mainDirection;
        Vector2 orthogonalDirection;
        
        bool initialized;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            initialized = false;
        }

        void OnDisable()
        {
            // Deinitialize on despawn
            initialized = false;
        }

        public void UpdateMainDirection(Vector3 mainDirection)
        {
            this.mainDirection = mainDirection;
            orthogonalDirection.x = mainDirection.y;
            orthogonalDirection.y = -mainDirection.x;
            initialized = true;
        }

        public void InitializeIfNecessary()
        {
            if (initialized)
                return;
            
            mainDirection = -transform.position.normalized;
            orthogonalDirection = new Vector2(mainDirection.y, -mainDirection.x);
            initialized = true;
        }
        
        void Update()
        {
            InitializeIfNecessary();
            
            var evasionGravity = -physicsHelper.GetGravityAtPosition(rigidbody.position, 0f) * planetAvoidanceMultiplier;

            var distanceToCenter = transform.position.magnitude;
            var directionToCenter = -(transform.position / distanceToCenter);
            var closenessToCenterPercent = Mathf.Clamp01(distanceToCenter / constants.playfieldRadius);
            var gravityTowardsCenter = borderAvoidanceCurve.Evaluate(closenessToCenterPercent) * borderAvoidanceMultiplier;
            evasionGravity += gravityTowardsCenter * directionToCenter;

            var evadeDirection = Vector3.Project(evasionGravity, orthogonalDirection);

            var direction = ((Vector3) mainDirection + evadeDirection).normalized;

            if ((playerAttractionMaxDistance > 0) && (player.isActiveAndEnabled))
            {
                var playerDelta = player.transform.position - transform.position;
                var playerDistanceSqr = playerDelta.sqrMagnitude;
                if (playerDistanceSqr <= (playerAttractionMaxDistance * playerAttractionMaxDistance))
                {
                    var playerDistance = Mathf.Sqrt(playerDistanceSqr);
                    var playerDirection = playerDelta.normalized;
                    var playerNearnessPercent = 1 - (playerDistance / playerAttractionMaxDistance);

                    direction = Vector3.Lerp(direction, playerDirection, playerNearnessPercent).normalized;
                }
            }

            rigidbody.velocity = direction * speed;
        }
    }
}