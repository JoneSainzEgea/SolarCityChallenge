/* DefaultRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -21/01/2026- 
 */
using UnityEngine;

public class DefaultRemoveBehaviour : IRemoveBehaviour
{
    PreviewSystem preview;
    Grid grid;
    GridDataManager gridData;
    GridData selectedData;
    Vector3Int pos;
    
    public void Initialize(PreviewSystem preview, Grid grid, GridDataManager gridData)
    {
        this.preview = preview;
        this.grid = grid;
        this.gridData = gridData;

        preview.StartShowingRemovePreview();
    }

    public bool CanRemove(Vector3Int gridPosition)
    {
        this.pos = gridPosition;
        
        selectedData = gridData.GetGridDataFromPos(gridPosition, Vector2Int.one);
        if(selectedData == null)
            return false;
        else
            return true;
    }

    public void Remove(ObjectPlacer placer, int index)
    {
        selectedData.RemoveObjectAt(pos);
        placer.RemoveObjectAt(index);
    }

    public void UpdatePreview(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}
