using UnityEngine;
using TMPro;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Speed")]
    [SerializeField] float startSpeed = 4.5f;
    [SerializeField] float speedIncreasePerSecond = 0.35f;
    [SerializeField] float maxSpeed = 18f;

    [Header("Despawn")]
    [SerializeField] float despawnX = -20f;

    [Header("Score")]
    [SerializeField] float scorePerSecond = 10f;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    const string HighScoreKey = "HIGH_SCORE";

    float currentSpeed;
    float score;
    float highScore;
    bool isGameOver;

    public float CurrentSpeed => currentSpeed;
    public float DespawnX => despawnX;
    public float Score => score;
    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentSpeed = startSpeed;
        score = 0f;
        isGameOver = false;

        highScore = PlayerPrefs.GetFloat(HighScoreKey, 0f);

        Time.timeScale = 1f;

        UpdateScoreUI();
        UpdateHighScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        currentSpeed += speedIncreasePerSecond * Time.deltaTime;
        if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;

        score += scorePerSecond * Time.deltaTime;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreUI();
        }

        UpdateScoreUI();
    }

    public void AddScore(float amount)
    {
        if (isGameOver) return;

        score += amount;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreUI();
        }

        UpdateScoreUI();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreUI();
        }

        Time.timeScale = 0f;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "High Score: " + Mathf.FloorToInt(highScore).ToString();
    }

    public void ResetHighScore()
    {
        highScore = 0f;
        PlayerPrefs.SetFloat(HighScoreKey, 0f);
        PlayerPrefs.Save();
        UpdateHighScoreUI();
    }
}
