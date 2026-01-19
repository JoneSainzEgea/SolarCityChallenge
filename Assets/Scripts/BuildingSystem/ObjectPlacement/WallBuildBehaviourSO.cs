/* WallBuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -19/01/2026- 
 */
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Wall")]
public class WallBuildBehaviourSO : BuildBehaviourSO
{
    [SerializeField] private int cost;

    public override int Cost => cost;

    public override void StartPreview(
        PreviewSystem preview,
        GameObject prefab,
        Vector2Int size)
    {
        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(
        Vector3Int pos,
        Grid grid,
        GridData floorData,
        GridData furnitureData)
    {
        // regla: solo sobre suelo
        return !floorData.CanPlaceObjectAt(pos, Vector2Int.one)
               && furnitureData.CanPlaceObjectAt(pos, Vector2Int.one);
    }

    public override void Place(
        Vector3Int pos,
        Grid grid,
        ObjectsDatabaseSO database,
        int objectIndex,
        ObjectPlacer placer,
        GridData floorData,
        GridData furnitureData,
        ResourceManagement resources)
    {
        int index = placer.PlaceObject(
            database.objectsData[objectIndex].Prefab,
            grid.CellToWorld(pos),
            database.objectsData[objectIndex].EnergyProduction,
            resources);

        furnitureData.AddObjectAt(
            pos,
            Vector2Int.one,
            database.objectsData[objectIndex].ID,
            index);
    }
}