using UnityEngine;

namespace MiniPlanetDefense
{
    public class PlayerLaser : MonoBehaviour
    {
        [SerializeField] LineRenderer targetingLineRender;
        [SerializeField] LineRenderer laserLineRenderer;
        [SerializeField] float distance = 100;

        void Awake()
        {
            laserLineRenderer.enabled = false;
        }
        
        void Update()
        {
            var playerPosition = transform.position;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            var targetDirection = (mousePosition - playerPosition).normalized;
            if (targetDirection.sqrMagnitude == 0)
            {
                targetDirection.x = 1;
            }

            var targetPosition = playerPosition + targetDirection * distance;
        
            targetingLineRender.SetPosition(0, playerPosition);
            targetingLineRender.SetPosition(1, targetPosition);
        }
    }
}