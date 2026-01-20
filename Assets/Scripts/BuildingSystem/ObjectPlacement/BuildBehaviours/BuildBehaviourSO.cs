/* BuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -19/01/2026- 
 */

using UnityEngine;

public abstract class BuildBehaviourSO : ScriptableObject
{
    public abstract void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size);

    public abstract bool CanPlace(Vector3Int gridPosition, Grid grid, GridDataManager gridData);

    public abstract bool HasMoney(ResourceManagement resourceManagement, int prize);

    public abstract void Place(ObjectPlacer placer, int ID, int energyProduction);

    public abstract void UpdatePreview(Vector3Int gridPosition);
}
