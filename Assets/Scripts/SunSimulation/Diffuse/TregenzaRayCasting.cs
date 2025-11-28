using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TregenzaRayCasting : MonoBehaviour
{
    public float rayDistance { get; set; }
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask layerMask;
    public bool northIsZero { get; set; }

    [Header("Debugging")]
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private float diskRadius = 5f;
    [SerializeField] private bool drawLines;
    public bool drawDisks { get; set; }

    [SerializeField] private FlexibleColorPicker colorPicker0;
    [SerializeField] private FlexibleColorPicker colorPicker1;
    private Color color0;
    private Color color1;
    public bool forwardHue { get; set; }

    private List<Vector3> rayDirections = new List<Vector3>(145);
    private List<bool> raycastHit = new List<bool>(145);
    private GameObject[] circleInstances;

    private void Awake()
    {
        rayDistance = 50f;
        northIsZero = true;
        drawLines = false;
        drawDisks = true;
        color0 = Color.blue;
        color1 = Color.red;
        forwardHue = false;
        TregenzaSky.GenertePatches(northIsZero);
        rayDirections = TregenzaSky.GetDirections();
    }

    public List<bool> UpdateRayCasting()
    {
        CastRays();

        if (drawDisks)
            DrawCircles();

        return raycastHit;
    }

    private void CastRays()
    {
        raycastHit.Clear();
        for (int i = 0; i < rayDirections.Count; i++)
        {
            raycastHit.Add(Physics.Raycast(transform.position, rayDirections[i], out RaycastHit hit, rayDistance, layerMask));
        }
    }

    public void ColorPatches(List<double> luminance)
    {
        if (circleInstances == null) return;

        for (int i = 0; i < luminance.Count; i++)
        {
            if (i >= circleInstances.Length)
                continue;

            GameObject circle = circleInstances[i];

            if (circle == null) continue;

            Color newColor = ColorFromValue(luminance[i]);

            Renderer rend = circle.GetComponent<Renderer>();
            if (rend != null)
                rend.material.SetColor("_BaseColor", newColor);

            TextMeshPro text = circle.GetComponentInChildren<TextMeshPro>();
            if (text != null)
                text.text = (i + 1).ToString();
        }
    }


    #region Debugging
    private void OnDrawGizmos()
    {
        if (raycastHit == null || raycastHit.Count == 0)
            return;

        for (int i = 0; i < raycastHit.Count; i++)
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
        DestroyAllCircles();

        circleInstances = new GameObject[rayDirections.Count];

        for (int i = 0; i < rayDirections.Count; i++)
        {
            Vector3 endPoint = transform.position + rayDirections[i] * rayDistance;
            Quaternion rotation = Quaternion.LookRotation(rayDirections[i]);

            GameObject instance = Instantiate(circlePrefab, endPoint, rotation, transform);
            instance.transform.localScale = new Vector3 (diskRadius * 100f, diskRadius * 100f, diskRadius * 100f);
            circleInstances[i] = instance;
        }
    }

    private void DestroyAllCircles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private Color ColorFromValue(double value)
    {
        if (value == 0)
            return Color.black;
        
        float v = Mathf.Clamp01((float)value);

        float hue0, hue1, sat, bright;
        if(colorPicker0 != null)
            color0 = colorPicker0.color;
        if (colorPicker1 != null)
            color1 = colorPicker1.color;
        Color.RGBToHSV(color0, out hue0, out sat, out bright);
        Color.RGBToHSV(color1, out hue1, out sat, out bright);

        float hue = LerpHueDirected(hue0, hue1, v, forwardHue);
        return Color.HSVToRGB(hue, 1f, 1f);
    }

    private float LerpHueDirected(float h0, float h1, float t, bool forward)
    {
        if (forward)
        {
            if (h1 < h0)
                h1 += 1f;
        }
        else
        {
            if (h1 > h0)
                h1 -= 1f;
        }

        return Mathf.Repeat(Mathf.Lerp(h0, h1, t), 1f);
    }
    #endregion
}
