//02/02/2026
// Jone Sainz Egea
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRoofBuilding : MonoBehaviour
{
    private GridDataManager gridDataManager;

    private Grid mainGrid;

    private GridData floorData;

    private bool isBuilt = false;

    List<Vector3Int> floorPositions;

    [SerializeField] private GameObject roof;

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

            roof.transform.position = worldCenter;
            roof.transform.position += Vector3.up;
            roof.transform.localScale = worldSize;
        }
    }

    private void MoveCamera()
    {
        // TODO: Move camera to top level
    }
}
