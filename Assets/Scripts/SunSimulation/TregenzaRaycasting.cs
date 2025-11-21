using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TregenzaRayCasting : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private GameObject circlePrefab;
    //[SerializeField] private float diskRadius = 5f;
    [SerializeField] private bool drawLines = true;
    [SerializeField] private bool drawDisks = true;

    private List<Patch> tregenzaPatches = new List<Patch>(145);
    private List<Vector3> rayDirections = new List<Vector3>(145);
    private List<bool> raycastHit = new List<bool>(145);
    private GameObject[] circleInstances;

    public List<bool> UpdatePatches()
    {
        DestroyAllCircles();

        GetDirections();
        CastRays();

        if (drawDisks)
            DrawCircles();

        return raycastHit;
    }

    private void DestroyAllCircles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void GetDirections()
    {
        rayDirections.Clear();
        raycastHit.Clear();

        tregenzaPatches = TregenzaSky.GenertePatches();

        FromGeometryToDirections();
    }

    private void FromGeometryToDirections()
    {
        for (int i = 0; i < tregenzaPatches.Count; ++i)
        {
            float elevationDeg = tregenzaPatches[i].Elevation;
            float azimtuhDeg = tregenzaPatches[i].Azimuth;

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

            rayDirections.Add(direction);
            raycastHit.Add(false);
        }
    }

    private void CastRays()
    {
        for (int i = 0; i < rayDirections.Count; i++)
        {
            raycastHit[i] = Physics.Raycast(transform.position, rayDirections[i], out RaycastHit hit, rayDistance, layerMask);
        }
    }

    public void ColorPatches(Dictionary<int, double> luminance)
    {
        if (circleInstances == null) return;

        foreach (var pair in luminance)
        {
            int index = pair.Key;
            double value = pair.Value;

            if (index < 0 || index >= circleInstances.Length)
                continue;

            GameObject circle = circleInstances[index];

            if (circle == null) continue;

            Color newColor = ColorFromValue(value);

            Renderer rend = circle.GetComponent<Renderer>();
            if (rend != null)
                rend.material.SetColor("_BaseColor", newColor);

            TextMeshPro text = circle.GetComponentInChildren<TextMeshPro>();
            if (text != null)
                text.text = (index + 1).ToString();
        }
    }


    #region Debugging
    private void OnDrawGizmos()
    {
        if (rayDirections == null || rayDirections.Count == 0)
            return;

        for (int i = 0; i < rayDirections.Count; i++)
        {
            Color c = raycastHit[i] ? Color.red : Color.green;
            Gizmos.color = c;

            Vector3 endPoint = transform.position + rayDirections[i] * rayDistance;

            if (drawLines)
                Gizmos.DrawLine(transform.position, endPoint);
        }
    }

    private void DrawCircles()
    {
        circleInstances = new GameObject[rayDirections.Count];

        for (int i = 0; i < rayDirections.Count; i++)
        {
            Vector3 endPoint = transform.position + rayDirections[i] * rayDistance;
            Quaternion rotation = Quaternion.LookRotation(rayDirections[i]);

            GameObject instance = Instantiate(circlePrefab, endPoint, rotation, transform);
            circleInstances[i] = instance;
        }
    }

    private Color ColorFromValue(double value)
    {
        if (value == 0)
            return Color.black;
        
        float v = Mathf.Clamp01((float)value);

        float hueStart = 0.66f; // azul
        float hueEnd = 0.0f;    // rojo

        float hue = Mathf.Lerp(hueStart, hueEnd, v);
        return Color.HSVToRGB(hue, 1f, 1f);
    }
    #endregion
}
