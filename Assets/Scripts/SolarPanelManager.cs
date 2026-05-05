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

    private bool checkShadows = false;

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
        if (checkShadows)
            CheckShadows();
    }

    public void StartCheckingShadows()
    {
        checkShadows = true;
    }

    public void StopCheckingShadows()
    {
        checkShadows = false;
    }

    public float GetSolarPanelPercentage()
    {
        CheckShadows();

        if (modules == null || modules.Count == 0)
            return 0f;

        int shadowedCount = 0;

        foreach (var module in modules)
        {
            if (module.isInShadow)
                shadowedCount++;
        }

        float percentage = ((float)shadowedCount / modules.Count) * 100;

        return percentage;
    }

    private void CheckShadows()
    {
        if (sun == null) return;

        Vector3 sunDirection = -sun.transform.forward;

        foreach (var module in modules)
        {
            module.CheckShadow(sunDirection, rayDistance, shadowMask);
        }
    }
}
