using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Planet currentPlanet;
        [SerializeField] float moveSpeedOnPlanet;
        [SerializeField] float freeMovementSpeed = 10;
        [SerializeField] float jumpImpulse = 5;
        [SerializeField] float maxSpeed = 10f;
        
        [Inject] PhysicsHelper physicsHelper;

        new Rigidbody2D rigidbody;

        bool hasMovedHorizontallyLastFrame;
        int horizontalMovementDirectionMultiplier = 1;

        Vector2 freeMoveDirection;
        
        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (currentPlanet == null)
                return;

            /*
            var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter(currentPlanet).normalized;
            rigidbody.AddForce(directionTowardsPlanetCenter * physicsHelper.GravityOnPlanet);
            */

            rigidbody.AddForce(physicsHelper.GetGravityAtPosition(transform.position));
            
            rigidbody.AddForce(freeMoveDirection * freeMovementSpeed);

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
            if (currentPlanet == null)
                return;

            FreelyMoveInDirections();
            //MoveAroundPlanet(currentPlanet);
        }

        void FreelyMoveInDirections()
        {
            freeMoveDirection.x = Input.GetAxis("Horizontal");
            freeMoveDirection.y = Input.GetAxis("Vertical");
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
                
                var speed = moveSpeedOnPlanet / currentPlanet.Radius;
                var moveDelta = -horizontal * horizontalMovementDirectionMultiplier * speed * Time.deltaTime;
                var rotatedDirection = Quaternion.Euler(0, 0, moveDelta) * deltaFromPlanetCenter;
                rigidbody.position = currentPlanet.transform.position + rotatedDirection;
            }

            hasMovedHorizontallyLastFrame = isMovingHorizontallyThisFrame;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.velocity = -CalculateDeltaToPlanetCenter(planet).normalized * jumpImpulse;
            }
        }

        Vector3 CalculateDeltaToPlanetCenter(Planet planet)
        {
            return planet.transform.position - transform.position;
        }

        void OnCollisionEnter(Collision other)
        {
            //var otherPlanet = other.gameObject.GetComponent<Planet>();
            //currentPlanet = otherPlanet;
        }
    }
}