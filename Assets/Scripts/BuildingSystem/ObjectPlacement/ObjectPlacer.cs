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
 * v4 -21/01/2026- crea y elimina grupos de objetos.
 * v5 -30/01/2026- GetGameObjectAt añadido para la previsualización del removing state.
 * 
 * TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
 */

using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedGameObjects = new();
    private List<List<GameObject>> groupedGameObjects = new();
    public bool isConnecting = false;
    private SolarComponent component1, component2;
    private int groupIndex = -1;

    #region Object Placement and Removal
    public int PlaceObject(GameObject prefab, Vector3 position, float energyProduction, ResourceManagement resManager)
    {
        // TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        SolarComponent component = newObject.GetComponentInChildren<SolarComponent>();
        if (component != null)
            component.OnPlacement(energyProduction, resManager);
        //else
        //    Debug.LogWarning("El prefab del componente no tiene el script de componente");

        placedGameObjects.Add(newObject);
        
        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }

    public GameObject GetGameObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
        {
            Debug.LogWarning("No object to remove found");
            return null;
        }
        return placedGameObjects[gameObjectIndex];
    }
    #endregion

    #region WallPlacement
    public void PlaceWallGroupObject(GameObject prefab, Vector3 position, Grid grid, GridDataManager gridData)
    {
        // TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        groupedGameObjects[groupIndex].Add(newObject);
        if (newObject.TryGetComponent<WallAutoTile>(out WallAutoTile autoTile))
        {
            autoTile.Initialize(grid, gridData);
        }
    }
    #endregion

    #region Grouped Objects
    public int CreateNewGroup()
    {
        groupIndex++;
        groupedGameObjects.Add(new List<GameObject>());
        return groupIndex;
    }

    public void PlaceGroupObject(GameObject prefab, Vector3 position)
    {
        // TODO: que sean hijos de un objeto concreto para limpieza de jerarquía
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        groupedGameObjects[groupIndex].Add(newObject);
    }

    public void RemoveGroupObjectAt(int groupObjectIndex)
    {
        if (groupedGameObjects.Count <= groupObjectIndex || groupedGameObjects[groupObjectIndex] == null)
            return;
        foreach(GameObject go in groupedGameObjects[groupObjectIndex])
        {
            Destroy(go);
        }
        groupedGameObjects[groupObjectIndex] = null;
    }
    #endregion

    #region Connections

    public void StartConnectingAt(int gameObjectIndex)
    {
        Debug.Log("Starts connecting");
        
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        component1 = placedGameObjects[gameObjectIndex].GetComponentInChildren<SolarComponent>();
        if (component1 == null)
            return;

        isConnecting = true;
    }

    public void StopConnectingAt(int gameObjectIndex)
    {
        Debug.Log("Stops connecting");
        isConnecting = false;

        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        component2 = placedGameObjects[gameObjectIndex].GetComponentInChildren<SolarComponent>();
        if (component2 == null)
            return;

        component1.OnConnection(component2);
        component2.OnConnection(component1);
    }
    #endregion
}
