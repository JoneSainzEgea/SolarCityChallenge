/* FloorBuildBehaviour
 * Jone Sainz Egea
 * 19/01/2026
 *
 * 
 * v1 -19/01/2026- .
 */

using UnityEngine;

public class FloorBuildBehaviour : IBuildBehaviour
{
    private Grid grid;
    private GridData floorData;
    private ObjectsDatabaseSO database;
    private ObjectPlacer placer;
    private PreviewSystem preview;
    private int index;

    public int Cost => database.objectsData[index].Prize;

    public FloorBuildBehaviour(
        int objectIndex,
        Grid grid,
        GridData floorData,
        ObjectsDatabaseSO database,
        ObjectPlacer placer,
        PreviewSystem preview)
    {
        index = objectIndex;
        this.grid = grid;
        this.floorData = floorData;
        this.database = database;
        this.placer = placer;
        this.preview = preview;
    }

    public bool CanPlace(Vector3Int position)
    {
        return floorData.CanPlaceObjectAt(position, database.objectsData[index].Size);
    }

    public void Place(Vector3Int position)
    {
        int placedIndex = placer.PlaceObject(
            database.objectsData[index].Prefab,
            grid.CellToWorld(position),
            database.objectsData[index].EnergyProduction,
            null);

        floorData.AddObjectAt(position,
            database.objectsData[index].Size,
            database.objectsData[index].ID,
            placedIndex);
    }

    public void UpdatePreview(Vector3Int position)
    {
        preview.UpdatePosition(grid.CellToWorld(position), CanPlace(position));
    }
}
