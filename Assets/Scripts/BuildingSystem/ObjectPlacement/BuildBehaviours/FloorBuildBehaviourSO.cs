/* FloorBuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject que define el caso específico de construcción del suelo. Hereda de BuildBehaviourSO.
 * Funciona con una bandera que define si es el primer click de la construcción o el segundo.
 * Con el primer click empieza una previsualización dependiendo de la posición del ratón.
 * Con el segundo click intenta colocar el suelo.
 * Coloca paredes en el borde exterior del rectángulo que forma el suelo.
 * 
 * v1 -19/01/2026- construcción del suelo a base de dos clicks mediante una bandera.
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
    Dictionary<Vector3Int, WallType> gridRectangleEdges;
    private GameObject prefab;
    private int prize;
    [SerializeField] private GameObject[] wallPrefabs;

    [SerializeField] private int wallID;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    private PreviewSystem preview;
    private ObjectPlacer placer;
    private bool isFirstClick = true;
    public override void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size, Grid grid, GridDataManager gridData)
    {
        this.prefab = prefab;
        this.size = size;
        this.preview = preview;
        this.grid = grid;
        this.gridData = gridData;

        isFirstClick = true;
        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(Vector3Int pos)
    {
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
            gridRectangleEdges = GetGridRectangleEdges(pos1, pos2);
            return true;
        }
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        this.resourceManagement = resourceManagement;
        this.prize = prize;
        if (!isFirstClick)
            return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize * gridRectangle.Count);
        return true;
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        this.placer = placer;

        GridData floorData = gridData.GetGridData(GridDataType.FloorData);

        if (isFirstClick)
        {
            preview.StopShowingPreview();
            
            preview.StartShowingFloorPlacementPreview(prefab, pos1);

            isFirstClick = false;
        }
        else
        {
            preview.StopShowingFloorPreview();
            int groupIndex = placer.CreateNewGroup();
            foreach (Vector3Int posRect in gridRectangle)
            {
                placer.PlaceGroupObject(prefab, grid.CellToWorld(posRect));
                floorData.AddObjectAt(posRect, size, ID, groupIndex);
                floorData.AddObjectToGroup(posRect, groupIndex);
            }

            PlaceExternalWalls();

            isFirstClick = true;
            preview.StartShowingPlacementPreview(prefab, size);
        }
    }

    private void PlaceExternalWalls()
    {
        GridData wallData = gridData.GetGridData(GridDataType.WallData);

        int groupIndex = placer.CreateNewGroup();

        foreach (KeyValuePair<Vector3Int, WallType> pair in gridRectangleEdges)
        {
            placer.WallPositioning(wallPrefabs, grid.CellToWorld(pair.Key), pair.Value);
            wallData.AddObjectAt(pair.Key, Vector2Int.one, wallID, groupIndex);
            wallData.AddObjectToGroup(pair.Key, groupIndex);
        }
    }

    public override void RemoveResources()
    {
        if (isFirstClick)
            resourceManagement.RemoveResource(ResourceType.Money, prize * gridRectangle.Count);
    }

    private List<Vector3Int> GetGridRectangle(Vector3Int pos1, Vector3Int pos2)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        int minX = Mathf.Min(pos1.x, pos2.x);
        int maxX = Mathf.Max(pos1.x, pos2.x);
        int minZ = Mathf.Min(pos1.z, pos2.z);
        int maxZ = Mathf.Max(pos1.z, pos2.z);

        int y = pos1.y;

        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                positions.Add(new Vector3Int(x, y, z));
            }
        }

        return positions;
    }

    private Dictionary<Vector3Int, WallType> GetGridRectangleEdges(Vector3Int pos1, Vector3Int pos2)
    {
        Dictionary<Vector3Int, WallType> edges = new Dictionary<Vector3Int, WallType>();

        int minX = Mathf.Min(pos1.x, pos2.x);
        int maxX = Mathf.Max(pos1.x, pos2.x);
        int minZ = Mathf.Min(pos1.z, pos2.z);
        int maxZ = Mathf.Max(pos1.z, pos2.z);
        int y = pos1.y;

        // Vertical line
        if (minX == maxX)
        {
            // Single square
            if (minZ == maxZ)
            {
                edges.Add(new Vector3Int(minX, y, minZ), WallType.Single);
                return edges;
            }

            for (int z = minZ + 1; z < maxZ; z++)
                edges.Add(new Vector3Int(minX, y, z), WallType.Vertical);
            edges.Add(new Vector3Int(minX, y, minZ), WallType.VerticalCornerStart);
            edges.Add(new Vector3Int(minX, y, maxZ), WallType.VerticalCornerEnd);
            return edges;
        }

        // Horizontal line
        if (minZ == maxZ)
        {
            for (int x = minX + 1; x < maxX; x++)
                edges.Add(new Vector3Int(x, y, minZ), WallType.Horizontal);


            edges.Add(new Vector3Int(minX, y, minZ), WallType.HorizontalCornerStart);
            edges.Add(new Vector3Int(maxX, y, minZ), WallType.HorizontalCornerEnd);
            return edges;
        }

        // Full rectangle
        for (int x = minX + 1; x < maxX; x++)
        {
            edges.Add(new Vector3Int(x, y, minZ), WallType.Bottom);
            edges.Add(new Vector3Int(x, y, maxZ), WallType.Top);
        }
        for (int z = minZ + 1; z < maxZ; z++)
        {
            edges.Add(new Vector3Int(minX, y, z), WallType.Left);
            edges.Add(new Vector3Int(maxX, y, z), WallType.Right);
        }
        edges.Add(new Vector3Int(minX, y, minZ), WallType.CornerBL);
        edges.Add(new Vector3Int(minX, y, maxZ), WallType.CornerTL);
        edges.Add(new Vector3Int(maxX, y, minZ), WallType.CornerBR);
        edges.Add(new Vector3Int(maxX, y, maxZ), WallType.CornerTR);


        return edges;
    }

    public override void UpdatePreview(Vector3Int gridPosition)
    {
        if (isFirstClick)
            preview.UpdatePosition(grid.CellToWorld(gridPosition), CanPlace(gridPosition));
        else
        {         
            List<Vector3Int> gridPositions = GetGridRectangle(pos1, gridPosition);
            List<Vector3> gridVectorPositions = new List<Vector3>();
            bool validity = true;
            foreach (Vector3Int pos in gridPositions)
            {
                gridVectorPositions.Add(grid.CellToWorld(pos));
                if(!CanPlace(gridPosition))
                    validity = false;
            }
            preview.UpdateFloorPosition(gridVectorPositions, validity);
        }
    }
}
