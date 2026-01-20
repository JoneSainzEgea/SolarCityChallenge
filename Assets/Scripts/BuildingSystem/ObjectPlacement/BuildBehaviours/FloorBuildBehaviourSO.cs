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
    private Vector2Int size;
    private Vector3Int pos;
    private GameObject prefab;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    public override void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size)
    {
        this.prefab = prefab;
        this.size = size;
        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(Vector3Int pos, Grid grid, GridDataManager gridData)
    {
        this.pos = pos;
        this.grid = grid;
        this.gridData = gridData;

        // Doesn't have floor
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        return floorData.CanPlaceObjectAt(pos, size);
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        // TODO: change calc for amount of floor
        this.resourceManagement = resourceManagement;
        return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize);
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        int index = placer.PlaceObject(prefab, grid.CellToWorld(pos), energyProduction, resourceManagement);

        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        floorData.AddObjectAt(pos, size, ID, index);
    }
}
