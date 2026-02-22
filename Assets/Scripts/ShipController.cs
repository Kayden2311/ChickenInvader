using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController Instance;

    [SerializeField] private GameObject ShipPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnShip()
    {
        if (ShipPrefabs == null)
        {
            Debug.LogError("ShipPrefabs is not assigned in ShipController!");
            return;
        }

        var newShip = Instantiate(ShipPrefabs, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.5f, 0)), Quaternion.identity);
        var point = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 0));
        point.z = 0;
        StartCoroutine(SpawnShipToPoint(newShip, point));
    }

    private IEnumerator SpawnShipToPoint(GameObject Ship, Vector3 point)
    {
        float timer = 0;
        while (Ship && Ship.transform.position != point)
        {
            timer += Time.fixedDeltaTime;
            Ship.transform.position = Vector3.Lerp(Ship.transform.position, point, timer);
            yield return new WaitForFixedUpdate();
        }
    }
}