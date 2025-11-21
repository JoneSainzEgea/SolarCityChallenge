using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct Patch
{
    public float Elevation;
    public float Azimuth;

    public Patch(float elevation, float azimuth)
    {
        this.Elevation = elevation;
        this.Azimuth = azimuth;
    }
}

public static class TregenzaSky
{
    public static List<Patch> SkyPatches = new List<Patch>();
    public static List<Vector3> RayDirections = new List<Vector3>();
    
    public static void GenertePatches (bool zeroNorth = true)
    {
        float[] heights = { 6f, 18f, 30f, 42f, 54f, 66f, 78f, 90f }; // Altura de cada anillo (alfa)

        int[] ringCounts = { 30, 30, 24, 24, 18, 12, 6, 1 }; // Número de patches por anillo

        float currentStartAz = 0f;
        bool clockwise = true;

        for (int ring = 0; ring < ringCounts.Length; ring++)
        {
            int count = ringCounts[ring];
            float elevation = heights[ring];
            float deltaAz = 360f / count;

            if (ring == 5) // Excepción de azimuth en el sexto anillo
            {
                currentStartAz = (0f - deltaAz) % 360f;
                if (currentStartAz < 0f) currentStartAz += 360f;
            }

            for (int i = 0; i < count; i++)
            {
                float azimuth;

                if (clockwise)
                    azimuth = currentStartAz + i * deltaAz;
                else
                    azimuth = currentStartAz - i * deltaAz;

                azimuth = (azimuth % 360f + 360f) % 360f;

                if (!zeroNorth)
                    azimuth = (azimuth + 180f) % 360f;

                SkyPatches.Add(new Patch(elevation, azimuth));
            }

            float lastAz;

            if (clockwise)
                lastAz = currentStartAz + (count - 1) * deltaAz;
            else
                lastAz = currentStartAz - (count - 1) * deltaAz;

            lastAz = (lastAz % 360f + 360f) % 360f;

            currentStartAz = lastAz;
            clockwise = !clockwise;
        }
    }

    public static List<Vector3> GetDirections()
    {
        for (int i = 0; i < SkyPatches.Count; i++)
        {
            float elevationDeg = SkyPatches[i].Elevation;
            float azimtuhDeg = SkyPatches[i].Azimuth;

            float elev = elevationDeg * Mathf.Deg2Rad;
            float az = azimtuhDeg * Mathf.Deg2Rad;

            float sinElev = Mathf.Sin(elev);
            float cosElev = Mathf.Cos(elev);
            float sinAz = Mathf.Sin(az);
            float cosAz = Mathf.Cos(az);

            // norte = +x; arriba = +y; este = -z
            float x = cosElev * Mathf.Cos(az);
            float z = cosElev * -Mathf.Sin(az);
            float y = sinElev;

            Vector3 direction = new Vector3(x, y, z).normalized;

            RayDirections.Add(direction);
        }

        return RayDirections;
    }

    public static List<float> GetIncidentAngle(Vector3 normal)
    {
        // Calcular el ángulo incidente de cada rayo de tregenza

        List<float> incidentAngles = new List<float>();

        for (int i = 0; i < RayDirections.Count; i++)
        {
            incidentAngles.Add(Vector3.Angle(-RayDirections[i], normal));
        }

        return incidentAngles;
    }
}
