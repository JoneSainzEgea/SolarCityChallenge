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
 * v3 -20/01/2025- Cambio de funcionamiento implementando BuildBehaviourSO y GridDataManager
 */

using UnityEngine;

public class PlacementState : IBuildingState
{
    private BuildBehaviourSO behaviour;
    private ResourceManagement resourceManagement;
    private SoundFeedback soundFeedback;
    
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridDataManager gridData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridDataManager gridData, ObjectPlacer objectPlacer, ResourceManagement resourceManagement, SoundFeedback soundFeedback)
    {
        ID = iD;
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
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            behaviour = database.objectsData[selectedObjectIndex].BuildBehaviour;
            behaviour.StartPreview(previewSystem, database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size, grid, gridData);
        }
        else
            throw new System.Exception($"No object with ID {ID}");
    }

    public bool OnAction(Vector3Int gridPosition)
    {
        if (!behaviour.HasMoney(resourceManagement, database.objectsData[selectedObjectIndex].Prize))
        {
            // TODO: change sound feedback and animation for money validity
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        if (!behaviour.CanPlace(gridPosition))
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }
        soundFeedback.PlaySound(SoundType.Place);
        behaviour.Place(objectPlacer, ID, database.objectsData[selectedObjectIndex].EnergyProduction);

        behaviour.UpdatePreview(gridPosition);
        return true;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        behaviour.UpdatePreview(gridPosition);
    }
    public void UpdateResources()
    {
        resourceManagement.RemoveResource(ResourceType.Money, database.objectsData[selectedObjectIndex].Prize);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
}
