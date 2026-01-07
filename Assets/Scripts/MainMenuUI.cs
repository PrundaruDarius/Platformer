using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject creditsPanel;

    [Header("Scene Names")]
    [SerializeField] string gameSceneName = "Main";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
