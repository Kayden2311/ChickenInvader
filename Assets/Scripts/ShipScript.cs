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
    [SerializeField] private AudioClip firesound;
    private AudioSource audioSource;

    [Header("Laser")]
    [SerializeField] private GameObject laserBeamPrefab;
    [SerializeField] private float laserDuration = 5f;
    [SerializeField] private Transform laserPoint;

    private GameObject activeLaser;
    private bool isLaserActive = false;

    [Header("Ulti")]
    [SerializeField] private GameObject ultiPrefab;
    [SerializeField] private int ultiCost = 7000;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DisableeShield());
    }

    private void Update()
    {
        Move();
        Fire();
        UseUlti();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0);
        transform.position += direction.normalized * Speed * Time.deltaTime;

        Vector3 TopLeftPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -TopLeftPoint.x, TopLeftPoint.x),
            Mathf.Clamp(transform.position.y, -TopLeftPoint.y, TopLeftPoint.y),
            0);
    }

    private void Fire()
    {
        if (isLaserActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            var bulletObj = Instantiate(
                BulletList[CurrentBulletTier],
                transform.position,
                Quaternion.identity);

            var bulletScript = bulletObj.GetComponent<BulletScipt>();
            if (bulletScript != null)
                bulletScript.SetBulletTier(CurrentBulletTier);

            if (audioSource != null && firesound != null)
                audioSource.PlayOneShot(firesound);
        }
    }

    private void UseUlti()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ultiPrefab == null) return;

            if (GameController.Instance.Score >= ultiCost)
            {
                GameController.Instance.DeductScore(ultiCost);
                Instantiate(ultiPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator DisableeShield()
    {
        yield return new WaitForSeconds(5f);
        if (Shield != null)
            Shield.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Shield.activeSelf &&
            (collision.CompareTag("Egg") || collision.CompareTag("Chicken")))
        {
            Die();
        }
        else if (collision.CompareTag("ChickenLeg"))
        {
            Destroy(collision.gameObject);
            GameController.Instance.AddScore(ScoreChickenLeg);

            currentChickenLegCount++;

            if (currentChickenLegCount >= chickenLegsPerTier)
            {
                currentChickenLegCount = 0;

                if (CurrentBulletTier < BulletList.Length - 1)
                    CurrentBulletTier++;
            }
        }
    }

    private void Die()
    {
        if (VFX != null)
        {
            var vfx = Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }

        GameController.Instance.PlayerDied();
        Destroy(gameObject);
    }

    public void ActivateShield(float duration)
    {
        if (Shield != null)
        {
            Shield.SetActive(true);
            StartCoroutine(DisableShieldAfter(duration));
        }
    }

    private IEnumerator DisableShieldAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (Shield != null)
            Shield.SetActive(false);
    }

    public void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Shield:
                ActivateShield(5f);
                break;

            case UpgradeType.BulletTierUp:
                if (CurrentBulletTier < BulletList.Length - 1)
                    CurrentBulletTier++;
                break;

            case UpgradeType.LaserBeam:
                ActivateLaser();
                break;
        }
    }

    private void ActivateLaser()
    {
        if (isLaserActive) return;
        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        isLaserActive = true;

        activeLaser = Instantiate(
            laserBeamPrefab,
            laserPoint.position,
            Quaternion.identity,
            laserPoint);

        yield return new WaitForSeconds(laserDuration);

        if (activeLaser != null)
            Destroy(activeLaser);

        isLaserActive = false;
    }
}