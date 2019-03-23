using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Planet currentPlanet;
        [SerializeField] float moveSpeedOnPlanet;
        [SerializeField] float jumpImpulse = 5;
        
        [Inject] PhysicsHelper physicsHelper;

        new Rigidbody2D rigidbody;

        bool hasMovedHorizontallyLastFrame;
        int horizontalMovementDirectionMultiplier = 1;
        
        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (currentPlanet == null)
                return;

            var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter().normalized;
            rigidbody.AddForce(directionTowardsPlanetCenter * physicsHelper.GravityOnPlanet);
        }

        void Update()
        {
            if (currentPlanet == null)
                return;

            var horizontal = Input.GetAxis("Horizontal");

            var isMovingHorizontallyThisFrame = horizontal != 0f;

            if (isMovingHorizontallyThisFrame)
            {
                var deltaFromPlanetCenter = -CalculateDeltaToPlanetCenter();
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
                rigidbody.velocity = -CalculateDeltaToPlanetCenter().normalized * jumpImpulse;
            }
        }

        Vector3 CalculateDeltaToPlanetCenter()
        {
            return currentPlanet.transform.position - transform.position;
        }

        void OnCollisionEnter(Collision other)
        {
            //var otherPlanet = other.gameObject.GetComponent<Planet>();
            //currentPlanet = otherPlanet;
        }
    }
}