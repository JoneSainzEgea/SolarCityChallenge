/* AutomaticRoofBuilding
 * Jone Sainz Egea
 * 02/02/2026
 *
 * Clase que se encarga de la creación automática del tejado de un suelo específico almacenado
 * Incluye los métodos: Initialize, CanRemove, Remove, UpdateResources y UpdatePreview
 * 
 * v1 -21/01/2026- Initialize, CanRemove, Remove, UpdateResources, UpdatePreview.
 */
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRoofBuilding : MonoBehaviour
{
    private GridDataManager gridDataManager;

    private Grid mainGrid;

    private GridData floorData;

    private bool isBuilt = false;

    List<Vector3Int> floorPositions;

    [SerializeField] private GameObject roofVisual;
    [SerializeField] private GameObject roofGrid;

    public void Initialize(GridDataManager gridDataManager, Grid mainGrid)
    {
        this.gridDataManager = gridDataManager;
        this.mainGrid = mainGrid;

        MoveCamera();

        if (isBuilt)
            return;

        BuildRoof();

        isBuilt = true;
    }

    private void BuildRoof()
    {
        floorData = gridDataManager.GetGridData(GridDataType.FloorData);

        floorPositions = floorData.GetAllPositions();

        //TODO: caso de que no sea rectangular
        bool isRectangular = true;

        if (isRectangular)
        {
            Vector3Int min = floorPositions[0];
            Vector3Int max = floorPositions[0];

            foreach (Vector3Int cell in floorPositions)
            {
                min = Vector3Int.Min(min, cell);
                max = Vector3Int.Max(max, cell);
            }

            Vector3 worldMin = mainGrid.CellToWorld(min);
            Vector3 worldMax = mainGrid.CellToWorld(max + Vector3Int.one);

            Vector3 worldSize = worldMax - worldMin;
            Vector3 worldCenter = worldMin + worldSize / 2f;

            roofVisual.transform.position = new Vector3(worldCenter.x, 2.9f, worldCenter.z);
            roofVisual.transform.localScale = worldSize;

            roofVisual.SetActive(true);            

            roofGrid.transform.position = new Vector3(worldCenter.x, 2.9f, worldCenter.z);
        }
    }

    private void MoveCamera()
    {
        // TODO: mover la cámara para que enfoque el nivel superior
    }
}
