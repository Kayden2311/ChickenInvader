using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    public UpgradeType upgradeType;

    [SerializeField] private float fallSpeed = 2f;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShipScript ship = collision.GetComponent<ShipScript>();

            if (ship != null)
            {
                ship.ApplyUpgrade(upgradeType);
            }

            Destroy(gameObject);
        }
    }
}