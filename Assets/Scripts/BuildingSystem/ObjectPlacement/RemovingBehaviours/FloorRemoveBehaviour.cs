/* FloorRemoveBehaviour
 * Jone Sainz Egea
 * 21/01/2026
 *
 * ScriptableObject que define el caso específico de eliminación del suelo. Hereda de la interfaz IRemoveBehaviour.
 * Para saber si se puede eliminar el suelo, compureba que no haya muebles encima de él.
 * Elimina el grupo completo del suelo de golpe, junto con sus paredes y decoraciones de pared, además de sus datos.
 * Actualiza los recursos, devolviendo el dinero invertido.
 * 
 * v1 -21/01/2026- eliminación del suelo junto a paredes y decoraciones de pared.
 */
using System.Collections.Generic;
using UnityEngine;

public class FloorRemoveBehaviour : IRemoveBehaviour
{
    PreviewSystem preview;
    ObjectPlacer placer;
    Grid grid;
    GridDataManager gridData;
    GridData selectedData;
    Vector3Int pos;
    List<Vector3Int> floorPositions;

    public void Initialize(PreviewSystem preview, ObjectPlacer placer, Grid grid, GridData selectedData, GridDataManager gridData)
    {
        this.preview = preview;
        this.placer = placer;
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

    /*
     * Removes floor data and object, also walls and wall furnitures
     */
    public void Remove(ObjectPlacer placer, int index)
    {
        selectedData.RemoveObjectAt(pos);
        placer.RemoveGroupObjectAt(index);

        GridData wallData = gridData.GetGridData(GridDataType.WallData);
        GridData externalWallData = gridData.GetGridData(GridDataType.ExternalWallData);
        GridData wallFurnitureData = gridData.GetGridData(GridDataType.WallFurnitureData);

        foreach (Vector3Int position in floorPositions)
        {
            if (wallData.IsOccupied(position))
            {
                int i = wallData.GetGroupRepresentationIndex(position);
                wallData.RemoveObjectAt(position);
                placer.RemoveGroupObjectAt(i);
            }
            if (externalWallData.IsOccupied(position))
            {
                int i = externalWallData.GetGroupRepresentationIndex(position);
                externalWallData.RemoveObjectAt(position);
                placer.RemoveGroupObjectAt(i);
            }
            if (wallFurnitureData.IsOccupied(position))
            {
                int i = wallFurnitureData.GetRepresentationIndex(position);
                wallFurnitureData.RemoveObjectAt(position);
                placer.RemoveObjectAt(i);
            }
        }
    }

    public void UpdateResources(ResourceManagement resourceManagement, int prize)
    {
        resourceManagement.AddResource(ResourceType.Money, prize * floorPositions.Count);

        // TODO: return money from furniture and manually placed walls
    }

    public void UpdatePreview(Vector3Int gridPosition, GameObject gameObjectToRemove)
    {
        throw new System.NotImplementedException();
    }
}
