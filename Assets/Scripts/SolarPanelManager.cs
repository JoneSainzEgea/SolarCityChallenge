// Jone Sainz Egea
// 21/04/2026

using UnityEngine;
using System.Collections.Generic;

public class SolarPanelManager : MonoBehaviour
{
    [Header("Referencias")]
    public Light sun;

    [Header("Configuración")]
    public float rayDistance = 100f;
    public LayerMask shadowMask;

    private List<SolarModule> modules = new List<SolarModule>();

    void Start()
    {
        if (sun == null)
        {
            sun = RenderSettings.sun;
        }

        modules.AddRange(GetComponentsInChildren<SolarModule>());
    }

    void Update()
    {
        if (sun == null) return;

        Vector3 sunDirection = -sun.transform.forward;

        foreach (var module in modules)
        {
            module.CheckShadow(sunDirection, rayDistance, shadowMask);
        }
    }
}
