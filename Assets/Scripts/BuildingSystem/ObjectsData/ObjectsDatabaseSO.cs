/* ObjectsDatabaseSO
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * ScriptableObject para tener la base de datos de todos los objetos que se pueden utilizar en la construcción.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- lista de objetos.
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/ObjectDatabase")]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectDataSO> objectsData;
}
