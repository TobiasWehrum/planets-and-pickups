using System;
using UnityEngine;
using Zenject;
using Zchfvy.Plus;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Steers a <see cref="MoveInDirectionWhileAvoidingPlanets"/> script so that the main direction is always pointing towards the player.
    /// </summary>
    [RequireComponent(typeof(MoveInDirectionWhileAvoidingPlanets))]
    public class SteerMoveInDirectionTowardsPlayer : MonoBehaviour
    {
        [Inject] Player player;
        
        MoveInDirectionWhileAvoidingPlanets moveInDirectionWhileAvoidingPlanets;
        private Vector3 dirToPlayer;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (dirToPlayer!=null)
                GizmosPlus.Arrow(transform.position, dirToPlayer);
        }

        void Awake()
        {
            moveInDirectionWhileAvoidingPlanets = GetComponent<MoveInDirectionWhileAvoidingPlanets>();
        }

        void Update()
        {
            if (!player.isActiveAndEnabled)
                return;
            
            dirToPlayer = player.transform.position - transform.position;
            moveInDirectionWhileAvoidingPlanets.UpdateMainDirection(dirToPlayer.normalized);
        }
    }
}