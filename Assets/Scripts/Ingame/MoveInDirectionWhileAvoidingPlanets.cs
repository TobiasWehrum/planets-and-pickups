using System;
using UnityEngine;
using Zchfvy.Plus;
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
        [SerializeField] float planetAvoidanceMultiplier = 3f;
        [SerializeField] AnimationCurve borderAvoidanceCurve;
        [SerializeField] float borderAvoidanceMultiplier = 1f;
        [SerializeField] float playerAttractionMaxDistance;

        [Inject] PhysicsHelper physicsHelper;
        [Inject] GameArea gameArea;
        [Inject] Player player;

        new Rigidbody2D rigidbody;

        Vector2 mainDirection;
        Vector2 orthogonalDirection;
        private Vector2 directionToCenter;
        private Vector2 repulsionDirection;
        private Vector2 movementDirection;
        private float closenessToCenterPercent;

        private Vector2 gravitationalRepulsionFromPlanets;
        private Vector2 gravitationalAttractionFromCenter;


        bool initialized;

        private void OnDrawGizmos()
        {
            if (mainDirection != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, mainDirection);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position, orthogonalDirection);
            }
        }


        private void OnDrawGizmosSelected()
        {
//            Gizmos.color = Color.white;
//            GizmosPlus.Arrow(transform.position, directionToCenter);
            Gizmos.color = Color.blue;
            GizmosPlus.Arrow(transform.position, repulsionDirection);
            Gizmos.color = Color.white;
            GizmosPlus.Arrow(transform.position, gravitationalAttractionFromCenter);
            Gizmos.color = Color.green;
            GizmosPlus.Arrow(transform.position, gravitationalRepulsionFromPlanets);
//            Gizmos.color = Color.green;
//            GizmosPlus.Arrow(transform.position, movementDirection);
        }

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


        void UpdatePointersToCenter()
        {
            directionToCenter = gameArea.Center - transform.position;
            closenessToCenterPercent =
                Mathf.Clamp01(directionToCenter.magnitude /
                              gameArea.Radius); // 1 if far, 0 if at center
        }

        Vector2 GetRepulsionFromPlanets()
        {
            gravitationalRepulsionFromPlanets =
                physicsHelper.GetGravityAtPosition(transform.position, 2) *
                -planetAvoidanceMultiplier;

            float gravityTowardsCenter =
                borderAvoidanceCurve.Evaluate(closenessToCenterPercent) *
                borderAvoidanceMultiplier;

            gravitationalAttractionFromCenter = (gravityTowardsCenter * directionToCenter);


            Vector2 repulsionDirection =
                Vector3.Project(gravitationalAttractionFromCenter + gravitationalRepulsionFromPlanets, orthogonalDirection);
            return repulsionDirection;
        }

        Vector2 LerpDirectionIfCloseToPlayer(Vector3 direction)
        {
            if ((playerAttractionMaxDistance > 0) && (player.isActiveAndEnabled))
            {
                var playerDelta = player.transform.position - transform.position;
                var playerDistanceSqr = playerDelta.sqrMagnitude;
                if (playerDistanceSqr <=
                    (playerAttractionMaxDistance * playerAttractionMaxDistance))
                {
                    var playerDistance = Mathf.Sqrt(playerDistanceSqr);
                    var playerDirection = playerDelta.normalized;
                    var playerNearnessPercent =
                        1 - (playerDistance / playerAttractionMaxDistance);

                    direction = Vector3
                        .Lerp(direction, playerDirection, playerNearnessPercent)
                        .normalized;
                }
            }

            return direction;
        }

        void Update()
        {
            InitializeIfNecessary();

            UpdatePointersToCenter();
            repulsionDirection = GetRepulsionFromPlanets();

            movementDirection = (mainDirection + repulsionDirection).normalized;
            movementDirection = LerpDirectionIfCloseToPlayer(movementDirection);
            rigidbody.velocity = movementDirection * speed;
        }
    }
}