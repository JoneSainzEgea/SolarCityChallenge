using System.Collections;
using System.Collections.Generic;
using UI.Dates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInteractions : MonoBehaviour
{
    [SerializeField] ChangeSkybox changeSkybox;
    [Header("Canvases")]
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private GameObject modeSelectionCanvas;
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject timeSelectionPanel;
    [SerializeField] private GameObject firstTutorialPanel;

    private void Start()
    {
        InitialConfig();
    }

    public void InitialConfig()
    {
        changeSkybox.OnChangeSkybox(true);
        uiCanvas.SetActive(true);
        modeSelectionCanvas.SetActive(false);

        mainMenuPanel.SetActive(true);

        settingsPanel.SetActive(false);
        confirmationPanel.SetActive(false);
        mapSelectionPanel.SetActive(false);
        timeSelectionPanel.SetActive(false);
        firstTutorialPanel.SetActive(false);
    }

    public void StartGame()
    {
        changeSkybox.OnChangeSkybox(false);
        uiCanvas.SetActive(false);
        modeSelectionCanvas.SetActive(true);
        mapSelectionPanel.SetActive(true);
    }

    public void OpenTimeSelection()
    {
        mapSelectionPanel.SetActive(false);
        timeSelectionPanel.SetActive(true);
    }

    public void ReturnToMap()
    {
        timeSelectionPanel.SetActive(false);
        mapSelectionPanel.SetActive(true);
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

    public void ConfirmDate()
    {
        timeSelectionPanel.SetActive(false);
        firstTutorialPanel.SetActive(true);
    }

    public void ConfirmStartGame()
    {
        changeSkybox.OnChangeSkybox(true);
        SceneManager.LoadScene(1);
    }
}
