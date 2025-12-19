/* DisconnectedComponentState
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase que se encarga de la gestión de un componente que está desconectado. Hereda de ComponentState.
 * 
 * v1 -10/12/2025- creación del estado.
 */
using UnityEngine;

public class DisconnectedComponentState : ComponentState
{
    private ResourceManagement resourceManagement;

    public DisconnectedComponentState(ResourceManagement resourceManagement)
    {
        this.resourceManagement = resourceManagement;
    }

    public override void EnterState()
    {
        Debug.Log("Enter disconnect state");
    }

    public override void EndState()
    {
        Debug.Log("End disconnect state");
    }
}
