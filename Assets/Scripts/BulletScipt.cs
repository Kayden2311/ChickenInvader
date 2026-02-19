using UnityEngine;

public class BulletScipt : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float DistancesDestroy;
    [SerializeField] private int baseDamage = 10;
    private int currentDamage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DistancesDestroy = 10;
        currentDamage = baseDamage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * Speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            BossScript.Instance.PutDamage(currentDamage);
            Destroy(gameObject);
        }

    }

    public void SetBulletTier(int tier)
    {
        if (tier < 0)
        {
            tier = 0;
        }

        currentDamage = baseDamage * (tier + 1);
    }
}
