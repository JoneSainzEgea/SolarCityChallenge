using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private GameObject cube, blockPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private InputManager gridInput;

    void Update()
    {
        Vector3 selectedPosition = gridInput.GetSelectedMapPosition();
        Vector3Int cellPosition = grid.WorldToCell(selectedPosition);

        cube.transform.position = grid.GetCellCenterWorld(cellPosition);

        if (gridInput.GetPlacementInput())
        {
            Instantiate(blockPrefab, cube.transform.position, Quaternion.identity);
        }
    }
}
