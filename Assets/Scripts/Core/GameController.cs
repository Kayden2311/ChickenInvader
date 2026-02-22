using System.Collections;
using System.Threading;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameState CurrentState { get; private set; }

    public int Score { get; private set; }
    public int PlayerLives = 3;

    [Header("Music Clips")]
    [SerializeField] private AudioClip gameMusic;

    [SerializeField] private AudioClip bossMusic;

    [SerializeField] private AudioClip winMusic;

    [SerializeField] private AudioClip overMusic;

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
        ChangeState(GameState.Playing);
        UpdateScoreUI();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.Playing:
                MusicManager.Instance.FadeTo(gameMusic, 1.5f);
                break;

            case GameState.BossFight:
                MusicManager.Instance.FadeTo(bossMusic, 1.5f);
                break;

            case GameState.Win:
                MusicManager.Instance.FadeTo(winMusic, 0.5f);
                Time.timeScale = 0.5f; // Slow down time for dramatic effect
                StartCoroutine(ResetTimeScaleCoroutine(5f));
                //Time.timeScale = 0f; // Pause the game
                break;

            case GameState.GameOver:
                MusicManager.Instance.FadeTo(overMusic, 0.2f);
                Time.timeScale = 0f; // Pause the game
                break;
        }
    }

    private IEnumerator ResetTimeScaleCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (ScoreController.Instance != null)
        {
            ScoreController.Instance.UpdateScore(Score);
        }
    }

    public void PlayerDied()
    {
        PlayerLives--;
        if (PlayerLives > 0)
        {
            ShipController.Instance.SpawnShip();
        }
        else
        {
            ChangeState(GameState.GameOver);
        }
    }
}