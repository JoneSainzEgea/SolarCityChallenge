/* FloorBuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -19/01/2026- 
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Floor")]
public class FloorBuildBehaviourSO : BuildBehaviourSO
{
    private Vector2Int size;
    private Vector3Int pos1;
    private Vector3Int pos2;
    List<Vector3Int> gridRectangle;
    private GameObject prefab;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    private bool isFirstClick = true;
    private PreviewSystem preview;
    public override void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size)
    {
        this.prefab = prefab;
        this.size = size;
        this.preview = preview;

        isFirstClick = true;
        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(Vector3Int pos, Grid grid, GridDataManager gridData)
    {
        this.grid = grid;
        this.gridData = gridData;
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        if (isFirstClick)
        {
            pos1 = pos;
            return floorData.CanPlaceObjectAt(pos1, Vector2Int.one); // Doesn't have floor
        }
        else
        {
            pos2 = pos;
            gridRectangle = GetGridRectangle(pos1, pos2);
            foreach (Vector3Int posRect in gridRectangle)
            {
                if (!floorData.CanPlaceObjectAt(posRect, Vector2Int.one))
                    return false;
            }
            return true;
        }
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        this.resourceManagement = resourceManagement;
        if (!isFirstClick)
            return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize*gridRectangle.Count);
        return true;
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);

        if (isFirstClick)
        {
            preview.StartShowingFloorPlacementPreview(prefab, pos1);

            isFirstClick = false;
        }
        else
        {
            foreach (Vector3Int posRect in gridRectangle)
            {
                int index = placer.PlaceObject(prefab, grid.CellToWorld(posRect), energyProduction, resourceManagement);

                floorData.AddObjectAt(pos1, size, ID, index);
            } 
        }
    }

    private List<Vector3Int> GetGridRectangle(Vector3Int pos1, Vector3Int pos2)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        int minX = Mathf.Min(pos1.x, pos2.x);
        int maxX = Mathf.Max(pos1.x, pos2.x);
        int minY = Mathf.Min(pos1.y, pos2.y);
        int maxY = Mathf.Max(pos1.y, pos2.y);

        int z = pos1.z;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                positions.Add(new Vector3Int(x, y, z));
            }
        }

        return positions;
    }

    public override void UpdatePreview(Vector3Int gridPosition)
    {
        if (isFirstClick)
            preview.UpdatePosition(grid.CellToWorld(gridPosition), CanPlace(gridPosition, grid, gridData));
        else
        {
            List<Vector3Int> gridPositions = GetGridRectangle(pos1, gridPosition);
            List<Vector3> gridVectorPositions = new List<Vector3>();
            List<bool> canPlace = new List<bool>();
            foreach (Vector3Int pos in gridPositions)
            {
                gridVectorPositions.Add(grid.CellToWorld(pos));
                canPlace.Add(CanPlace(gridPosition, grid, gridData));
            }
            preview.UpdateFloorPosition(gridVectorPositions, canPlace);
        }
    }
}
