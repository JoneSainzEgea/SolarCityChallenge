/* PlacementSystem
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * La clase PlacementSystem se encarga de la comunicación de todo lo que tiene que ver con el sistema de construcción.
 * Recibe toda la información necesaria para ello: input, grid y su visualización, objetos, tipos de objetos y su previsualización, sonidos.
 * Desde este script se gestiona el comienzo y fin de la construcción y demolición, y los efectos sobre los recursos.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System.
 * v1 -03/12/2025- construcción y demolición utilizando distintos tipos de objetos.
 * v2 -09/12/2025- añadido de conexión con ResourceManagement e implementación de UpdateResources.
 * v3 -11/12/2025- inclusión del sistema de conexión de componentes.
 * v4 -04/02/2025- añadido del grid del tejado.
 * 
 * TODO: implementar estado de mover
 */

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid mainGrid;
    [SerializeField] private Grid roofGrid;
    private Grid grid;


    [SerializeField] private GameObject mainGridVisualization;
    [SerializeField] private GameObject roofGridVisualization;
    private GameObject gridVisualization;

    private GridDataManager gridDataManager, mainGridDataManager, roofGridDataManager;    

    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private ResourceManagement resourceManagement;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private SoundFeedback soundFeedback;
    [SerializeField] private AutomaticRoofBuilding roofBuilding;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();

        mainGridDataManager = new();
        mainGridDataManager.InitializeGridData();

        gridDataManager = mainGridDataManager;

        grid = mainGrid;
        gridVisualization = mainGridVisualization;
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

        buildingState = new PlacementState(ID, grid, preview, database, gridDataManager, objectPlacer, resourceManagement, soundFeedback);
        buildingState.EnterState();

        inputManager.OnMousePressed += PlaceStructure;
        inputManager.OnCancel += StopPlacement;
    }

    public void StartRotating()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RotatingState(grid, preview, gridDataManager, objectPlacer, soundFeedback);
        buildingState.EnterState();

        inputManager.OnMousePressed += PlaceStructure; // PlaceStructure llamará a buildingState.OnAction
        inputManager.OnCancel += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RemovingState(grid, preview, database, gridDataManager, objectPlacer, resourceManagement, soundFeedback);
        buildingState.EnterState();

        inputManager.OnMousePressed += PlaceStructure;
        inputManager.OnCancel += StopPlacement;
    }

    public void StartConnecting()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new ConnectingState(grid, preview, database, gridDataManager, objectPlacer, resourceManagement, soundFeedback);
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

    public void BuildRoof()
    {
        StopPlacement();
        
        roofBuilding.Initialize(gridDataManager, grid);

        if(roofGridDataManager == null)
        {
            roofGridDataManager = new();
            roofGridDataManager.InitializeGridData();
        }

        gridDataManager = roofGridDataManager;

        grid = roofGrid;
        gridVisualization = roofGridVisualization;
    }
}
