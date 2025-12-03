using UnityEngine;

public interface IBuildingState
{
    void EnterState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
    void EndState();
}