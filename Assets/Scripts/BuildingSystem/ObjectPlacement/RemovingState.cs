/* RemovingState
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Estado que hereda de la interfaz IBuildingState, implementando sus métodos.
 * Este estado se encarga de la eliminación de objetos existentes en el grid.
 * Recibe el ID del objeto que se va a colocar y la información necesaria para revisar la posibilidad de colocarlo y dar retroalimentación al usuario.
 * Actualiza la información del dinero y la energía.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- comprueba que no se puede colocar un objeto en esa posición, elimina el objeto que hay en esa posición y lo elimina del almacenamiento del GridData.
 * v2 -09/12/2025- actualiza valores de dinero y energía.
 * v3 -20/01/2026- introducción de GridDataManager
 * 
 * TODO: añadir previsualización que muestra el objeto de la escena en rojo si se va a eliminar
 */

using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private int gameObjectID = -1;
    private IRemoveBehaviour behaviour;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridDataManager gridData;
    ObjectPlacer objectPlacer;
    ResourceManagement resourceManagement;
    SoundFeedback soundFeedback;

    public RemovingState(Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridDataManager gridData, ObjectPlacer objectPlacer, ResourceManagement resourceManagement, SoundFeedback soundFeedback)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.resourceManagement = resourceManagement;
        this.soundFeedback = soundFeedback;
    }

    public void EnterState()
    {
        previewSystem.StartShowingRemovePreview();
    }

    public bool OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = gridData.GetGridDataFromPos(gridPosition, Vector2Int.one);

        if(selectedData == null)
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        else if (gridData.GetGridDataType(gridPosition, Vector2Int.one) == GridDataType.FloorData)
        {
            behaviour = new FloorRemoveBehaviour();
            behaviour.Initialize(previewSystem, grid, selectedData, gridData);
        }
        else
        {
            behaviour = new DefaultRemoveBehaviour();
            behaviour.Initialize(previewSystem, grid, selectedData, gridData);
        }

        if (!behaviour.CanRemove(gridPosition))
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
        gameObjectID = selectedData.GetRepresentationID(gridPosition);
        if (gameObjectIndex == -1 || gameObjectID == -1)
            return false;

        soundFeedback.PlaySound(SoundType.Remove);
        behaviour.Remove(objectPlacer, gameObjectIndex);

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        return true;
    }
    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }

    public void UpdateResources()
    {
        resourceManagement.AddResource(ResourceType.Money, database.objectsData[gameObjectID].Prize);
        resourceManagement.RemoveResource(ResourceType.Energy, database.objectsData[gameObjectID].EnergyProduction);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
    
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return gridData.IsOccupied(gridPosition, Vector2Int.one);
    }
}
