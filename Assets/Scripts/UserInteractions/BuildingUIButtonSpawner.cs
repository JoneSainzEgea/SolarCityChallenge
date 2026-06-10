using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUIButtonSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlacementSystem placement;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private BuildingUIButton buttonPrefab;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private int firstObjectID;
    [SerializeField] private int lastObjectID;

    private void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = firstObjectID; i <= lastObjectID; i++)
        {
            BuildingUIButton button = Instantiate(buttonPrefab, buttonsParent);
            ObjectDataSO objectData = database.objectsData[i];
            button.SetUp(objectData.Icon, objectData.Name, () => OnButtonPressed(objectData.ID));
        }
    }

    private void OnButtonPressed(int id)
    {
        Debug.Log("Clicked on a button");
        placement.StartPlacement(id);
    } 
}
