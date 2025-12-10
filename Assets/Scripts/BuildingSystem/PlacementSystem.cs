/* PlacementSystem
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * La clase PlacementSystem se encarga de la comunicación de todo lo que tiene que ver con el sistema de construcción.
 * Recibe toda la información necesaria para ello: input, grid y su visualización, objetos, tipos de objetos y su previsualización, sonidos.
 * Desde este script se gestiona el comienzo y fin de la construcción y demolición, y los efectos sobre los recursos.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- construcción y demolición utilizando distintos tipos de objetos
 * v2 -09/12/2025- añadido de conexión con ResourceManagement e implementación de UpdateResources
 * 
 * TODO: implementar estado de mover
 */

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;

    [SerializeField] private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    [SerializeField] private PreviewSystem preview;

    [SerializeField] private ResourceManagement resourceManagement;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField] private SoundFeedback soundFeedback;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
        
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer, resourceManagement, soundFeedback);
        buildingState.EnterState();

        inputManager.OnMousePressed += PlaceStructure;
        inputManager.OnCancel += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RemovingState(grid, preview, database, floorData, furnitureData, objectPlacer, resourceManagement, soundFeedback);
        buildingState.EnterState();

        inputManager.OnMousePressed += PlaceStructure;
        inputManager.OnCancel += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsInteractingWithUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        if (buildingState.OnAction(gridPosition))
            buildingState.UpdateResources();
    }

    private void StopPlacement()
    {
        if(buildingState == null)
            return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnMousePressed -= PlaceStructure;
        inputManager.OnCancel -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}
