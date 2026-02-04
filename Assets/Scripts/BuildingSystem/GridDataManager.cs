/* GridDataManager
 * Jone Sainz Egea
 * 20/01/2026
 *
 * Controlador de los distintos tipos de GridData. Funciona como sistema central, facilita la comunicación.
 * Define e inicializa los distintos tipos de GridData: FloorData, WallData, FurnitureData y WallFurnitureData.
 * Contiene métodos para acceder a los GridData a través del tipo o de la posición.
 * También para obtener el tipo de GridData de una posición. Y para saber si hay data de una posición.
 * 
 * v1 -20/01/2026- InitializeGridData, GetGridData, GetGridDataFromPos, GetGridDataType, IsOccupied.
 */
using UnityEngine;

public enum GridDataType { FloorData, FurnitureData, WallData, WallFurnitureData, ExternalWallData}

public class GridDataManager
{
    private GridData floorData, furnitureData, wallData, wallFurnitureData, externalWallData;

    public void InitializeGridData()
    {
        floorData = new();
        furnitureData = new();
        wallData = new();
        wallFurnitureData = new();
        externalWallData = new();
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
            case GridDataType.ExternalWallData:
                return externalWallData;
            default:
                return furnitureData;
        }
    }

    public GridData GetGridDataFromPos(Vector3Int pos, Vector2Int size)
    {
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = furnitureData;
        }
        else if (wallFurnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = wallFurnitureData;
        }
        else if (wallData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = wallData;
        }
        else if (externalWallData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = externalWallData;
        }
        else if (floorData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = floorData;
        }
        return selectedData;
    }

    public GridDataType GetGridDataType(Vector3Int pos, Vector2Int size)
    {
        GridDataType selectedData = GridDataType.FurnitureData;
        if (furnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = GridDataType.FurnitureData;
        }
        else if (wallFurnitureData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = GridDataType.WallFurnitureData;
        }
        else if (wallData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = GridDataType.WallData;
        }
        else if (externalWallData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = GridDataType.ExternalWallData;
        }
        else if (floorData.CanPlaceObjectAt(pos, size) == false)
        {
            selectedData = GridDataType.FloorData;
        }
        return selectedData;
    }

    public bool IsOccupied(Vector3Int pos, Vector2Int size)
    {
        GridData selectedData = GetGridDataFromPos(pos, size);
        if (selectedData == null)
            return false;
        return true;
    }
}
