using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CircleColorController : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] private float luminance = 0f;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        float hueStart = 0.66f; // Hue azul

        float hueEnd = 0.0f; // Hue rojo

        float hue = Mathf.Lerp(hueStart, hueEnd, luminance);

        float saturation = 1f;
        float brightness = 1f;

        Color finalColor = Color.HSVToRGB(hue, saturation, brightness);

        if (luminance == 0f)
            finalColor = Color.black;

        rend.material.SetColor("_BaseColor", finalColor);
    }
}
