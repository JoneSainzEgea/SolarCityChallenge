/* FloorBuildBehaviour
 * Jone Sainz Egea
 * 19/01/2026
 *
 * 
 * v1 -19/01/2026- .
 */

using UnityEngine;

public class WallBuildBehaviour : IBuildBehaviour
{
    public bool CanPlace(Vector3Int position)
    {
        // comprobar que haya suelo debajo
        // comprobar adyacencia
        return true;
    }

    public void Place(Vector3Int position)
    {
        // lógica específica de pared
    }

    public void UpdatePreview(Vector3Int position) { }

    public int Cost => 10;
}
