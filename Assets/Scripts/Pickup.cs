using UnityEngine;

namespace MiniPlanetDefense
{
    public class Pickup : MonoBehaviour
    {
        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}