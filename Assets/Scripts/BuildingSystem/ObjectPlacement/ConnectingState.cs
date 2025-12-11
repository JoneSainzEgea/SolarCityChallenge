/* ConnectingState
 * Jone Sainz Egea
 * 11/12/2025
 * 
 * Estado que hereda de la interfaz IBuildingState, implementando sus métodos.
 * Este estado se encarga de la conexión de objetos existentes en el grid.
 * Actualiza el estado de los componentes.
 * 
 * v1 -11/12/2025- 
 * 
 * TODO: añadir previsualización de la conexión.
 */
using UnityEngine;

public class ConnectingState : IBuildingState
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

    public ConnectingState(Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, ResourceManagement resourceManagement, SoundFeedback soundFeedback)
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
        previewSystem.StartShowingConnectionPreview();
    }

    public bool OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if (selectedData == null)
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
            soundFeedback.PlaySound(SoundType.Place);
            if (!objectPlacer.isConnecting)
                objectPlacer.StartConnectingAt(gameObjectIndex);
            else
                objectPlacer.StopConnectingAt(gameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        //previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        return true;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // TODO: visualización
    }

    public void UpdateResources()
    {
        Debug.Log("Updates resources");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
}
