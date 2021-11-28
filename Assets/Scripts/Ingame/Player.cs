using System;
using UnityEditor;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Zchfvy.Plus;


namespace MiniPlanetDefense
{
    /// <summary>
    /// The player. Can move on planets, jump, collect pickups and optionally also move while in the air.
    /// Dies when touching an enemy.
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeedOnPlanet = 650.0f;
        [SerializeField] float jumpImpulse = 5;
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float onPlanetRadius = 0.1f;
        [SerializeField] Color colorOnPlanet = Color.yellow;
        [SerializeField] Color colorOffPlanet = Color.white;
        [SerializeField] Renderer mainRenderer;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] ParticleSystem deathParticleSystem;

        [SerializeField] private MicInput micInput;

        [Inject] PhysicsHelper physicsHelper;
        [Inject] GameArea gameArea;
        [Inject] IngameUI ingameUI;
        [Inject] SoundManager soundManager;

        new Rigidbody2D rigidbody;

        float radius;


        bool isColoredOnPlanet;

        int score;

        bool destroyed;

        Planet previousPlanet;
        Planet currentPlanet;

        private bool isRotatingClockwise = false;

        public float currentAngle;


        public bool manualMovementOnPlanet = false;

        int horizontalMovementDirectionMultiplier = 1;


        private Vector2 gravitationalForce;



        void Awake()
        {
            Application.targetFrameRate = -1;
            Reset(transform.position);
            micInput = FindObjectOfType<MicInput>();
        }


        public void Reset(Vector3 pos)
        {
            
            
            rigidbody = GetComponent<Rigidbody2D>();
            radius = transform.localScale.x / 2f;

            isColoredOnPlanet = false;
            RefreshColor();


            transform.position = pos;

            trailRenderer.Clear();
        }

        void GetDirectionForOrbit()
        {
            // Create vector r from planet center to player
            Vector2 c = currentPlanet.transform.position; // center pos
            Vector2 p = transform.position;
            Vector2 r = (p - c).normalized; // vector from c to player

            // Create perp t of above (tangent on landing point)
            Vector2 t = Vector2.Perpendicular(r);

            // Project player vel v onto t 
            Vector2 v = rigidbody.velocity.normalized;
            float tangvMag = Vector2.Dot(v, t);

            // if 0 < v --> clockwise, else counterclockwise
            if (0 < tangvMag)
                isRotatingClockwise = true;

            else
                isRotatingClockwise = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (currentPlanet != null)
            {
                Vector2 p = GetPositionOnCircle(currentAngle);
                Gizmos.DrawLine(currentPlanet.transform.position, p);
                Gizmos.DrawSphere(currentPlanet.transform.position, 0.1f);
            }

            if (rigidbody != null)
            {
                Gizmos.color = Color.green;
                Vector2 v = rigidbody.velocity.normalized * 5.0f;
                GizmosPlus.Arrow(transform.position, v);
            }

            Gizmos.color = new Color(0, 0, 255, 0.2f);
            Gizmos.DrawSphere(transform.position, radius);

            if (gravitationalForce != null)
            {
                Gizmos.color = Color.white;
                GizmosPlus.Arrow(transform.position, gravitationalForce);
            }
        }

        void FixedUpdate()
        {
            // Gravitational attraction 
            if (currentPlanet == null)
            {
                gravitationalForce =
                    physicsHelper.GetGravityAtPosition(transform.position, radius);
                rigidbody.AddForce(gravitationalForce);
            }
            else
            {
                if (manualMovementOnPlanet)
                {
                    var directionTowardsPlanetCenter =
                        CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                    rigidbody.AddForce(directionTowardsPlanetCenter *
                                       physicsHelper.GravityOnPlanet);
                }
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


        void LandedOnPlanet()
        {
            Debug.Log("Landed on " + currentPlanet.name);
            soundManager.PlaySound(Sound.TouchPlanet);
            GetDirectionForOrbit();
            currentAngle = GetCurrentAngleOnCircle();
            transform.position = GetPositionOnCircle(currentAngle);
            rigidbody.velocity = Vector2.zero;
        }

        void Update()
        {
            currentPlanet =
                physicsHelper.GetCurrentPlanet(rigidbody.position,
                    radius + onPlanetRadius);
            if ((currentPlanet != null) && (currentPlanet != previousPlanet))
            {
                LandedOnPlanet();
            }

            previousPlanet = currentPlanet;

            if (currentPlanet != null) // if there is current planet
            {
                gravitationalForce = Vector2.zero;
                if (manualMovementOnPlanet)
                {
                    MoveAroundPlanet(currentPlanet);
                }
                else
                {
                    OrbitPlanet();
                }

                if (GetJumpTrigger())
                {
                    Debug.Log("Jump!");
                    var jumpForceDirection =
                        -CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                    rigidbody.velocity = jumpForceDirection * jumpImpulse;
                    currentPlanet = null;
                    soundManager.PlaySound(Sound.Jump);
                }
            }

            SetColoredOnPlanet(currentPlanet != null);
        }


        void MoveAroundPlanet(Planet planet)
        {
            currentAngle = GetCurrentAngleOnCircle();
            var horizontal = Input.GetAxis("Horizontal");
            var isMovingHorizontallyThisFrame = horizontal != 0f;

            if (isMovingHorizontallyThisFrame)
            {
                var deltaFromPlanetCenter = -CalculateDeltaToPlanetCenter(planet);

                var speed = moveSpeedOnPlanet / planet.Radius;
                var moveDelta = -horizontal * horizontalMovementDirectionMultiplier *
                                speed * Time.deltaTime;
                var rotatedDirection =
                    Quaternion.Euler(0, 0, moveDelta) * deltaFromPlanetCenter;
                rigidbody.position = planet.transform.position + rotatedDirection;
            }
        }


        Vector3 CalculateDeltaToPlanetCenter(Planet planet)
        {
            return planet.transform.position - transform.position;
        }

        void SetColoredOnPlanet(bool value)
        {
            if (isColoredOnPlanet == value)
                return;

            isColoredOnPlanet = value;
            RefreshColor();
        }

        void RefreshColor()
        {
            var color = isColoredOnPlanet ? colorOnPlanet : colorOffPlanet;
            mainRenderer.material.color = color;
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }


        void OnCollisionEnter2D(Collision2D other)
        {
            var otherGameObject = other.gameObject;
            if (otherGameObject.CompareTag(Tag.Pickup))
            {
                var pickup = other.gameObject.GetComponent<NPC>();
                pickup.RemoveNPC();

                soundManager.PlaySound(Sound.Pickup);

                score++;
                ingameUI.SetScore(score);
            }
            else if (otherGameObject.CompareTag(Tag.Enemy))
            {
                HitEnemy();
            }
        }

        void HitEnemy()
        {
            if (destroyed)
                return;

            deathParticleSystem.transform.parent = null;
            deathParticleSystem.Play();

            gameObject.SetActive(false);
            destroyed = true;

            ingameUI.ShowRestartScreen();

            soundManager.PlaySound(Sound.Death);
        }


        void OrbitPlanet()
        {
            float clockwiseMultiplier = (isRotatingClockwise) ? 1f : -1f;

            float angularVelocity = moveSpeedOnPlanet / currentPlanet.Radius;
            //Move object as orbit
            currentAngle += angularVelocity * Time.deltaTime * clockwiseMultiplier;
            currentAngle = currentAngle % 360.0f;
            if (currentAngle < 0)
                currentAngle += 360.0f;
            transform.position = GetPositionOnCircle(currentAngle);
        }

        /// <summary>
        /// Gets cartesian position on circle 
        /// </summary>
        /// <param name="angle"></param> Angle must be in degrees 
        /// <returns></returns>
        protected Vector2 GetPositionOnCircle(float angle)
        {
            Vector2 centerPos = currentPlanet.transform.position;
            float orbitRadius = currentPlanet.Radius + radius;
            float rad_angle = angle * Mathf.Deg2Rad;
            return new Vector2(
                centerPos.x + Mathf.Cos(rad_angle) * orbitRadius,
                centerPos.y + Mathf.Sin(rad_angle) * orbitRadius
            );
        }


        /// <summary>
        /// Gets angle on circle in degrees (0-360)
        /// </summary>
        /// <returns></returns>
        protected float GetCurrentAngleOnCircle()
        {
            Vector2 c = currentPlanet.transform.position; // center pos
            Vector2 p = transform.position;
            float orbitRadius = currentPlanet.Radius + radius;
            Vector2 diff = (p - c) / orbitRadius;

            float theta_rad = Mathf.Atan2(diff.y, diff.x);
            float theta_deg = theta_rad * Mathf.Rad2Deg;
            if (theta_deg < 0.0)
            {
                theta_deg += 360.0f;
            }

            return theta_deg;
        }

        bool GetJumpTrigger()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return true;

            if (MicInput.isTriggered())
            {
                return true;
            }

            return false;
        }
    }
}