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
    private Dictionary<int, double> luminanceFromObserver = new Dictionary<int, double>();

    [SerializeField] private LuminanceCSVReading luminanceCSV;
    [SerializeField] private TregenzaRayCasting raycasting;

    [Header("Date")]
    private int year = 2025;
    [Range(5,7)][SerializeField] private int month;
    [Range(1, 31)][SerializeField] private int day;
    [Range(6, 23)][SerializeField] private int hour;
    [Range(0, 55)][SerializeField] private int minutes;

    [Header("Debugging")]
    [SerializeField] private Color color0;
    [SerializeField] private Color color1;

    private void Start()
    {
        UpdateDiffuseRadiance();
    }

    public void UpdateDiffuseRadiance()
    {
        raycastHits.Clear();
        luminanceValues.Clear();
        luminanceFromObserver.Clear();

        GetTregenzaRayCasting();
        GetLuminanceValues();
        SaveLuminanceFromObserver();
        CalculateDiffuseRadiance();
    }

    private void GetTregenzaRayCasting()
    {
        raycastHits = GetComponent<TregenzaRayCasting>().UpdatePatches();
    }

    private void GetLuminanceValues()
    {
        DateTime date = new DateTime(year, month, day, hour, minutes, 0);

        string formattedDate = date.ToString("yyyy-MM-dd HH:mm");

        luminanceValues = luminanceCSV.GetValues(formattedDate);

        PrintList(luminanceValues);

        Debug.Log("Valores encontrados: " + luminanceValues.Count);
    }

    private void SaveLuminanceFromObserver()
    {
        for(int i = 0; i < 145; i++)
        {
            if (raycastHits[i] == false)
            {
                luminanceFromObserver.Add(i, luminanceValues[i]);
            }
            else
            {
                luminanceFromObserver.Add(i, 0d);
            }
        }

        Debug.Log("Valores a sumar: " + luminanceFromObserver.Count);

        PrintColors(luminanceFromObserver);
    }

    private void CalculateDiffuseRadiance()
    {
        // Preguntar
    }

    #region Debugging
    private void PrintList(List<double> list)
    {
        Debug.Log(string.Join(", ", list));
    }


    private void PrintColors(Dictionary<int, double> luminance)
    {
        raycasting.ColorPatches(luminance);
    }
    #endregion
}
