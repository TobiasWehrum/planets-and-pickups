using UnityEngine;
using Zenject;

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

        void Awake()
        {
            moveInDirectionWhileAvoidingPlanets = GetComponent<MoveInDirectionWhileAvoidingPlanets>();
        }

        void Update()
        {
            if (!player.isActiveAndEnabled)
                return;
            
            var deltaToPlayer = player.transform.position - transform.position;
            moveInDirectionWhileAvoidingPlanets.UpdateMainDirection(deltaToPlayer.normalized);
        }
    }
}