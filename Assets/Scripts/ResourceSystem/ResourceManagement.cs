using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    [SerializeField] private ResourcesDatabaseSO resourcesDataBase;
    private Dictionary<int, float> resourceDictionary = new();

    private void Start()
    {
        RestartAllResources();
    }


    public void AddResource(int  resourceID, float amount)
    {
        if (!resourceDictionary.ContainsKey(resourceID))
            return;

        resourceDictionary[resourceID] += amount;
    }

    public void RemoveResource(int resourceID, float amount)
    {
        if (!resourceDictionary.ContainsKey(resourceID))
            return;

        resourceDictionary[resourceID] -= amount;

        if (resourceDictionary[resourceID] < 0)
            resourceDictionary[resourceID] = 0f;
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

    public void RestartAllResources()
    {
        resourceDictionary.Clear();

        foreach (var resource in resourcesDataBase.resourcesData)
        {
            if (!resourceDictionary.ContainsKey(resource.ID))
                resourceDictionary.Add(resource.ID, resource.InitialAmount);
        }
    }
}
