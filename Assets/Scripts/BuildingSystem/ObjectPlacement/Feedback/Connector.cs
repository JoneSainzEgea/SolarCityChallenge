/* Connector
 * Jone Sainz Egea
 * 19/12/2025
 * 
 * Clase que se encarga de la visualización de la conexión entre elementos.
 * 
 * v1 -19/12/2025- crea un nuevo objeto con un line renderer cada vez que se llama al constructor.
 * 
 * TODO: cambiar el método de visualización para no crear tantos game objects.
 */
using UnityEngine;

public class Connector
{
    public LineRenderer Line { get; private set; }
    private GameObject lineGameObject;

    public Connector(Vector3 pos1)
    {
        lineGameObject = new GameObject("Connection");
        Line = lineGameObject.AddComponent<LineRenderer>();

        Line.positionCount = 2;
        Line.material = new Material(Shader.Find("Sprites/Default"));
        Line.startColor = Color.red;
        Line.endColor = Color.red;
        Line.startWidth = 0.1f;
        Line.endWidth = 0.1f;
        Line.useWorldSpace = true;

        Line.enabled = true;
        pos1 += Vector3.up;
        Line.SetPosition(0, pos1);
    }

    public void Update(Vector3 pos2)
    {
        pos2 += Vector3.up;
        Line.SetPosition(1, pos2);
    }

    public void Show() => Line.enabled = true;
    public void Hide() => Line.enabled = false;
}
