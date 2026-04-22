using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UI.Dates;
using UnityEngine;

public class UIOptions : MonoBehaviour
{
    [SerializeField] private DatePicker datePicker;
    [SerializeField] private GameObject exitPanel;
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            OpenExitPanel();
        }
    }

    public void UpdateDateValue()
    {
        int year = datePicker.SelectedDate.Date.Year;
        int month = datePicker.SelectedDate.Date.Month;
        int day = datePicker.SelectedDate.Date.Day;

        DataForSimulation.UpdateDate(year, month, day);
    }

    public void OpenExitPanel()
    {
        exitPanel.SetActive(true);
    }
    public void CloseExitPanel()
    {
        exitPanel.SetActive(false);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif  
    }
}
