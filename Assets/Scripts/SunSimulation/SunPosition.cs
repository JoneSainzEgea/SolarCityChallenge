
// Jone Sainz Egea
// v1 - 05/11/2025: clase con la información de fecha y hora, latitud y longitud. Llama al método CalculateSunPosition de la clase SunPosition.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity.Mathematics;
using UnityEngine;

public class SunPosition : MonoBehaviour
{
    private DateTime date;
    private double latitude;
    private double longitude;

    private SunPositionMath sunPositionMath;

    [SerializeField] private GameObject sunDirectionalLight;

    private void Start()
    {
        sunPositionMath = GetComponent<SunPositionMath>();

        latitude = GameManager.Instance.latitud;
        longitude = GameManager.Instance.longitud;
        date = new DateTime(GameManager.Instance.year, GameManager.Instance.month, GameManager.Instance.day, 13, 0, 0);
        UpdateSunPosition();
    }

    public void UpdateSunPosition()
    {
        UpdateDate();

        // Llamada al cálculo de la altura y el azimut solar
        sunPositionMath.CalculateSunPosition(date, latitude, longitude, sunDirectionalLight);
    }

    private void UpdateDate()
    {
        date = new DateTime(DataForSimulation.Year, DataForSimulation.Month, DataForSimulation.Day, (int)DataForSimulation.Hour, (int)DataForSimulation.Minutes, 0);
    }
}
