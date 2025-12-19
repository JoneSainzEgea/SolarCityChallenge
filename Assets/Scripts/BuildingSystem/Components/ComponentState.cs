/* ComponentState
 * Jone Sainz Egea
 * 10/12/2025
 * 
 * Clase abstracta que define cómo serán los estados de los componentes.
 * Métodos EnterState, Update y EndState.
 * 
 * v1 -10/12/2025- EnterState,  Update y EndState.
 */

public abstract class ComponentState
{
    public abstract void EnterState();
    public virtual void Update() {}
    public abstract void EndState();
}
