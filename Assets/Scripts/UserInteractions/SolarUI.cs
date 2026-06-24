// Jone Sainz Egea
// 06/05/2026
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolarUI : MonoBehaviour
{
    [SerializeField] private Slider rotationSlider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private ShadowCalculator shadowCalculator;
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        rotationSlider.minValue = 0f;
        rotationSlider.maxValue = 90f;
        rotationSlider.value = 30f;
        rotationSlider.wholeNumbers = true;

        rotationSlider.onValueChanged.AddListener(OnSliderChanged);

        OnSliderChanged(rotationSlider.value);
    }

    private void OnSliderChanged(float value)
    {
        valueText.text = value.ToString();

        // Enviar valor al sistema
        shadowCalculator.SetPanelsRotation(value);
    }

    public void CalculateOrientationResult()
    {
        float efficiency = shadowCalculator.CalculateOrientationEfficiency();

        Debug.Log($"Eficiencia: {efficiency * 100f}%");

        resultText.text = GetFeedback(efficiency);
    }

    private string GetFeedback(float efficiency)
    {
        float percent = efficiency * 100f;

        if (efficiency > 0.9f) return $"Óptimo ({percent:0}%)";
        if (efficiency > 0.8f) return $"Muy bueno ({percent:0}%)";
        if (efficiency > 0.7f) return $"Aceptable ({percent:0}%)";
        return $"Mejorable ({percent:0}%)";
    }
}
