using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DiffuseRadianceCalculation : MonoBehaviour
{
    private List<bool> raycastHits = new List<bool>();
    private List<double> luminanceValues = new List<double>();
    //private List<double> luminanceFromObserver = new List<double>();
    private List<double> luminanceFromObserver = new List<double>();

    [SerializeField] private LuminanceCSVReading luminanceCSV;
    [SerializeField] private TregenzaRayCasting raycasting;

    [Header("Date")]
    private int year = 2025;
    [Range(5,7)][SerializeField] private int month;
    [Range(1, 31)][SerializeField] private int day;
    [Range(6, 23)][SerializeField] private int hour;
    [Range(0, 55)][SerializeField] private int minutes;

    private void Start()
    {
        UpdateDiffuseRadiance();
    }

    public void UpdateDiffuseRadiance()
    {
        raycastHits.Clear();
        luminanceValues.Clear();
        luminanceFromObserver.Clear();

        raycastHits = raycasting.UpdateRayCasting();

        GetLuminanceValues();
        SaveLuminanceFromObserver();
        CalculateDiffuseRadiance();
    }


    private void GetLuminanceValues()
    {
        DateTime date = new DateTime(year, month, day, hour, minutes, 0);

        string formattedDate = date.ToString("yyyy-MM-dd HH:mm");

        luminanceValues = luminanceCSV.GetValues(formattedDate);

        PrintList(luminanceValues);

        //Debug.Log("Valores encontrados: " + luminanceValues.Count);
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

        Debug.Log("Valores a sumar: " + luminanceFromObserver.Count);

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

        Debug.Log("Irradiancia energética: " + Ee + " W/m²");
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
