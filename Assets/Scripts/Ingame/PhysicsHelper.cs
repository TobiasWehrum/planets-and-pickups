using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Takes care of computing the gravity and figuring out what current an entity (presumably the player)
    /// might currently be close on enough to say "entity is on the planet".
    /// </summary>
    public class PhysicsHelper : MonoBehaviour
    {
        [SerializeField] float gravityMultiplier = 10f;
        [SerializeField] AnimationCurve gravityCurve;
        [SerializeField] float gravityMaxDistance = 10f;
        [SerializeField] bool planetGravityDependsOnRadius;

        public Vector3 centroid = Vector3.zero;

        public float GravityOnPlanet => gravityMultiplier;

        [SerializeField] List<Planet> planets = new List<Planet>();


        [Inject] private GameArea gameArea;

        public void RegisterPlanet(Planet planet)
        {
            Debug.Log($"<color=green>Add "+ planet.name + "</color>");
            planets.Add(planet);
            centroid = GetCenterOfPlanetarySystem();
        }

        public void DeregisterPlanet(Planet planet)
        {
            planets.Remove(planet);
            centroid = GetCenterOfPlanetarySystem();
        }

        public Vector3 GetGravityAtPosition(Vector3 position, float objectRadius)
        {
            var accumulatedGravity = Vector3.zero;
            foreach (var planet in planets)
            {
                var deltaToPlanet = planet.Position - position;
                var distanceToPlanet = deltaToPlanet.magnitude;
                var distanceToPlanetEdge = distanceToPlanet - planet.Radius - objectRadius;
                var percentDistanceToPlanetEdge = Mathf.Clamp01(distanceToPlanetEdge / gravityMaxDistance);
                if (percentDistanceToPlanetEdge == 1)
                    continue;

                var gravityFromPlanet = gravityCurve.Evaluate(percentDistanceToPlanetEdge) * gravityMultiplier;

                if (planetGravityDependsOnRadius)
                    gravityFromPlanet *= planet.Radius;
                
                accumulatedGravity += deltaToPlanet.normalized * gravityFromPlanet;
            }
            return accumulatedGravity;
        }

        public Planet GetCurrentPlanet(Vector3 position, float searchRadius)
        {
            foreach (var planet in planets)
            {
                var deltaToPlanet = planet.Position - position;
                var distanceToPlanet = deltaToPlanet.magnitude;
                var distanceToPlanetEdge = distanceToPlanet - planet.Radius - searchRadius;
                if (distanceToPlanetEdge <= 0f)
                    return planet;
            }

            return null;
        }

        private Vector3 GetCenterOfPlanetarySystem()
        {
            Vector3 center = Vector3.zero;
            foreach (var planet in planets)
            {
                center += planet.Position;
            }
            center = center / planets.Count;
            return center;
        }
        
        
    }
}