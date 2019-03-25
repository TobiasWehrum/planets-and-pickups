using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A helper class to <see cref="Pool"/>. Stores the original prefab so that the instance
    /// can be categorized properly when it is released.
    /// </summary>
    public class PooledObjectInfo : MonoBehaviour
    {
        public GameObject Prefab { get; set; }
    }
}