using System.Collections;
using UnityEngine;

public class DivingChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject divingChickenPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minSpawnX = -9f;
    [SerializeField] private float maxSpawnX = 9f;
    [SerializeField] private float divingSpeed = 5f;
    [SerializeField] private float destroyY = -10f;

    private void Start()
    {
        Debug.Log("[DivingChickenSpawner] Started!");
        Debug.Log($"[DivingChickenSpawner] Spawn interval: {spawnInterval}s");
        Debug.Log($"[DivingChickenSpawner] X range: {minSpawnX} to {maxSpawnX}");
        Debug.Log($"[DivingChickenSpawner] Diving speed: {divingSpeed}");
        Debug.Log($"[DivingChickenSpawner] Destroy Y: {destroyY}");
        
        if (divingChickenPrefab == null)
        {
            Debug.LogError("[DivingChickenSpawner] divingChickenPrefab is NULL! Please assign it in Inspector!");
        }
        else
        {
            Debug.Log($"[DivingChickenSpawner] Prefab assigned: {divingChickenPrefab.name}");
        }
        
        StartCoroutine(SpawnDivingChicken());
    }

    private IEnumerator SpawnDivingChicken()
    {
        int spawnCount = 0;
        
        while (true)
        {
            Debug.Log($"[DivingChickenSpawner] Waiting {spawnInterval}s before next spawn...");
            yield return new WaitForSeconds(spawnInterval);

            spawnCount++;
            Debug.Log($"[DivingChickenSpawner] === Spawn attempt #{spawnCount} ===");

            if (divingChickenPrefab == null)
            {
                Debug.LogError("[DivingChickenSpawner] Prefab is NULL! Skipping spawn.");
                continue;
            }

            // Find player each time
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("[DivingChickenSpawner] Player not found! Make sure player has 'Player' tag. Skipping spawn.");
                continue;
            }

            // Get player's current position
            Vector3 targetPos = player.transform.position;
            Debug.Log($"[DivingChickenSpawner] Player position: {targetPos}");
            
            // Spawn at random X position at spawner's Y
            float randomX = Random.Range(minSpawnX, maxSpawnX);
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
            Debug.Log($"[DivingChickenSpawner] Spawn position: {spawnPos}");
            
            GameObject chicken = Instantiate(divingChickenPrefab, spawnPos, Quaternion.identity);
            Debug.Log($"[DivingChickenSpawner] Chicken spawned: {chicken.name}");
            
            // Calculate direction from spawn to target
            Vector3 direction = (targetPos - spawnPos).normalized;
            Debug.Log($"[DivingChickenSpawner] Direction: {direction}");
            
            // Start diving coroutine with direction
            StartCoroutine(DiveChicken(chicken, direction, spawnCount));
        }
    }

    private IEnumerator DiveChicken(GameObject chicken, Vector3 direction, int id)
    {
        if (chicken == null)
        {
            Debug.LogWarning($"[DivingChickenSpawner] Chicken #{id} is NULL at start!");
            yield break;
        }

        Debug.Log($"[DivingChickenSpawner] Chicken #{id} diving started...");
        
        float travelTime = 0f;
        while (chicken != null && chicken.transform.position.y > destroyY)
        {
            chicken.transform.position += direction * divingSpeed * Time.deltaTime;
            travelTime += Time.deltaTime;
            yield return null;
        }

        if (chicken != null)
        {
            Debug.Log($"[DivingChickenSpawner] Chicken #{id} reached destroy Y after {travelTime:F2}s. Destroying...");
            Destroy(chicken);
        }
        else
        {
            Debug.Log($"[DivingChickenSpawner] Chicken #{id} was destroyed externally after {travelTime:F2}s.");
        }
    }
}
