using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderChangesTextValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private bool isHour = true;

    private void Start()
    {
        slider.onValueChanged.AddListener((v) => 
        {
            sliderText.text = v.ToString();
            if(isHour)
                DataForSimulation.UpdateHour((int)v);
            else
                DataForSimulation.UpdateMinutes((int)v);
        });
    }
}
