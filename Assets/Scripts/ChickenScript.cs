using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] private GameObject EggPrefabs;
    [SerializeField] private int score;
    [SerializeField] private GameObject ChickenLegPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(SpawnEgg());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameController.Instance.AddScore(score);
            Instantiate(ChickenLegPrefabs, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
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
        Spawner.Instance.DecreaseChicken();
    }
}