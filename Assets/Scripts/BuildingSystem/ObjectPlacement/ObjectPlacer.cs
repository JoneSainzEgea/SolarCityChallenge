/* ObjectPlacer
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Clase que se encarga de la colocación y eliminación de objetos en el grid.
 * Para colocarlos recibe la información del prefab y la posición, crea el objeto en la posición y devuelve la información del índice en la lista de objetos en el que se coloca.
 * Para eliminarlos recibe el índice de la lista en el que estaba colocado, destruye el objeto de esa posición y almacena null en la lista de objetos.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- creación y eliminación de objetos.
 */

using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
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
}
