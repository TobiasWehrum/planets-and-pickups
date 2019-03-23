using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniPlanetDefense
{
    public class PhysicsHelper : MonoBehaviour
    {
        [SerializeField] float gravity;

        public float GravityOnPlanet => gravity;
    }
}