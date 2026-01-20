/* ResourceManagement
 * Jone Sainz Egea
 * 05/12/2025
 * 
 * Script que gestiona toda la información relativa a los recursos cuantitativos.
 * Contiene un diccionario en el que almacena el ID del recurso y su cantidad.
 * Se encarga de la comunicación con ResourcesUI para la visualización de cantidades.
 * Contiene métodos públicos para añadir recursos, quitar recursos, obtener cantidad del recurso y reiniciar recursos.
 * 
 * v1 -05/12/2025- añadir y quitar recursos, obtener cantidad, mostrar en UI, reiniciar cantidades.
 */

using System.Collections.Generic;
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

        // TODO: ¿podría la generación ed energía ser negativa?

        if (resourceDictionary[resourceID] < 0)
            resourceDictionary[resourceID] = 0f;

        UpdateResourcesUI();
    }

    private int GetResourceID(ResourceType resourceType)
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

    public float GetResourceAmount(ResourceType resourceType)
    {
        int resourceID = GetResourceID(resourceType);

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
