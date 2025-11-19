using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LuminanceCSVReading : MonoBehaviour
{
    [SerializeField] private TextAsset luminanceCSV;

    public List<double> GetValues(String date)
    {
        List<double> luminanceValues = new List<double>(145);

        // Lectura del CSV
        string[] lines = luminanceCSV.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            // Verificar longitud: 3 columnas + 145 valores
            if (parts.Length < 148)
            {
                Debug.LogWarning($"Fila ignorada, columnas insuficientes en línea {i + 1}");
                continue;
            }

            // Comparar fechas
            if (date == parts[2])
            {
                for (int c = 3; c < 148; c++)
                {
                    if (double.TryParse(parts[c], NumberStyles.Any,CultureInfo.InvariantCulture, out double d))
                    {
                        luminanceValues.Add(d);
                    }
                    else
                    {
                        luminanceValues.Add(double.NaN);
                    }
                }

                return luminanceValues;
            }
        }

        Debug.LogWarning("No se ha encontrado la fecha indicada");
        return null;
    }
}
