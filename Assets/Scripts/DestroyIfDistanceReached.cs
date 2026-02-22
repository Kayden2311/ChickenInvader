using UnityEngine;

public class DestroyIfDistanceReached : MonoBehaviour
{
    [SerializeField] private float Distances;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyIfTrue();
    }
    void DestroyIfTrue()
    {
        Vector3 CenterScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2), 0);
        if (Vector3.Distance(transform.position, CenterScreen) > Distances)
            Destroy(this.gameObject);
    }
}
