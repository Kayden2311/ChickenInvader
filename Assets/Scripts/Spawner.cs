using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float gridSize = 1;
    private Vector3 SpawnPos;
    private int currentChicken;
    private bool bossSpawned = false;
    
    [SerializeField] private GameObject ChickenPrefabs;
    [SerializeField] private Transform ChickenGrid;
    [SerializeField] private GameObject BossPrefab;

    public static Spawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        SpawnPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        // Calculate the grid size based on the screen dimensions
        SpawnPos.x += ((gridSize / 2 + (screenWidth / 4)));
        SpawnPos.y -= gridSize;
        SpawnPos.z = 0;

        SpawnChicken(Mathf.FloorToInt(screenHeight / 2 / gridSize), Mathf.FloorToInt(screenWidth / gridSize / 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnChicken(int row, int numberChicken) 
    {
        float x = SpawnPos.x;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < numberChicken; j++)
            {
                SpawnPos.x = SpawnPos.x + gridSize;
                GameObject Chicken = Instantiate(ChickenPrefabs, SpawnPos, Quaternion.identity);
                Chicken.transform.parent = ChickenGrid;
                currentChicken++;
            }
            SpawnPos.x = x;
            SpawnPos.y -= gridSize;
        }
        
    }
    public void DecreaseChicken()
    {
        currentChicken--;
        Debug.Log($"Grid chicken count: {currentChicken}");
        
        if (currentChicken <= 0 && BossPrefab != null && !bossSpawned)
        {
            // Only spawn boss once
            if (GameController.Instance.CurrentState != GameState.BossFight)
            {
                bossSpawned = true;
                // Spawn the boss prefab when all chickens are destroyed
                Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.1f, 0));
                spawnPos.z = 0;
                Instantiate(BossPrefab, spawnPos, Quaternion.identity);
                GameController.Instance.ChangeState(GameState.BossFight);
            }
        }
    }
}
