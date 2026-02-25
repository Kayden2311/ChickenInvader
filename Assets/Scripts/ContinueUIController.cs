using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ContinueUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;

    [Header("Settings")]
    [SerializeField] private int continueCost = 5000;

    public static ContinueUIController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (continuePanel != null)
        {
            continuePanel.SetActive(false);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (costText != null)
        {
            costText.text = $"Cost: {continueCost}";
        }
    }

    public void ShowContinueScreen()
    {
        Debug.Log("[ContinueUIController] ShowContinueScreen called!");
        
        if (continuePanel != null)
        {
            Debug.Log("[ContinueUIController] Activating continue panel...");
            continuePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("[ContinueUIController] continuePanel is NULL! Please assign it in Inspector!");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int currentScore = GameController.Instance.Score;

        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }

        // Enable/disable continue button based on score
        if (continueButton != null)
        {
            continueButton.interactable = currentScore >= continueCost;
        }
    }

    private void OnContinueClicked()
    {
        int currentScore = GameController.Instance.Score;

        if (currentScore >= continueCost)
        {
            // Deduct score
            GameController.Instance.AddScore(-continueCost);

            // Give player 1 life
            GameController.Instance.PlayerLives = 1;

            // Hide continue panel
            if (continuePanel != null)
            {
                continuePanel.SetActive(false);
            }

            // Resume game
            Time.timeScale = 1f;

            // Change state back to playing or boss fight
            if (BossScript.Instance != null)
            {
                GameController.Instance.ChangeState(GameState.BossFight);
            }
            else
            {
                GameController.Instance.ChangeState(GameState.Playing);
            }

            // Spawn new ship
            if (ShipController.Instance != null)
            {
                ShipController.Instance.SpawnShip();
            }
        }
    }

    private void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
