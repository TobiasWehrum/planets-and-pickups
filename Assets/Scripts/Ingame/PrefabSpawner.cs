using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Spawns a prefab each x seconds using a either Zenject directly or the <see cref="Pool"/>. Used e.g. for pickups and enemies.
    /// </summary>
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] float spawnDelay = 5f;
        [SerializeField] [Range(0, 1)] float startPercent;
        [SerializeField] bool usePool;

        [Inject] Constants constants;
        [Inject] DiContainer diContainer;
        [Inject] Pool pool;

        float spawnCountdown;

        void Awake()
        {
            spawnCountdown = startPercent * spawnDelay;
        }
        
        void Update()
        {
            spawnCountdown -= Time.deltaTime;
            if (spawnCountdown <= 0)
            {
                spawnCountdown += spawnDelay;
                Spawn();
            }
        }

        void Spawn()
        {
            var distanceFromCenter = constants.playfieldRadius;
            var angleRad = Random.value * Mathf.PI * 2;
            var position = new Vector3(Mathf.Cos(angleRad) * distanceFromCenter, Mathf.Sin(angleRad) * distanceFromCenter, 0);

            if (usePool)
            {
                pool.Get(prefab, position, Quaternion.identity, transform);
            }
            else
            {
                diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, transform);
            }
        }
    }
}