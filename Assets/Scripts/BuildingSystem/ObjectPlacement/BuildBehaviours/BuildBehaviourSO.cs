/* BuildBehaviourSO
 * Jone Sainz Egea
 * 19/01/2026
 *
 * ScriptableObject abstracto que define los métodos que tienen que sobreescribir los distintos tipos de construcción de elemenots.
 * Contiene los métodos: StartPreview, CanPlace, HasMoney, Place,RemoveResources y UpdatePreview.
 * 
 * v1 -19/01/2026- StartPreview, CanPlace, HasMoney, Place,RemoveResources, UpdatePreview.
 */

using UnityEngine;

public abstract class BuildBehaviourSO : ScriptableObject
{
    public abstract void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size, Grid grid, GridDataManager gridData);

    public abstract bool CanPlace(Vector3Int gridPosition);

    public abstract bool HasMoney(ResourceManagement resourceManagement, int prize);

    public abstract void Place(ObjectPlacer placer, int ID, int energyProduction);

    public abstract void RemoveResources();

    public abstract void UpdatePreview(Vector3Int gridPosition);
}
