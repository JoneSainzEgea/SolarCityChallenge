using System.Collections;
using System.Collections.Generic;
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
        Line.startWidth = 0.1f;
        Line.endWidth = 0.1f;
        Line.useWorldSpace = true;

        Line.enabled = true;
        Line.SetPosition(0, pos1);
    }

    public void Update(Vector3 pos2)
    {
        Line.SetPosition(1, pos2);
    }

    public void Show() => Line.enabled = true;
    public void Hide() => Line.enabled = false;
}
