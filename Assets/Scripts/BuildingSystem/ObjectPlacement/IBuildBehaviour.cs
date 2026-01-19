/* IBuildBehaviour
 * Jone Sainz Egea
 * 19/01/2026
 *
 * Interfaz que define el comportamiento para los diferentes tipos de construcción para PlacementState.
 * 
 * v1 -19/01/2026- CanPlace, Place, UpdatePreview, Cost.
 */
using UnityEngine;

public interface IBuildBehaviour
{
    bool CanPlace(Vector3Int position);
    void Place(Vector3Int position);
    void UpdatePreview(Vector3Int position);
    int Cost { get; }
}
