using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static List<Patch> GenertePatches (bool zeroNorth = true)
    {
        List<Patch> patches = new List<Patch>(145);

        float[] heights = { 6f, 18f, 30f, 42f, 54f, 66f, 78f, 90f }; // Altura de cada anillo (alfa)

        int[] ringCounts = { 30, 30, 24, 24, 18, 12, 6, 1 }; // Número de patches por anillo

        for (int ring = 0; ring < ringCounts.Length; ring++)
        {
            int count = ringCounts[ring];
            float elevation = heights[ring];
            float deltaAz = 360 / count;

            for (int i = 0; i < count; i++)
            {
                float azimuth = i * deltaAz;

                if (!zeroNorth)
                    azimuth = (azimuth + 180f) % 360f;
                
                patches.Add(new Patch(elevation, azimuth));
            }
        }

        return patches;
    }
}
