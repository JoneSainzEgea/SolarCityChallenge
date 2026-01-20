/* DefaultBuildBehaviourSO
 * Jone Sainz Egea
 * 20/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -20/01/2026- 
 */
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Default")]
public class DefaultBuildBehaviourSO : BuildBehaviourSO
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

        // Has floor and doesn't have furniture
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        GridData furnitureData = gridData.GetGridData(GridDataType.FurnitureData);
        return !floorData.CanPlaceObjectAt(pos, size) && furnitureData.CanPlaceObjectAt(pos, size);
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        this.resourceManagement = resourceManagement;
        return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize);
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        int index = placer.PlaceObject(prefab,grid.CellToWorld(pos), energyProduction, resourceManagement);

        GridData furnitureData = gridData.GetGridData(GridDataType.FurnitureData);
        furnitureData.AddObjectAt(pos,size, ID, index);
    }
}
