/* GridDataManager
 * Jone Sainz Egea
 * 20/01/2026
 *
 * Controlador de los distintos tipos de GridData
 * 
 * v1 -20/01/2026- 
 */
using UnityEngine;

public enum GridDataType { FloorData, FurnitureData, WallData, WallFurnitureData}

public class GridDataManager
{
    private GridData floorData, furnitureData, wallData, wallFurnitureData;
    public void InitializeGridData()
    {
        floorData = new();
        furnitureData = new();
        wallData = new();
        wallFurnitureData = new();
    }

    public GridData GetGridData(GridDataType gridDataType)
    {
        switch (gridDataType)
        {
            case GridDataType.FloorData:
                return floorData;
            case GridDataType.FurnitureData:
                return furnitureData;
            case GridDataType.WallData:
                return wallData;
            case GridDataType.WallFurnitureData:
                return wallFurnitureData;
            default:
                return furnitureData;
        }
    }

    public GridData GetGridDataType(Vector3Int pos, Vector2Int size)
    {
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = furnitureData;
        }
        else if (floorData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = floorData;
        }
        else if (wallData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = wallData;
        }
        else if (wallFurnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = wallFurnitureData;
        }
        return selectedData;
    }

    public bool IsOccupied(Vector3Int pos, Vector2Int size)
    {
        GridData selectedData = GetGridDataType(pos, size);
        if (selectedData == null)
            return false;
        return true;
    }
}
