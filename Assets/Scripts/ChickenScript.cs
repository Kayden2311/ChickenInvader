using System.Collections;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] private GameObject EggPrefabs;
    [SerializeField] private int score;
    [SerializeField] private GameObject ChickenLegPrefabs;

    [Header("Upgrade Drop")]
    [SerializeField] private GameObject[] upgradePrefabs;
    [SerializeField] private float upgradeDropChance = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioClip dieSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnEgg());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ChickenDie();
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator SpawnEgg()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 15));
            Instantiate(EggPrefabs, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (Spawner.Instance != null)
        {
            Spawner.Instance.DecreaseChicken();
        }
    }

    public void ChickenDie()
    {
        GameController.Instance.AddScore(score);

        if (ChickenLegPrefabs != null)
            Instantiate(ChickenLegPrefabs, transform.position, Quaternion.identity);

        TryDropUpgrade();

        if (audioSource != null && dieSound != null)
            audioSource.PlayOneShot(dieSound);

        Destroy(gameObject);
    }

    public void DieByUlti()
    {
        StopAllCoroutines();

        if (audioSource != null && dieSound != null)
            audioSource.PlayOneShot(dieSound);

        Destroy(gameObject);
    }

    private void TryDropUpgrade()
    {
        if (upgradePrefabs == null || upgradePrefabs.Length == 0) return;

        if (Random.value <= upgradeDropChance)
        {
            int index = Random.Range(0, upgradePrefabs.Length);
            Instantiate(upgradePrefabs[index], transform.position, Quaternion.identity);
        }
    }
}