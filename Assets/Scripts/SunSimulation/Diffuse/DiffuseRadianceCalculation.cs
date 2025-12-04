using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UI.Dates;
using UnityEngine;

public class DiffuseRadianceCalculation : MonoBehaviour
{
    private List<bool> raycastHits = new List<bool>();
    private List<double> luminanceValues = new List<double>();
    //private List<double> luminanceFromObserver = new List<double>();
    private List<double> luminanceFromObserver = new List<double>();
    
    [SerializeField] private LuminanceCSVReading luminanceCSV;
    [SerializeField] private TregenzaRayCasting raycasting;

    private int year = 2025;
    private int month = 5;
    private int day = 27;
    private int hour = 12;
    private int minutes = 0;


    private void Start()
    {
        UpdateDiffuseRadiance();
    }

    public void UpdateDiffuseRadiance()
    {
        raycastHits.Clear();
        if (luminanceValues != null)
            luminanceValues.Clear();
        if (luminanceFromObserver != null)
            luminanceFromObserver.Clear();

        raycastHits = raycasting.UpdateRayCasting();

        UpdateDate();
        GetLuminanceValues();
        if (luminanceValues == null)
            return;
        SaveLuminanceFromObserver();
        CalculateDiffuseRadiance();
    }

    private void UpdateDate()
    {
        year = DataForSimulation.Year;
        month = DataForSimulation.Month;
        day = DataForSimulation.Day;
        hour = (int)DataForSimulation.Hour;
        minutes = (int)DataForSimulation.Minutes;
    }


    private void GetLuminanceValues()
    {          
        DateTime date = new DateTime(year, month, day, (int)hour, (int)minutes, 0);

        string formattedDate = date.ToString("yyyy-MM-dd HH:mm");
        Debug.Log(formattedDate);

        luminanceValues = luminanceCSV.GetValues(formattedDate);
    }

    private void SaveLuminanceFromObserver()
    {
        for(int i = 0; i < 145; i++)
        {
            if (raycastHits[i] == false)
            {
                luminanceFromObserver.Add(luminanceValues[i]);
            }
            else
            {
                luminanceFromObserver.Add(0d);
            }
        }

        raycasting.ColorPatches(luminanceFromObserver);
    }

    // PREGUNTAR
    private void CalculateDiffuseRadiance()
    {
        List<float> incidentAnglesCos = TregenzaSky.GetIncidentAngleCos(transform.up);

        double luminousEfficacy = 120.0;

        if (incidentAnglesCos.Count != luminanceFromObserver.Count)
        {
            Debug.LogWarning("La longitud de las listas de luminancia y ángulos no coinciden");
            return;
        }

        double Ev = 0d; // iluminancia en lux = lm/m2
        double dOmega = 0.0433d; // Solid angle promedio en sr

        for (int i = 0; i < luminanceFromObserver.Count; i++)
        {
            double L = luminanceFromObserver[i]; // cd/m2
            double cosTheta = Mathf.Max(0f, incidentAnglesCos[i]);   

            Ev += L * cosTheta * dOmega;
        }

        // Convierte iluminancia en lux a irradiancia energética en W/m2
        double Ee = Ev / luminousEfficacy;
    }

    #region Debugging
    private void PrintList(List<double> list)
    {
        Debug.Log(string.Join(", ", list));
    }

    private void PrintList(List<float> list)
    {
        Debug.Log(string.Join(", ", list));
    }
    #endregion
}
