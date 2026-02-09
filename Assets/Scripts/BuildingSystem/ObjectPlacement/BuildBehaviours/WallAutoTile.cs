// 27/01/2026
// Uso de flags bit a bit para hacer combinaciones
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[System.Flags]
public enum WallNeighbourMask
{
    None = 0,
    North = 1 << 0,
    South = 1 << 1,
    East = 1 << 2,
    West = 1 << 3
}

public class WallAutoTile : MonoBehaviour
{
    [SerializeField] private FloorBuildBehaviourSO floorBuild;
    private GridDataManager gridData;
    private Grid grid;
    private Vector3Int wallCell;

    [Header("Wall Variants")]
    [SerializeField] private GameObject singleWall; // One wall on one side on the tile
    [SerializeField] private GameObject doubleWall; // Two walls in one tile, parallel
    [SerializeField] private GameObject cornerWall; // Two walls in one tile, forming a corner
    [SerializeField] private GameObject threeCornerWall; // Three walls on one tile
    [SerializeField] private GameObject fourCornerWall; // Four walls in one tile

    private void OnEnable()
    {
        floorBuild.OnFloorPlaced += OnFloorPlaced;
    }

    private void OnDisable()
    {
        floorBuild.OnFloorPlaced -= OnFloorPlaced;
    }

    public void Initialize(Grid grid, GridDataManager gridData)
    {
        this.grid = grid;
        this.gridData = gridData;
        wallCell = grid.WorldToCell(transform.position);
        Recalculate();
    }

    private void OnFloorPlaced(Vector3Int cellToCheck)
    {
        if (cellToCheck == wallCell)
            Recalculate();
    }

    public void Recalculate()
    {
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        WallNeighbourMask mask = GetMask(wallCell, floorData);

        transform.position = wallCell;
        transform.rotation = Quaternion.identity;

        ApplyVisual(mask);
    }


    private WallNeighbourMask GetMask(Vector3Int cell, GridData floorData)
    {
        WallNeighbourMask mask = WallNeighbourMask.None;

        if (floorData.IsOccupied(cell + Vector3Int.forward)) mask |= WallNeighbourMask.North;
        if (floorData.IsOccupied(cell + Vector3Int.back)) mask |= WallNeighbourMask.South;
        if (floorData.IsOccupied(cell + Vector3Int.right)) mask |= WallNeighbourMask.East;
        if (floorData.IsOccupied(cell + Vector3Int.left)) mask |= WallNeighbourMask.West;

        return mask;
    }

    private void ApplyVisual(WallNeighbourMask mask)
    {
        DisableAll();

        int connections = CountBits(mask);

        switch (connections)
        {
            case 0:
                Activate(fourCornerWall);
                break;
            case 1:
                Activate(threeCornerWall);
                SetRotationForThreeCorners(mask);
                break;
            case 2:
                if (IsStraight(mask))
                {
                    Activate(doubleWall);
                    SetRotationForStraight(mask);
                }
                else
                {
                    Activate(cornerWall);
                    SetRotationForCorner(mask);
                }
                break;
            case 3:
                Activate(singleWall);
                SetRotationForSingle(mask);
                break;
            case 4:
                DisableAll();
                Debug.Log("No wall");
                break;
        }
    }

    private int CountBits(WallNeighbourMask mask)
    {
        int count = 0;
        if (mask.HasFlag(WallNeighbourMask.North)) count++;
        if (mask.HasFlag(WallNeighbourMask.South)) count++;
        if (mask.HasFlag(WallNeighbourMask.East)) count++;
        if (mask.HasFlag(WallNeighbourMask.West)) count++;
        return count;
    }

    private bool IsStraight(WallNeighbourMask mask)
    {
        bool vertical = mask.HasFlag(WallNeighbourMask.North) && mask.HasFlag(WallNeighbourMask.South);
        bool horizontal = mask.HasFlag(WallNeighbourMask.East) && mask.HasFlag(WallNeighbourMask.West);
        return vertical || horizontal;
    }

    private void DisableAll()
    {
        singleWall.SetActive(false);
        doubleWall.SetActive(false);
        cornerWall.SetActive(false);
        threeCornerWall.SetActive(false);
        fourCornerWall.SetActive(false);
    }

    private void Activate(GameObject obj)
    {
        obj.SetActive(true);
        transform.rotation = Quaternion.identity;
    }

    private void SetRotationForSingle(WallNeighbourMask mask)
    {
        if (!mask.HasFlag(WallNeighbourMask.North))
        {
            transform.position += Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (!mask.HasFlag(WallNeighbourMask.East))
        {
            transform.position += Vector3.right + Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (!mask.HasFlag(WallNeighbourMask.South))
        {
            transform.position += Vector3.right;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    private void SetRotationForStraight(WallNeighbourMask mask)
    {
        if (mask.HasFlag(WallNeighbourMask.North))
            transform.rotation = Quaternion.identity;
        else
        {
            transform.position += Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void SetRotationForCorner(WallNeighbourMask mask)
    {
        if (mask.HasFlag(WallNeighbourMask.North) && mask.HasFlag(WallNeighbourMask.East))
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (mask.HasFlag(WallNeighbourMask.East) && mask.HasFlag(WallNeighbourMask.South))
        {
            transform.position += Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (mask.HasFlag(WallNeighbourMask.South) && mask.HasFlag(WallNeighbourMask.West))
        {
            transform.position += Vector3.right + Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.position += Vector3.right;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    private void SetRotationForThreeCorners(WallNeighbourMask mask)
    {
        if (mask.HasFlag(WallNeighbourMask.North))
        {
            transform.rotation = Quaternion.identity;
        }
        else if (mask.HasFlag(WallNeighbourMask.South))
        {
            transform.position += Vector3.forward + Vector3.right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (mask.HasFlag(WallNeighbourMask.East))
        {
            transform.position += Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if(mask.HasFlag(WallNeighbourMask.West))
        {
            transform.position += Vector3.right;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }
}
