/* BatteryComponent
 * Jone Sainz Egea
 * 11/12/2025
 * 
 * Clase que se encarga del funcionamento del componente de la batería. Hereda SolarComponent.
 * 
 * v1 -11/12/2025- OnPlacement, CheckDependencies.
 */
using UnityEngine;

public class BatteryComponent : SolarComponent
{
    // Necesita conexíón con: panel solar
    private bool connectedToSolarPanel = false;
    private bool connectedToSystem = false;
    public override void OnPlacement(float energyProduction, ResourceManagement resManager)
    {
        base.OnPlacement(energyProduction, resManager);
    }

    public override bool CheckDependencies(SolarComponent connectedComponent)
    {
        // TODO
        Debug.Log($"Intentando conectar {this} y {connectedComponent}");

        if(connectedComponent is SolarPanelComponent)
        {
            connectedToSolarPanel = true;
        }
        connectedToSystem = true;
        if(connectedToSolarPanel && connectedToSystem)
            return true;
        return false;
    }
}
