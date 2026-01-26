/* FloorRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -21/01/2026- 
 */
using System.Collections.Generic;
using UnityEngine;

public class FloorRemoveBehaviour : IRemoveBehaviour
{
    PreviewSystem preview;
    Grid grid;
    GridDataManager gridData;
    GridData selectedData;
    Vector3Int pos;
    List<Vector3Int> floorPositions;

    public void Initialize(PreviewSystem preview, Grid grid, GridData selectedData, GridDataManager gridData)
    {
        this.preview = preview;
        this.grid = grid;
        this.selectedData = selectedData;
        this.gridData = gridData;

        preview.StartShowingRemovePreview();
    }

    /*
     * Returns true if there's no furniture on top of it 
     */
    public bool CanRemove(Vector3Int gridPosition)
    {
        this.pos = gridPosition;

        floorPositions = selectedData.GetGroupPositions(gridPosition);
        GridData furnitureData = gridData.GetGridData(GridDataType.FurnitureData);

        foreach (Vector3Int position in floorPositions)
        {
            if (furnitureData.IsOccupied(position))
                return false;
        }

        return true;
    }

    public void Remove(ObjectPlacer placer, int index)
    {
        selectedData.RemoveObjectAt(pos);
        placer.RemoveGroupObjectAt(index);

        GridData wallData = gridData.GetGridData(GridDataType.WallData);
        GridData wallFurnitureData = gridData.GetGridData(GridDataType.WallFurnitureData);

        foreach (Vector3Int position in floorPositions)
        {
            if (wallData.IsOccupied(position))
            {
                int i = wallData.GetRepresentationIndex(position);
                wallData.RemoveObjectAt(position);
                placer.RemoveObjectAt(i);
            }
            if (wallFurnitureData.IsOccupied(position))
            {
                int i = wallFurnitureData.GetRepresentationIndex(position);
                wallFurnitureData.RemoveObjectAt(position);
                placer.RemoveObjectAt(i);
            }
        }
        // Removes walls, and wall furniture
    }

    public void UpdatePreview(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}
