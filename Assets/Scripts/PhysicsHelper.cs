using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniPlanetDefense
{
    public class PhysicsHelper : MonoBehaviour
    {
        [SerializeField] float gravityMultiplier = 10f;
        [SerializeField] AnimationCurve gravityCurve;
        [SerializeField] float gravityMaxDistance = 10f;
        [SerializeField] bool planetGravityDependsOnRadius;

        public float GravityOnPlanet => gravityMultiplier;

        List<Planet> planets = new List<Planet>();

        public void RegisterPlanet(Planet planet)
        {
            planets.Add(planet);
        }

        public void DeregisterPlanet(Planet planet)
        {
            planets.Remove(planet);
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
    }
}