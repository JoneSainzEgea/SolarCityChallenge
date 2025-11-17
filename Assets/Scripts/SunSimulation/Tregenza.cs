using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tregenza : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debug")]
    [SerializeField] private bool hitSomething;
    [SerializeField] private bool hitSomething1;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, rayDistance, layerMask))
        {
            hitSomething = true;
        }
        else
        {
            hitSomething = false;
        }
        if (Physics.Raycast(transform.position, new Vector3(0.5f, 0.5f, 0), out RaycastHit hit2, rayDistance, layerMask))
        {
            hitSomething1 = true;
        }
        else
        {
            hitSomething1 = false;
        }
    }

    // Dibuja el rayo en la SceneView para depuración
    private void OnDrawGizmos()
    {
        Gizmos.color = hitSomething ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * rayDistance);
        Gizmos.color = hitSomething1 ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3 (0.5f,0.5f,0) * rayDistance);
    }
}
