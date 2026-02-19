using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] private GameObject EggPrefabs;
    [SerializeField] private int score;
    [SerializeField] private GameObject ChickenLegPrefabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEgg());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ScoreController.Instance.GetScore(score);
            Instantiate(ChickenLegPrefabs, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
        }
    }

    IEnumerator SpawnEgg() 
    {
        while(true)
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
