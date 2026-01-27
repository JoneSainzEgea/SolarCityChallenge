using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[System.Flags]
public enum WallNeighbourMask
{
    None = 0,
    North = 1 << 0,
    South = 1 << 1,
    East = 1 << 2,
    West = 1 << 3
}

public class WallAutoTile : MonoBehaviour
{
    [Header("Variants")]
    [SerializeField] private GameObject singleWall;
    [SerializeField] private GameObject doubleWall;
    [SerializeField] private GameObject cornerWall;
    [SerializeField] private GameObject threeCornerWall;
    [SerializeField] private GameObject fourCornerWall;


    //private void RecalculateWallsAround(Vector3Int cell)
    //{
    //    foreach (Vector3Int dir in directions)
    //    {
    //        Vector3Int pos = cell + dir;


    //        if (wallGrid.HasWall(pos))
    //            wallGrid.GetWall(pos).Recalculate(pos, floorData);
    //    }
    //}

    public void Recalculate(Vector3Int cellPosition, GridData floorData)
    {
        WallNeighbourMask mask = GetMask(cellPosition, floorData);
        ApplyVisual(mask);
    }


    private WallNeighbourMask GetMask(Vector3Int cell, GridData floorData)
    {
        WallNeighbourMask mask = WallNeighbourMask.None;


        if (floorData.IsOccupied(cell + Vector3Int.forward)) mask |= WallNeighbourMask.North;
        if (floorData.IsOccupied(cell + Vector3Int.back)) mask |= WallNeighbourMask.South;
        if (floorData.IsOccupied(cell + Vector3Int.right)) mask |= WallNeighbourMask.East;
        if (floorData.IsOccupied(cell + Vector3Int.left)) mask |= WallNeighbourMask.West;


        return mask;
    }

    private void ApplyVisual(WallNeighbourMask mask)
    {
        DisableAll();


        int connections = CountBits(mask);


        switch (connections)
        {
            case 0:
                Activate(singleWall);
                break;


            case 1:
                Activate(doubleWall);
                SetRotationForSingle(mask);
                break;


            case 2:
                if (IsStraight(mask))
                {
                    Activate(doubleWall);
                    SetRotationForStraight(mask);
                }
                else
                {
                    Activate(cornerWall);
                    SetRotationForCorner(mask);
                }
                break;


            case 3:
                Activate(threeCornerWall);
                SetRotationForTJunction(mask);
                break;


            case 4:
                Activate(fourCornerWall);
                break;
        }
    }

    private int CountBits(WallNeighbourMask mask)
    {
        int count = 0;
        if (mask.HasFlag(WallNeighbourMask.North)) count++;
        if (mask.HasFlag(WallNeighbourMask.South)) count++;
        if (mask.HasFlag(WallNeighbourMask.East)) count++;
        if (mask.HasFlag(WallNeighbourMask.West)) count++;
        return count;
    }

    private bool IsStraight(WallNeighbourMask mask)
    {
        bool vertical = mask.HasFlag(WallNeighbourMask.North) && mask.HasFlag(WallNeighbourMask.South);
        bool horizontal = mask.HasFlag(WallNeighbourMask.East) && mask.HasFlag(WallNeighbourMask.West);


        return vertical || horizontal;
    }

    private void DisableAll()
    {
        singleWall.SetActive(false);
        doubleWall.SetActive(false);
        cornerWall.SetActive(false);
        threeCornerWall.SetActive(false);
        fourCornerWall.SetActive(false);
    }


    private void Activate(GameObject obj)
    {
        obj.SetActive(true);
        transform.rotation = Quaternion.identity;
    }

    private void SetRotationForSingle(WallNeighbourMask mask)
    {
        if (mask.HasFlag(WallNeighbourMask.North))
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (mask.HasFlag(WallNeighbourMask.East))
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (mask.HasFlag(WallNeighbourMask.South))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (mask.HasFlag(WallNeighbourMask.West))
            transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    private void SetRotationForStraight(WallNeighbourMask mask)
    {
        // Vertical (N-S) = rotación base
        if (mask.HasFlag(WallNeighbourMask.North))
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void SetRotationForCorner(WallNeighbourMask mask)
    {
        if (mask.HasFlag(WallNeighbourMask.North) && mask.HasFlag(WallNeighbourMask.East))
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (mask.HasFlag(WallNeighbourMask.East) && mask.HasFlag(WallNeighbourMask.South))
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (mask.HasFlag(WallNeighbourMask.South) && mask.HasFlag(WallNeighbourMask.West))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    private void SetRotationForTJunction(WallNeighbourMask mask)
    {
        // Rotamos para que el lado "abierto" mire al lado sin vecino
        if (!mask.HasFlag(WallNeighbourMask.North))
            transform.rotation = Quaternion.identity;
        else if (!mask.HasFlag(WallNeighbourMask.East))
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (!mask.HasFlag(WallNeighbourMask.South))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 270, 0);
    }
}
