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
        StartCoroutine(SpawnDivingChicken());
    }

    private IEnumerator SpawnDivingChicken()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (divingChickenPrefab == null) continue;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) continue;

            Vector3 targetPos = player.transform.position;
            float randomX = Random.Range(minSpawnX, maxSpawnX);
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
            
            GameObject chicken = Instantiate(divingChickenPrefab, spawnPos, Quaternion.identity);
            Vector3 direction = (targetPos - spawnPos).normalized;
            
            StartCoroutine(DiveChicken(chicken, direction));
        }
    }

    private IEnumerator DiveChicken(GameObject chicken, Vector3 direction)
    {
        if (chicken == null) yield break;

        while (chicken != null && chicken.transform.position.y > destroyY)
        {
            chicken.transform.position += direction * divingSpeed * Time.deltaTime;
            yield return null;
        }

        if (chicken != null)
        {
            Destroy(chicken);
        }
    }
}

