/* ObjectDataSO
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * ScriptableObject para tener la información de cada objeto que se puede utilizar en la construcción.
 * Los objetos tienen: ID único, nombre, tamaño y prefab del objeto.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- ID, nombre, tamaño 2D y prefab del objeto.
 * v2 -09/12/2025- Precio y producción de energía
 * 
 * TODO: ampliar a dimensión vertical.
 */

using System;
using UnityEngine;


[CreateAssetMenu(menuName = "BuildingSystem/Object")]
[Serializable]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public int Prize { get; private set; }

    // TODO: posibilidad de que se venda a otro precio

    [field: SerializeField]
    public int EnergyProduction { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
