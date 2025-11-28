using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SFB;
using UnityEngine;
using TMPro;
using System.Windows.Forms;

public class LuminanceCSVReading : MonoBehaviour
{
    [SerializeField] private TextAsset luminanceCSV;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    private string luminanceData;

    private void Awake()
    {
        luminanceData = luminanceCSV.text;
    }

    public void OpenCSV()
    {
        var extensions = new[] {
            new ExtensionFilter("CSV Files", "csv"),
            new ExtensionFilter("All Files", "*" ),
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Seleccionar CSV", "", extensions, false);

        if (paths.Length > 0)
        {
            string path = paths[0];
            Debug.Log("Archivo seleccionado: " + path);

            string loadedCSV = File.ReadAllText(path);
            luminanceData = loadedCSV;
        }
    }

    public List<double> GetValues(String date)
    {
        List<double> luminanceValues = new List<double>(145);

        // Lectura del CSV
        string[] lines = luminanceData.Split('\n');

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

        ThrowError("No data was found for the specified date.");
        return null;
    }

    private void ThrowError(string error)
    {
        errorText.text = error;
        errorPanel.SetActive(true);
    }
}


