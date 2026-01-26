// TODO
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/WallFurniture")]
public class WallFurnitureBuildBehaviourSO : BuildBehaviourSO
{
    private Vector2Int size;
    private Vector3Int pos;
    private GameObject prefab;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    private PreviewSystem preview;
    private int prize;
    public override void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size, Grid grid, GridDataManager gridData)
    {
        this.preview = preview;
        this.prefab = prefab;
        this.size = size;
        this.grid = grid;
        this.gridData = gridData;

        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(Vector3Int pos)
    {
        this.pos = pos;

        // Has wall and doesn't have wall furniture
        GridData wallData = gridData.GetGridData(GridDataType.WallData);
        GridData wallFurnitureData = gridData.GetGridData(GridDataType.WallFurnitureData);
        return !wallData.CanPlaceObjectAt(pos, size) && wallFurnitureData.CanPlaceObjectAt(pos, size);
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        this.resourceManagement = resourceManagement;
        this.prize = prize;

        return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize);
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        int index = placer.PlaceObject(prefab, grid.CellToWorld(pos), energyProduction, resourceManagement);

        GridData wallFurnitureData = gridData.GetGridData(GridDataType.WallFurnitureData);
        wallFurnitureData.AddObjectAt(pos, size, ID, index);
    }

    public override void RemoveResources()
    {
        resourceManagement.RemoveResource(ResourceType.Money, prize);
    }

    public override void UpdatePreview(Vector3Int gridPosition)
    {
        preview.UpdatePosition(grid.CellToWorld(gridPosition), CanPlace(gridPosition));
    }
}
