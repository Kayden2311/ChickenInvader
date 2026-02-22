using System.Collections;
using UnityEngine;

public class DivingChickenScript: MonoBehaviour
{
    [Header("=== DROP ===")]
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private float minDropTime = 2f;
    [SerializeField] private float maxDropTime = 5f;

    [Header("=== SCORE ===")]
    [SerializeField] private int score = 10;
    [SerializeField] private GameObject chickenLegPrefab;

    private Vector3 moveDir;
    private float speed;
    private float destroyY;

    public void Init(Vector3 direction, float moveSpeed, float destroyPosY)
    {
        moveDir = direction.normalized;
        speed = moveSpeed;
        destroyY = destroyPosY;

        StartCoroutine(DropEggRoutine());
    }

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DropEggRoutine()
    {
        while (true)
        {
            float t = Random.Range(minDropTime, maxDropTime);
            yield return new WaitForSeconds(t);

            if (eggPrefab != null)
                Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameController.Instance.AddScore(score);

            if (chickenLegPrefab != null)
                Instantiate(chickenLegPrefab, transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}