/* IRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * Interfaz que define los métodos que tienen que implementar los comportamientos de eliminación de objetos.
 * Incluye los métodos: Initialize, CanRemove, Remove, UpdateResources y UpdatePreview
 * 
 * v1 -21/01/2026- Initialize, CanRemove, Remove, UpdateResources, UpdatePreview.
 */
using UnityEngine;

public interface IRemoveBehaviour
{
    public abstract void Initialize(PreviewSystem preview, ObjectPlacer placer, Grid grid, GridData selectedData, GridDataManager gridData);

    public abstract bool CanRemove(Vector3Int gridPosition);

    public abstract void Remove(ObjectPlacer placer, int ID);

    public abstract void UpdateResources(ResourceManagement resourceManagement, int prize);

    public abstract void UpdatePreview(Vector3Int gridPosition, GameObject gameObjectToRemove);
}
