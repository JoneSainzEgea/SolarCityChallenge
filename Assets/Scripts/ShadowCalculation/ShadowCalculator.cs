// Jone Sainz Egea
// 05/05/2026
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShadowCalculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shadowPercentageText;

    private List<SolarPanelManager> panels = new List<SolarPanelManager>();

    public void ShowShadowPercentage()
    {
        float value = GetTotalSolarPercentage();

        shadowPercentageText.text = $"Shadow percentage is: {value}%, ¡well done!";
    }

    public void StartCheckingShadows()
    {
        RefreshPanels();

        foreach (var panel in panels)
        {
            panel.checkShadows = true;
        }
    }

    public void StopCheckingShadows()
    {
        RefreshPanels();

        foreach (var panel in panels)
        {
            panel.checkShadows = false;
        }
    }

    public float GetTotalSolarPercentage()
    {
        RefreshPanels();
        
        if (panels == null || panels.Count == 0)
            return 0f;

        float total = 0f;

        foreach (var panel in panels)
        {
            total += panel.GetSolarPanelPercentage();
        }

        return (total / panels.Count) * 100;
    }

    private void RefreshPanels()
    {
        panels.Clear();
        panels.AddRange(FindObjectsOfType<SolarPanelManager>());
    }
}
