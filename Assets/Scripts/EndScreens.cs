using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreens : MonoBehaviour
{
    //Win and lose panels on the Canvas
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    //Win and Lose sounds
    [SerializeField] private AudioClip winSound;
    [SerializeField] private float winVolume = 0.8f;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private float loseVolume = 0.8f;
    void Start()
    {
        //Hide both panels when the game starts
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    //Called by the exit door; show win screen and freeze the game
    public void ShowWin()
    {
        SoundManager.Instance.Play(winSound, winVolume);
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    //Called by player health at zero; show lose screen and freeze the game
    public void ShowLose()
    {
        SoundManager.Instance.Play(loseSound, loseVolume);
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    //Play Again button; unfreeze time before reloading the level
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Quit button; closes the built game (does nothing in the editor)
    public void Quit()
    {
        Application.Quit();
    }
}
