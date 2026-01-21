/* FloorRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -21/01/2026- 
 */
using UnityEngine;

public class FloorRemoveBehaviour : IRemoveBehaviour
{
    PreviewSystem preview;
    Grid grid;
    GridDataManager gridData;
    GridData selectedData;
    Vector3Int pos;

    public void Initialize(PreviewSystem preview, Grid grid, GridData selectedData, GridDataManager gridData)
    {
        this.preview = preview;
        this.grid = grid;
        this.selectedData = selectedData;
        this.gridData = gridData;

        preview.StartShowingRemovePreview();
    }

    public bool CanRemove(Vector3Int gridPosition)
    {
        this.pos = gridPosition;

        // There's no objects on top of the floor (only furniture)


        return true;
    }

    public void Remove(ObjectPlacer placer, int index)
    {
        selectedData.RemoveObjectAt(pos);
        placer.RemoveGroupObjectAt(index);

        // Removes walls, and wall furniture
    }

    public void UpdatePreview(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}
