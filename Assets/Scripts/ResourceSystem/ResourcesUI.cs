using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI energyText;

    public void UpdateTextValues(float moneyAmount, float energyAmount)
    {
        moneyText.text = moneyAmount.ToString();
        energyText.text = energyAmount.ToString();
    }
}
