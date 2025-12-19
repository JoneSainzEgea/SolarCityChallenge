/* ConnectedComponentState
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase que se encarga de la gestión de un componente que está conectado. Hereda de ComponentState.
 * Se encarga de modificar el recusro de producción de energía.
 * 
 * v1 -10/12/2025- modifica el recurso de producción de energía al entrar y salir del estado.
 */
using UnityEngine;

public class ConnectedComponentState : ComponentState
{
    private float energyProduction;
    private ResourceManagement resourceManagement;

    public ConnectedComponentState(float energyProduction, ResourceManagement resourceManagement)
    {
        this.energyProduction = energyProduction;
        this.resourceManagement = resourceManagement;
    }
    public override void EnterState()
    {
        Debug.Log("Enter connect state");
        resourceManagement.AddResource(ResourceType.Energy, energyProduction);
    }

    public override void EndState()
    {
        resourceManagement.RemoveResource(ResourceType.Energy, energyProduction);
        Debug.Log("End connect state");
    }
}
