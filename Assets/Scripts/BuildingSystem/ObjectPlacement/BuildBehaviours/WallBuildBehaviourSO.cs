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

        // Has floor and doesn't have wall
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        GridData wallData = gridData.GetGridData(GridDataType.WallData);
        return !floorData.CanPlaceObjectAt(pos, size) && wallData.CanPlaceObjectAt(pos, size);
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        // TODO: change calc for amount of wall
        this.resourceManagement = resourceManagement;
        return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize);
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        int index = placer.PlaceObject(prefab, grid.CellToWorld(pos), energyProduction, resourceManagement);

        GridData wallData = gridData.GetGridData(GridDataType.WallData);
        wallData.AddObjectAt(pos, size, ID, index);
    }
}