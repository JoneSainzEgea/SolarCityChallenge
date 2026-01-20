/* ObjectPlacer
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Clase que se encarga de la colocación, conexión y eliminación de objetos en el grid.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- creación y eliminación de objetos.
 * v2 -10/12/2025- llama a OnPlacement del componente.
 * v3 -11/12/2025- conecta componentes.
 * 
 * TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
 */

using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedGameObjects = new();
    public bool isConnecting = false;
    private BuildingComponent component1, component2;

    public int PlaceObject(GameObject prefab, Vector3 position, float energyProduction, ResourceManagement resManager)
    {
        // TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        BuildingComponent component = newObject.GetComponentInChildren<BuildingComponent>();
        if (component != null)
            component.OnPlacement(energyProduction, resManager);
        else
            Debug.LogWarning("El prefab del componente no tiene el script de componente");

        placedGameObjects.Add(newObject);
        
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }

    public void StartConnectingAt(int gameObjectIndex)
    {
        Debug.Log("Starts connecting");
        
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        component1 = placedGameObjects[gameObjectIndex].GetComponentInChildren<BuildingComponent>();
        if (component1 == null)
            return;

        isConnecting = true;
    }

    public void StopConnectingAt(int gameObjectIndex)
    {
        Debug.Log("Stops connecting");

        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        component2 = placedGameObjects[gameObjectIndex].GetComponentInChildren<BuildingComponent>();
        if (component2 == null)
            return;

        component1.OnConnection(component2);
        component2.OnConnection(component1);

        isConnecting = false;
    }
}
