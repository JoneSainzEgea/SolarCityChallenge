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
 * v2 -30/01/2026- colocación de muros externos al poner suelo.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Floor")]
public class FloorBuildBehaviourSO : BuildBehaviourSO
{
    private Vector2Int size;
    private Vector3Int pos1;
    private Vector3Int pos2;
    List<Vector3Int> gridRectangle;
    List<Vector3Int> gridRectangleEdges;
    private GameObject prefab;
    private int prize;
    [SerializeField] private GameObject wallPrefab;

    [SerializeField] private int wallID;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    private PreviewSystem preview;
    private ObjectPlacer placer;
    private bool isFirstClick = true;

    public event Action<Vector3Int> OnFloorPlaced;

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
        GridData externalWallData = gridData.GetGridData(GridDataType.ExternalWallData);

        int groupIndex = placer.CreateNewGroup();

        HashSet<Vector3Int> notifiedCells = new();

        foreach (Vector3Int pos in gridRectangleEdges)
        {
            placer.PlaceWallGroupObject(wallPrefab, grid.CellToWorld(pos), grid, gridData);
            externalWallData.AddObjectAt(pos, Vector2Int.one, wallID, groupIndex);
            externalWallData.AddObjectToGroup(pos, groupIndex);

            Vector3Int[] neighbours =
            {
                pos + Vector3Int.forward,
                pos + Vector3Int.back,
                pos + Vector3Int.right,
                pos + Vector3Int.left
            };

            foreach (Vector3Int neighbour in neighbours)
            {
                if (!gridRectangle.Contains(neighbour) && notifiedCells.Add(neighbour))
                {
                    NotifyIfWallExists(neighbour);
                }
            }
        }
    }

    private void NotifyIfWallExists(Vector3Int cell)
    {
        GridData externalWallData = gridData.GetGridData(GridDataType.ExternalWallData);

        if (externalWallData.IsOccupied(cell))
            OnFloorPlaced?.Invoke(cell);
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

    private List<Vector3Int> GetGridRectangleEdges(Vector3Int pos1, Vector3Int pos2)
    {
        List<Vector3Int> edges = new List<Vector3Int>();

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
                edges.Add(new Vector3Int(minX, y, minZ));
                return edges;
            }

            for (int z = minZ + 1; z < maxZ; z++)
                edges.Add(new Vector3Int(minX, y, z));
            edges.Add(new Vector3Int(minX, y, minZ));
            edges.Add(new Vector3Int(minX, y, maxZ));
            return edges;
        }

        // Horizontal line
        if (minZ == maxZ)
        {
            for (int x = minX + 1; x < maxX; x++)
                edges.Add(new Vector3Int(x, y, minZ));


            edges.Add(new Vector3Int(minX, y, minZ));
            edges.Add(new Vector3Int(maxX, y, minZ));
            return edges;
        }

        // Full rectangle
        for (int x = minX + 1; x < maxX; x++)
        {
            edges.Add(new Vector3Int(x, y, minZ));
            edges.Add(new Vector3Int(x, y, maxZ));
        }
        for (int z = minZ + 1; z < maxZ; z++)
        {
            edges.Add(new Vector3Int(minX, y, z));
            edges.Add(new Vector3Int(maxX, y, z));
        }
        edges.Add(new Vector3Int(minX, y, minZ));
        edges.Add(new Vector3Int(minX, y, maxZ));
        edges.Add(new Vector3Int(maxX, y, minZ));
        edges.Add(new Vector3Int(maxX, y, maxZ));

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
