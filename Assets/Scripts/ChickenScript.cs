using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(SpawnEgg());
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ChickenDie();
            Destroy(collision.gameObject);
        }
    }

    private void TryDropUpgrade()
    {
        if (upgradePrefabs.Length == 0) return;

        if (Random.value <= upgradeDropChance)
        {
            int index = Random.Range(0, upgradePrefabs.Length);
            Instantiate(upgradePrefabs[index], transform.position, Quaternion.identity);
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
        // Check if Spawner.Instance exists before calling to avoid MissingReferenceException
        if (Spawner.Instance != null)
        {
            Spawner.Instance.DecreaseChicken();
        }
    }

    public void ChickenDie()
    {
        GameController.Instance.AddScore(score);
        Instantiate(ChickenLegPrefabs, transform.position, Quaternion.identity);
        TryDropUpgrade();
        if (audioSource != null && dieSound != null)
        {
            audioSource.PlayOneShot(dieSound);
        }
        Destroy(gameObject);
    }
}