using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingState : IBuildingState
{
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridDataManager gridData;
    private ObjectPlacer objectPlacer;
    private SoundFeedback soundFeedback;

    public RotatingState(Grid grid, PreviewSystem previewSystem, GridDataManager gridData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;
    }

    public void EnterState()
    {
        // TODO: preview sobre el ratón de rotación
        previewSystem.StopShowingPreview();
    }

    public bool OnAction(Vector3Int gridPosition)
    {
        if (gridData.IsOccupied(gridPosition, Vector2Int.one) == false)
        {
            return false;
        }

        GridData selectedData = gridData.GetGridDataFromPos(gridPosition, Vector2Int.one);

        if (selectedData == null)
        {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return false;
        }

        int gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

        if (gameObjectIndex != -1)
        {
            GameObject objectToRotate = objectPlacer.GetGameObjectAt(gameObjectIndex);
            if (objectToRotate != null)
            {
                objectToRotate.transform.Rotate(0, 90, 0);
                soundFeedback.PlaySound(SoundType.Place);
                return true;
            }
        }
        return false;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // TODO: Resaltar el objeto que está bajo el ratón
    }

    public void UpdateResources() { }

    public void EndState() { }
}
