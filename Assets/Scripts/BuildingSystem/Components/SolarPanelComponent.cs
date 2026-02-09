/* SolarPanelComponent
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase que se encarga del funcionamento del componente del panel solar. Hereda SolarComponent.
 * 
 * v1 -10/12/2025- OnPlacement, CheckDependencies.
 */

using UnityEngine;

public class SolarPanelComponent : SolarComponent
{
    public override void OnPlacement(float energyProduction, ResourceManagement resManager)
    {
        base.OnPlacement(energyProduction, resManager);
    }

    public override bool CheckDependencies(SolarComponent connectedComponent)
    {
        // TODO
        Debug.Log($"Intentando conectar {this} y {connectedComponent}");
        return true;
    }
}
