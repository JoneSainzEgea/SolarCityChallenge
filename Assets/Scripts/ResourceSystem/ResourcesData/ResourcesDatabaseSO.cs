/* ResourcesDatabaseSO
 * Jone Sainz Egea
 * 05/12/2025
 * 
 * ScriptableObject para tener la base de datos de todos los recursos que afectan al juego.
 * 
 * v1 -05/12/2025- lista de recursos.
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/ResourceDatabase")]
public class ResourcesDatabaseSO : ScriptableObject
{
    public List<ResourceDataSO> resourcesData;
}
