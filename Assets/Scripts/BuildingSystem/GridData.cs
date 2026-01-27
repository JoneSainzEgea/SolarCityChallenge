/* GridData
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Clase que se encarga del almacenamiento de la información de los objetos que hay sobre el grid.
 * Almacena la información del objeto colocado según su posición, tamaño, ID y el índice del orden de almacenamiento de la lista.
 * Calcula las posiciones que ocupa el objeto en el grid según su tamaño.
 * Devuelve el valor de si una casilla está ocupada o no.
 * Borra objetos eliminados de la lista.
 * Devuelve el valor del índice de la lista de objetos colocados.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- almacenamiento de objetos en el grid según su posición, ID, tamaño e índice.
 * v2 -21/01/2026- añadido de GridDataGroups para elementos conjuntos (suelo)
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
    Dictionary<Vector3Int, int> dataGroups = new();
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach(var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    public void AddObjectToGroup(Vector3Int gridPosition, int groupIndex)
    {
        dataGroups.Add(gridPosition, groupIndex);
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        // TODO: cambiar esto cuando añada rotación de objetos del grid
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool IsOccupied(Vector3Int position)
    {
        if (placedObjects.ContainsKey(position))
            return true;
        return false;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        
        foreach(var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
   
        return true;
    }

    public List<Vector3Int> GetGroupPositions(Vector3Int position)
    {
        List<Vector3Int> groupPositions = new List<Vector3Int>();

        if (dataGroups.ContainsKey(position))
        {
            int groupIndex = dataGroups[position];
            foreach (KeyValuePair<Vector3Int, int> pair in dataGroups)
            {
                if (pair.Value == groupIndex)
                    groupPositions.Add(pair.Key);
            }
        }

        return groupPositions;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }
    public int GetRepresentationID(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].ID;
    }

    public int GetGroupRepresentationIndex(Vector3Int gridPosition)
    {
        if (dataGroups.ContainsKey(gridPosition) == false)
            return -1;
        return dataGroups[gridPosition];
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        List<Vector3Int> keysToRemove = new List<Vector3Int>();

        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            if (dataGroups.ContainsKey(pos))
            {
                int groupIndex = dataGroups[pos];
                foreach (KeyValuePair<Vector3Int, int> pair in dataGroups)
                {
                    if(pair.Value == groupIndex)
                        keysToRemove.Add(pair.Key);
                }
            }
            else
                keysToRemove.Add(pos);
        }

        foreach (var key in keysToRemove)
        {
            dataGroups.Remove(key);
            placedObjects.Remove(key);
        }
    }
}

/* PlacementData
 *
 * Clase que se utiliza para almacenar la información de los objetos del grid.
 * Contiene una lista con las posiciones que ocupa el objeto, el ID del objeto almacenado y el índice de la lista de objetos.
 *
 */

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID {  get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
