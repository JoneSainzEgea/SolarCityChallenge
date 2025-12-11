/* BatteryComponent
 * Jone Sainz Egea
 * 11/12/2025
 * 
 * Clase que se encarga del funcionamento del componente de la batería. Hereda BuildingComponent.
 * 
 * v1 -11/12/2025- OnPlacement, CheckDependencies.
 */
using UnityEngine;

public class BatteryComponent : BuildingComponent
{
    // Necesita conexíón con: panel solar
    private bool connectedToSolarPanel = false;
    public override void OnPlacement(float energyProduction, ResourceManagement resManager)
    {
        base.OnPlacement(energyProduction, resManager);
    }

    public override bool CheckDependencies(BuildingComponent connectedComponent)
    {
        // TODO
        Debug.Log($"Intentando conectar {this} y {connectedComponent}");

        if(connectedComponent is SolarPanelComponent)
        {
            connectedToSolarPanel = true;
        }
        if(connectedToSolarPanel)
            return true;
        return false;
    }
}
