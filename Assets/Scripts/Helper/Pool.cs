using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// An all-purpose pool class, able to spawn objects with Instantiate()-like signatures and allowing to release them again so they can be reused.
    /// </summary>
    public class Pool : MonoBehaviour
    {
        [Inject] DiContainer diContainer;
        
        Dictionary<GameObject, List<GameObject>> instancesByPrefab = new Dictionary<GameObject, List<GameObject>>();

        public GameObject Get(GameObject prefab)
        {
            var instance = GetExistingInstance(prefab);
            if (instance != null)
                return instance;
            
            instance = diContainer.InstantiatePrefab(prefab);
                
            var pooledObjectInfo = instance.gameObject.AddComponent<PooledObjectInfo>();
            pooledObjectInfo.Prefab = prefab;
                
            instance.name = instance.name + " " + Time.frameCount;

            return instance;
        }

        public GameObject Get(GameObject prefab, Transform parent)
        {
            var instance = Get(prefab);
            instance.transform.parent = parent;
            return instance;
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instance = Get(prefab);
            var transform = instance.transform;
            transform.parent = parent;
            transform.position = position;
            transform.rotation = rotation;
            return instance;
        }
        
        public T Get<T>(T prefab) where T : Component
        {
            return Get(prefab.gameObject).GetComponent<T>();
        }

        public T Get<T>(T prefab, Transform parent) where T : Component
        {
            return Get(prefab.gameObject, parent).GetComponent<T>();
        }

        public T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            return Get(prefab.gameObject, position, rotation, parent).GetComponent<T>();
        }

        public void Release(GameObject instance)
        {
            var pooledObjectInfo = instance.GetComponent<PooledObjectInfo>();
            if (pooledObjectInfo == null)
            {
                Debug.LogError("Can't find a PooledObjectInfo to release " + instance.name);
                return;
            }

            instance.SetActive(false);
            instance.transform.parent = transform;
            
            var instances = GetInstances(pooledObjectInfo.Prefab);
            instances.Add(instance);
        }

        GameObject GetExistingInstance(GameObject prefab)
        {
            var instances = GetInstances(prefab);
            if (instances.Count == 0)
                return null;
            
            var index = instances.Count - 1;
            var instance = instances[index];
            instances.RemoveAt(index);
            instance.gameObject.SetActive(true);
            return instance;
        }

        List<GameObject> GetInstances(GameObject prefab)
        {
            List<GameObject> instances;
            if (!instancesByPrefab.TryGetValue(prefab, out instances))
            {
                instances = new List<GameObject>();
                instancesByPrefab[prefab] = instances;
            }

            return instances;
        }
    }
}