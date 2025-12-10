/* PlacementState
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Estado que hereda de la interfaz IBuildingState, implementando sus métodos.
 * Este estado se encarga de la creación de nuevos objetos, su previsualización, su ubicación, y almacenamiento de la posición en la que se sitúa.
 * Recibe el ID del objeto que se va a colocar y la información necesaria para revisar la posibilidad de colocarlo y dar retroalimentación al usuario.
 * Actauliza la información del dinero y la energía después de colocarlo.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- Identifica el objeto, lo previsualiza, comprueba que se pueda colocar, llama a colocarlo y almacena la información de su posición, tipo de objeto y tamaño.
 * v2 -09/12/2025- Comprueba que haya dinero suficiente, actualiza valores de dinero y energía.
 */

using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    ResourceManagement resourceManagement;
    SoundFeedback soundFeedback;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, ResourceManagement resourceManagement, SoundFeedback soundFeedback)
    {
        ID = iD;
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
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {ID}");
    }

    public bool OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        bool moneyValidity = CheckMoneyValidity();
        if (moneyValidity == false)
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        soundFeedback.PlaySound(SoundType.Place);
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab,
                                             grid.CellToWorld(gridPosition),
                                             database.objectsData[selectedObjectIndex].EnergyProduction,
                                             resourceManagement);
        
        // TODO: cambiar la siguiente línea, ahora mismo el objeto 0 es el "suelo", implementar nuevo sistema
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
        return true;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
    public void UpdateResources()
    {
        resourceManagement.RemoveResource(ResourceType.Money, database.objectsData[selectedObjectIndex].Prize);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // TODO: cambiar esto por los objetos que sean de tipo suelo y los que se puedan poner encima
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private bool CheckMoneyValidity()
    {
        if (resourceManagement.GetResourceAmount(ResourceType.Money) < database.objectsData[selectedObjectIndex].Prize)
            return false;
        return true;
    }
}
