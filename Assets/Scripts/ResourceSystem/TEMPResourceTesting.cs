using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TEMPResourceTesting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private ResourceManagement resourceManagement;

    private float moneyAmount, energyAmount;


    private void UpdateTextValues()
    {
        //energyText.text = ToString(energyAmount);
    }

    public void ChangeMoneyValue(float amount, bool isPositive)
    {
        if(isPositive)
            resourceManagement.AddResource(resourceManagement.GetResourceID(ResourceType.Money), amount);
        else
            resourceManagement.RemoveResource(resourceManagement.GetResourceID(ResourceType.Money), amount);

        UpdateTextValues();
    }
    public void ChangeEnergyValue(float amount, bool isPositive)
    {
        if(isPositive)
            resourceManagement.AddResource(resourceManagement.GetResourceID(ResourceType.Energy), amount);
        else
            resourceManagement.RemoveResource(resourceManagement.GetResourceID(ResourceType.Energy), amount);
        
        UpdateTextValues();
    }
}
