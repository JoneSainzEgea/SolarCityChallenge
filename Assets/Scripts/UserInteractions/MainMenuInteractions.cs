using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInteractions : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmationPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        mainMenuPanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    public void Cancel()
    {
        confirmationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnConfirmedQuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
