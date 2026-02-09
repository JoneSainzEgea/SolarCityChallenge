/* DefaultRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define el caso genérico de eliminación de objetos. Hereda de la interfaz IRemoveBehaviour.
 * Se encarga de la comprobación de que se pueda eliminar, la eliminación del objeto y sus datos y la actualización de recursos.
 * 
 * v1 -21/01/2026- inicializa y elimina el objeto que haya en la posición indicada.
 */
using UnityEngine;

public class DefaultRemoveBehaviour : IRemoveBehaviour
{
    PreviewSystem preview;
    ObjectPlacer placer;
    Grid grid;
    GridDataManager gridData;
    GridData selectedData;
    Vector3Int pos;
    
    public void Initialize(PreviewSystem preview, ObjectPlacer placer, Grid grid, GridData selectedData, GridDataManager gridData)
    {
        this.preview = preview;
        this.placer = placer;
        this.grid = grid;
        this.selectedData = selectedData;
        this.gridData = gridData;

        preview.StartShowingRemovePreview();
    }

    public bool CanRemove(Vector3Int gridPosition)
    {
        this.pos = gridPosition;

        return true;
    }

    public void Remove(ObjectPlacer placer, int index)
    {
        selectedData.RemoveObjectAt(pos);
        placer.RemoveObjectAt(index);
    }

    public void UpdateResources(ResourceManagement resourceManagement, int prize)
    {

    }

    public void UpdatePreview(Vector3Int gridPosition, GameObject gameObjectToRemove)
    {
        preview.UpdateRemoval(gridPosition, gameObjectToRemove, false);
    }
}
