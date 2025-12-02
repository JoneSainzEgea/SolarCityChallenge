using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private GridData floorData, furniureData;

    private List<GameObject> placedGameObjects = new();

    [SerializeField] private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furniureData = new();
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePos;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }
        
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furniureData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1);

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // TODO: cambiar esto por los objetos que sean de tipo suelo y los que se puedan poner encima
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furniureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnMousePressed -= PlaceStructure;
        inputManager.OnCancel -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }
}
