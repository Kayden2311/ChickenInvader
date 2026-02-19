using System.Collections;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject EggPrefabs;
    [SerializeField] private int Health = 100;
    [SerializeField] private GameObject VFX;

    public static BossScript Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEgg());
        StartCoroutine(MoveBossToRandomPoint());
    }
    public void PutDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
            var vfx = Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEgg() 
    {
        while(true)
        {
            Instantiate(EggPrefabs, transform.position, Quaternion.identity); 
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        }
    }
    IEnumerator MoveBossToRandomPoint()
    {
        Vector3 point = GetRandomPoint();
        while (transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, 0.1f);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        StartCoroutine(MoveBossToRandomPoint());
    }
    Vector3 GetRandomPoint() 
    {
        // Use viewport coordinates (0–1 range) mapped to world space
        // X: full width (0 to 1), Y: upper half of the screen (0.5 to 1)
        Vector3 posRandom = Camera.main.ViewportToWorldPoint(
            new Vector3(Random.Range(0f, 1f), Random.Range(0.5f, 1f), 0)
        );
        posRandom.z = 0;
        return posRandom;
    }
}
