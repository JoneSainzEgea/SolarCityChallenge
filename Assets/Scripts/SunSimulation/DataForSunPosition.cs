
// Jone Sainz Egea
// v1 - 05/11/2025: clase con la información de fecha y hora, latitud y longitud. Llama al método CalculateSunPosition de la clase SunPosition.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataForSunPosition : MonoBehaviour
{
    private DateTime date;
    private double latitude;
    private double longitude;

    private SunPosition sunPositionScript;

    [SerializeField] private GameObject sunDirectionalLight;


    private void Start()
    {
        sunPositionScript = GetComponent<SunPosition>();
        
        // Fecha y hora actual
        date = DateTime.Now;
        
        // Coordenadas de Burgos
        latitude = 42.35079519629251;
        longitude = -3.6877558759138362;

        // Llamada al cálculo de la altura y el azimut solar
        sunPositionScript.CalculateSunPosition(date, latitude, longitude, sunDirectionalLight);
    }
}
