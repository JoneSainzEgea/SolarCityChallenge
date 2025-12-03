/* IBuildingState
 * Jone Sainz Egea
 * 03/12/2025
 * 
 * Interfaz que define cómo serán los diferentes estados de construcción.
 * Se definen las funciones de EnterState, OnAction, UpdateState y EndState.
 * Necesita la información del Vector3Int gridPosition.
 * 
 * Inspirado en el código de: Sunny Valley Studio, Grid Placement System
 * v1 -03/12/2025- EnterState, OnAction, UpdateState y EndState
 */

using UnityEngine;

public interface IBuildingState
{
    void EnterState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
    void EndState();
}