using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeedOnPlanet;
        [SerializeField] float freeMovementSpeed = 10;
        [SerializeField] float jumpImpulse = 5;
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float maxDistanceFromCenter;
        [SerializeField] float onPlanetRadius = 0.1f;
        
        [Inject] PhysicsHelper physicsHelper;

        new Rigidbody2D rigidbody;

        float radius;

        bool hasMovedHorizontallyLastFrame;
        int horizontalMovementDirectionMultiplier = 1;

        Vector2 freeMoveDirection;
        
        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            radius = transform.localScale.x / 2f;
        }

        void FixedUpdate()
        {
            var currentPlanet = physicsHelper.GetCurrentPlanet(rigidbody.position, radius + onPlanetRadius);
            if (currentPlanet == null)
            {
                rigidbody.AddForce(physicsHelper.GetGravityAtPosition(transform.position, radius));
                rigidbody.AddForce(freeMoveDirection * freeMovementSpeed);
            }
            else
            {
                var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                rigidbody.AddForce(directionTowardsPlanetCenter * physicsHelper.GravityOnPlanet);
            }

            // Cap max speed
            if (maxSpeed > 0)
            {
                var speedSqr = rigidbody.velocity.sqrMagnitude;
                if (speedSqr > maxSpeed * maxSpeed)
                {
                    rigidbody.velocity *= maxSpeed / Mathf.Sqrt(speedSqr);
                }
            }
        }

        void Update()
        {
            var currentPlanet = physicsHelper.GetCurrentPlanet(rigidbody.position, radius + onPlanetRadius);
            //Debug.Log(currentPlanet);
            
            if (currentPlanet == null)
            {
                FreelyMoveInDirections();
                RestrictPlayerPosition();
            }
            else
            {
                freeMoveDirection.x = 0;
                freeMoveDirection.y = 0;
                
                MoveAroundPlanet(currentPlanet);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var jumpForceDirection = -CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                    /*
                    var direction = Input.GetAxis("Horizontal");
                    jumpForceDirection.x += jumpForceDirection.y * direction;
                    jumpForceDirection.y -= jumpForceDirection.x * direction;
                    jumpForceDirection.Normalize();
                    */
                    
                    rigidbody.velocity = jumpForceDirection * jumpImpulse;
                }
            }
        }

        void FreelyMoveInDirections()
        {
            freeMoveDirection.x = Input.GetAxis("Horizontal");
            freeMoveDirection.y = Input.GetAxis("Vertical");
        }
        
        void RestrictPlayerPosition()
        {
            var distanceFromCenterSqr = rigidbody.position.sqrMagnitude;
            if (distanceFromCenterSqr > maxDistanceFromCenter * maxDistanceFromCenter)
            {
                rigidbody.position *= maxDistanceFromCenter / Mathf.Sqrt(distanceFromCenterSqr);
            }
        }

        void MoveAroundPlanet(Planet planet)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var isMovingHorizontallyThisFrame = horizontal != 0f;

            if (isMovingHorizontallyThisFrame)
            {
                var deltaFromPlanetCenter = -CalculateDeltaToPlanetCenter(planet);
                /*
                if (!hasMovedHorizontallyLastFrame)
                {
                    horizontalMovementDirectionMultiplier = (deltaFromPlanetCenter.y < 0) ? -1 : 1;
                }
                */
                
                var speed = moveSpeedOnPlanet / planet.Radius;
                var moveDelta = -horizontal * horizontalMovementDirectionMultiplier * speed * Time.deltaTime;
                var rotatedDirection = Quaternion.Euler(0, 0, moveDelta) * deltaFromPlanetCenter;
                rigidbody.position = planet.transform.position + rotatedDirection;
            }

            hasMovedHorizontallyLastFrame = isMovingHorizontallyThisFrame;
        }

        Vector3 CalculateDeltaToPlanetCenter(Planet planet)
        {
            return planet.transform.position - transform.position;
        }
    }
}