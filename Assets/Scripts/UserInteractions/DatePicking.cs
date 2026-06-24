using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UI.Dates;
using UnityEngine;

public class DatePicking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private DatePicker datePicker;
    int year, month, day;

    private void Start()
    {
        year = datePicker.VisibleDate.Date.Year;
        month = datePicker.VisibleDate.Date.Month;
        day = datePicker.VisibleDate.Date.Day;

        dateText.text = $"Fecha seleccionada: {day}/{month}/{year}";       
        GameManager.Instance.UpdateDate(year, month, day);
    }

    public void UpdateDateValue()
    {
        year = datePicker.SelectedDate.Date.Year;
        month = datePicker.SelectedDate.Date.Month;
        day = datePicker.SelectedDate.Date.Day;

        dateText.text = $"Fecha seleccionada: {day}/{month}/{year}";

        GameManager.Instance.UpdateDate(year, month, day);
    }
}
