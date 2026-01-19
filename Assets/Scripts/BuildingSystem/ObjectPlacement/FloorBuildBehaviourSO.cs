/* FloorBuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -19/01/2026- 
 */
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Floor")]
public class FloorBuildBehaviourSO : BuildBehaviourSO
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
        return floorData.CanPlaceObjectAt(pos, Vector2Int.one);
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

        floorData.AddObjectAt(
            pos,
            database.objectsData[objectIndex].Size,
            database.objectsData[objectIndex].ID,
            index);
    }
}
