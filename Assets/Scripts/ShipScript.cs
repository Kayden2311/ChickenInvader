using System.Collections;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private GameObject[] BulletList;
    [SerializeField] private int CurrentBulletTier;
    [SerializeField] private GameObject VFX;
    [SerializeField] private GameObject Shield;
    [SerializeField] private int ScoreChickenLeg;
    [SerializeField] private int chickenLegsPerTier = 10;
    private int currentChickenLegCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisableeShield());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0);
        //Normalize the direction vector to ensure consistent speed (direction == 1) in all directions
        transform.position += direction.normalized * Speed * Time.deltaTime;

        //Limit moving space
        Vector3 TopLeftPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -TopLeftPoint.x, TopLeftPoint.x), Mathf.Clamp(transform.position.y, -TopLeftPoint.y, TopLeftPoint.y), 0);
    }

    void Fire() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            var bulletObj = Instantiate(BulletList[CurrentBulletTier], transform.position, Quaternion.identity);
            var bulletScript = bulletObj.GetComponent<BulletScipt>();
            if (bulletScript != null)
            {
                bulletScript.SetBulletTier(CurrentBulletTier);
            }
        }
    }
    IEnumerator DisableeShield()
    {
        yield return new WaitForSeconds(10);
        Shield.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Shield.activeSelf && (collision.CompareTag("Egg") || collision.CompareTag("Chicken")))
        {
            Destroy(gameObject);
        } else if (collision.CompareTag("ChickenLeg"))
        {
            Destroy(collision.gameObject);
            ScoreController.Instance.GetScore(ScoreChickenLeg);
            currentChickenLegCount++;
            if (currentChickenLegCount >= chickenLegsPerTier)
            {
                currentChickenLegCount = 0;
                if (CurrentBulletTier < BulletList.Length - 1)
                {
                    CurrentBulletTier++;
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            var vfx = Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
            ShipController.Instance.SpawnShip();
        }
    }
}
