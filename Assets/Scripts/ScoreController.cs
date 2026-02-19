using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score;
    public static ScoreController Instance;
    void Awake()
    {
        Instance = this;
    }
    public void GetScore(int score)
    {
        this.score += score;
        scoreText.text = "Score: " + this.score.ToString();
    }
}
