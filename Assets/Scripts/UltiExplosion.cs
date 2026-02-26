using System.Collections;
using UnityEngine;

public class UltiExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float delayBeforeExplode = 0.3f;

    private bool hasExploded = false;

    private void Start()
    {
        StartCoroutine(MoveToCenterAndExplode());
    }

    private IEnumerator MoveToCenterAndExplode()
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        center.z = 0;

        while (Vector3.Distance(transform.position, center) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                center,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeExplode);

        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        GameObject[] chickens = GameObject.FindGameObjectsWithTag("Chicken");
        foreach (GameObject chicken in chickens)
        {
            var script = chicken.GetComponent<ChickenScript>();
            if (script != null)
                script.DieByUlti();
            else
                Destroy(chicken);
        }

        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            var bossScript = boss.GetComponent<BossScript>();
            if (bossScript != null)
                bossScript.PutDamage(99999);
        }

        Destroy(gameObject);
    }
}