using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
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