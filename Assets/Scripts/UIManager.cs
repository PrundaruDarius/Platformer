using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text finalScoreText;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        scoreText.text = "SCORE: " + Mathf.FloorToInt(GameManager.Instance.Score).ToString();

        if (GameManager.Instance.IsGameOver)
        {
            if (gameOverPanel != null && !gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);

                if (finalScoreText != null)
                    finalScoreText.text = "FINAL SCORE: " + Mathf.FloorToInt(GameManager.Instance.Score).ToString();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
