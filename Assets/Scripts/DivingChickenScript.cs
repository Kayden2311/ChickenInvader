using UnityEngine;

public class DivingChickenScript : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int score = 10;
    [SerializeField] private GameObject chickenLegPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip diveSound;

    private AudioSource audioSource;
    private Vector3 moveDir;
    private float speed;
    private float destroyY;

    public void Init(Vector3 direction, float moveSpeed, float destroyPosY)
    {
        moveDir = direction.normalized;
        speed = moveSpeed;
        destroyY = destroyPosY;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameController.Instance.AddScore(score);

            if (chickenLegPrefab != null)
            {
                Instantiate(chickenLegPrefab, transform.position, Quaternion.identity);
            }
            
            if (audioSource != null && diveSound != null)
            {
                audioSource.PlayOneShot(diveSound);
            }
            
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}