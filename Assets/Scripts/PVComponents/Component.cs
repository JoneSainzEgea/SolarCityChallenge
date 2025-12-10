/* Component
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase abstracta que define cómo serán los componentes que se pueden situar.
 * Métodos OnPlacement, OnConnection, CheckDependencies y OnRemoval.
 * 
 * v1 -10/12/2025- OnPlacement, OnConnection, CheckDependencies y OnRemoval.
 */

using UnityEngine;

public abstract class Component : MonoBehaviour
{
    public ComponentState componentState;
    public float energyProduction;
    public ResourceManagement resourceManagement;

    public virtual void OnPlacement(float energyProd, ResourceManagement resManager)
    {
        energyProduction = energyProd;
        resourceManagement = resManager;
        componentState = new DisconnectedComponentState(resourceManagement);
        componentState.EnterState();
    }

    public virtual void OnConnection()
    {
        if (CheckDependencies())
        {
            componentState.EndState();
            componentState = new ConnectedComponentState(energyProduction, resourceManagement);
            componentState.EnterState();
        }
    }

    public abstract bool CheckDependencies();

    //private void Update()
    //{
    //    componentState.Update();
    //}
    public virtual void OnRemoval()
    {
        componentState.EndState();
    }
}
