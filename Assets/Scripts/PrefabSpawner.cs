using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MiniPlanetDefense
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] float spawnDelay = 5f;

        [Inject] Constants constants;
        [Inject] DiContainer diContainer;

        float spawnCountdown;

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

            diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, transform);
        }
    }
}