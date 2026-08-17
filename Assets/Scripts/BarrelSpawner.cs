using System.Collections;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(delay);

            if (barrel != null)
            {
                Instantiate(barrel, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
