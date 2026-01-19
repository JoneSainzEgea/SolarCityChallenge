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
    public abstract int Cost { get; }

    public abstract void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size);

    public abstract bool CanPlace(Vector3Int gridPosition, Grid grid, GridData floorData, GridData furnitureData);

    public abstract void Place(
        Vector3Int gridPosition,
        Grid grid,
        ObjectsDatabaseSO database,
        int objectIndex,
        ObjectPlacer placer,
        GridData floorData,
        GridData furnitureData,
        ResourceManagement resources);
}
