/* DefaultBuildBehaviourSO
 * Jone Sainz Egea
 * 20/01/2026
 *
 * ScriptableObject que define el comportamiento de construcción genérica de elementos. Hereda de BuildBehaviourSO.
 * Inicia la previsualización del elemento a construir.
 * Al hacer click comprueba que haya dinero para construir el elemento y que se pueda colocar.
 * Las condiciones para poder colocarlo son: que no haya otro mueble, que haya suelo.
 * Añade los datos de colocación y actualiza recursos y previsualización.
 * 
 * v1 -20/01/2026- previsualización, comprobaciones, colocación y actualización de previsualización, recursos y datos.
 */
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Behaviours/Default")]
public class DefaultBuildBehaviourSO : BuildBehaviourSO
{
    private Vector2Int size;
    private Vector3Int pos;
    private GameObject prefab;
    private Grid grid;
    private GridDataManager gridData;
    private ResourceManagement resourceManagement;
    private PreviewSystem preview;
    private int prize;
    public override void StartPreview(PreviewSystem preview, GameObject prefab, Vector2Int size, Grid grid, GridDataManager gridData)
    {
        this.preview = preview;
        this.prefab = prefab;
        this.size = size;
        this.grid = grid;
        this.gridData = gridData;

        preview.StartShowingPlacementPreview(prefab, size);
    }

    public override bool CanPlace(Vector3Int pos)
    {
        this.pos = pos;

        // Has floor and doesn't have furniture
        GridData floorData = gridData.GetGridData(GridDataType.FloorData);
        GridData furnitureData = gridData.GetGridData(GridDataType.FurnitureData);
        return !floorData.CanPlaceObjectAt(pos, size) && furnitureData.CanPlaceObjectAt(pos, size);
    }

    public override bool HasMoney(ResourceManagement resourceManagement, int prize)
    {
        this.resourceManagement = resourceManagement;
        this.prize = prize;

        return (resourceManagement.GetResourceAmount(ResourceType.Money) >= prize);
    }

    public override void Place(ObjectPlacer placer, int ID, int energyProduction)
    {
        int index = placer.PlaceObject(prefab,grid.CellToWorld(pos), energyProduction, resourceManagement);

        GridData furnitureData = gridData.GetGridData(GridDataType.FurnitureData);
        furnitureData.AddObjectAt(pos,size, ID, index);
    }

    public override void RemoveResources()
    {
        resourceManagement.RemoveResource(ResourceType.Money, prize);
    }

    public override void UpdatePreview(Vector3Int gridPosition)
    {
        preview.UpdatePosition(grid.CellToWorld(gridPosition), CanPlace(gridPosition));
    }
}
