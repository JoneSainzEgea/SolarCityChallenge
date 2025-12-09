using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    [SerializeField] private ResourcesDatabaseSO resourcesDataBase;
    [SerializeField] ResourcesUI resourcesUI;
    private Dictionary<int, float> resourceDictionary = new();

    private void Start()
    {
        RestartAllResources();
    }


    public void AddResource(ResourceType resourceType, float amount)
    {
        int resourceID = GetResourceID(resourceType);

        if (!resourceDictionary.ContainsKey(resourceID))
            return;

        resourceDictionary[resourceID] += amount;
        UpdateResourcesUI();
    }

    public void RemoveResource(ResourceType resourceType, float amount)
    {
        int resourceID = GetResourceID(resourceType);

        if (!resourceDictionary.ContainsKey(resourceID))
            return;

        resourceDictionary[resourceID] -= amount;

        if (resourceDictionary[resourceID] < 0)
            resourceDictionary[resourceID] = 0f;

        UpdateResourcesUI();
    }

    public int GetResourceID(ResourceType resourceType)
    {
        foreach (var resource in resourcesDataBase.resourcesData)
        {
            if(resource.ResourceType == resourceType)
            {
                return resource.ID;
            }
        }
        return -1;
    }

    public float GetResourceAmount(int resourceID)
    {
        if (!resourceDictionary.ContainsKey(resourceID))
            return -1;
        return resourceDictionary[resourceID];
    }

    public void RestartAllResources()
    {
        resourceDictionary.Clear();

        foreach (var resource in resourcesDataBase.resourcesData)
        {
            if (!resourceDictionary.ContainsKey(resource.ID))
                resourceDictionary.Add(resource.ID, resource.InitialAmount);
        }

        UpdateResourcesUI();
    }

    private void UpdateResourcesUI()
    {
        // TODO: hacer esto escalable
        
        float moneyValue = resourceDictionary[GetResourceID(ResourceType.Money)];
        float energyValue = resourceDictionary[GetResourceID(ResourceType.Energy)];

        resourcesUI.UpdateTextValues(moneyValue, energyValue);
    }
}
