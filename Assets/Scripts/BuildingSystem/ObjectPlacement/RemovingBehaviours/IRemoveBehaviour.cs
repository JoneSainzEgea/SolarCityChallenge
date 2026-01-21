/* IRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define
 * 
 * v1 -21/01/2026- 
 */
using UnityEngine;

public interface IRemoveBehaviour
{
    public abstract void Initialize(PreviewSystem preview, Grid grid, GridData selectedData, GridDataManager gridData);

    public abstract bool CanRemove(Vector3Int gridPosition);

    public abstract void Remove(ObjectPlacer placer, int ID);

    public abstract void UpdatePreview(Vector3Int gridPosition);
}
