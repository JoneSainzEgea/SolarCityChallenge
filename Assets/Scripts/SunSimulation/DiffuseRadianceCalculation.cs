using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DiffuseRadianceCalculation : MonoBehaviour
{
    private List<bool> raycastHits = new List<bool>();
    private List<double> luminanceValues = new List<double>();
    private List<double> luminanceFromObserver = new List<double>();

    [Header("Date")]
    private int year = 2025;
    [Range(5,7)][SerializeField] private int month;
    [Range(1, 31)][SerializeField] private int day;
    [Range(6, 23)][SerializeField] private int hour;
    [Range(0, 55)][SerializeField] private int minutes;

    private void Start()
    {
        GetTregenzaRayCasting();
        GetLuminanceValues();
        SaveLuminanceFromObserver();
        CalculateDiffuseRadiance();
    }

    private void GetTregenzaRayCasting()
    {
        raycastHits.Clear();
        raycastHits = GetComponent<TregenzaRayCasting>().UpdatePatches();
    }

    private void GetLuminanceValues()
    {
        DateTime date = new DateTime(year, month, day, hour, minutes, 0);

        string formattedDate = date.ToString("yyyy-MM-dd HH:mm");

        luminanceValues = LuminanceCSVReading.GetValues(formattedDate);

        Debug.Log(formattedDate);
    }

    private void SaveLuminanceFromObserver()
    {
        for(int i = 0; i < 145; i++)
        {
            if (raycastHits[i] == false)
            {
                luminanceFromObserver.Add(luminanceValues[i]);
            }
        }
    }

    private void CalculateDiffuseRadiance()
    {
        // Preguntar
    }
}
