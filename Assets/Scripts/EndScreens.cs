using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreens : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
