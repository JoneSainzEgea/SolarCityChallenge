/* SolarPanelComponent
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase que se encarga del funcionamento del componente del panel solar. Hereda BuildingComponent.
 * 
 * v1 -10/12/2025- OnPlacement, CheckDependencies.
 */

using UnityEngine;

public class SolarPanelComponent : BuildingComponent
{
    public override void OnPlacement(float energyProduction, ResourceManagement resManager)
    {
        base.OnPlacement(energyProduction, resManager);
    }

    public override bool CheckDependencies(BuildingComponent connectedComponent)
    {
        // TODO
        Debug.Log($"Intentando conectar {this} y {connectedComponent}");
        return true;
    }
}
