using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuInteractions : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuConfirmationPanel;
    [SerializeField] private GameObject quitConfirmationPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseUI.SetActive(isPaused);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OpenMainMenuConfirmation()
    {
        pausePanel.SetActive(false);
        mainMenuConfirmationPanel.SetActive(true);
    }

    public void OpenQuitConfirmation()
    {
        pausePanel.SetActive(false);
        quitConfirmationPanel.SetActive(true);
    }

    public void CloseMainMenuConfirmation()
    {
        mainMenuConfirmationPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void CloseQuitConfirmation()
    {
        quitConfirmationPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OnConfirmedMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnConfirmedQuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
