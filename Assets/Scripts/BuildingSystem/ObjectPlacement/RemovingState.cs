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
 * 
 * TODO: añadir previsualización que muestra el objeto de la escena en rojo si se va a eliminar
 */

using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private int gameObjectID = -1;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    ResourceManagement resourceManagement;
    SoundFeedback soundFeedback;

    public RemovingState(Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, ResourceManagement resourceManagement, SoundFeedback soundFeedback)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
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
        GridData selectedData = null;
        if(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if(selectedData == null)
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            gameObjectID = selectedData.GetRepresentationID(gridPosition);
            if (gameObjectIndex == -1 || gameObjectID == -1)
                return false;
            soundFeedback.PlaySound(SoundType.Remove);
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
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

    // Cuando no se puede colocar un objeto en esa posición devuelve true, hay un objeto que se puede eliminar en esa casilla
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }
}
