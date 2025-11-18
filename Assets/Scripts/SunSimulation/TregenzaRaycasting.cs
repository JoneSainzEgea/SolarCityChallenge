using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TregenzaRaycasting : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private LayerMask layerMask;

    [Header("Debugging")]
    [SerializeField] private float diskRadius = 5f;
    [SerializeField] private bool drawLines = true;
    [SerializeField] private bool drawDisks = true;

    private List<Patch> tregenzaPatches = new List<Patch>(145);
    private List <Vector3> rayDirections = new List<Vector3>(145);
    private List<bool> raycastHit = new List<bool>(145);

    private void Start()
    {
        UpdatePatches();
    }

    public void UpdatePatches()
    {
        GetDirections();
        CastRays();
    }

    private void GetDirections()
    {
        rayDirections.Clear();
        raycastHit.Clear();

        tregenzaPatches = TregenzaSky.GenertePatches();

        for (int i = 0; i < tregenzaPatches.Count; ++i)
        {
            float elevationDeg = tregenzaPatches[i].Elevation;
            float azimtuhDeg = tregenzaPatches[i].Azimuth;

            float elev = elevationDeg * Mathf.Deg2Rad;
            float az = azimtuhDeg * Mathf.Deg2Rad;

            float sinElev = Mathf.Sin(elev);
            float cosElev = Mathf.Cos(elev);
            float sinAz = Mathf.Sin(az);
            float cosAz = Mathf.Cos(az);

            float x = cosElev * sinAz;      // norte = +x
            float y = sinElev;              // arriba = +y
            float z = cosElev * -cosAz;     // este = -z

            Vector3 direction = new Vector3(x, y, z).normalized;

            rayDirections.Add(direction);
            raycastHit.Add(false);
        }
    }

    private void CastRays()
    {
        for (int i = 0; i < rayDirections.Count; i++)
        {
            raycastHit[i] = Physics.Raycast(transform.position, rayDirections[i], out RaycastHit hit, rayDistance, layerMask);
        }
    }


    #region Debugging
    private void OnDrawGizmos()
    {
        if (rayDirections == null || rayDirections.Count == 0)
            return;

        for (int i = 0; i < rayDirections.Count; i++)
        {
            Color c = raycastHit[i] ? Color.red : Color.green;
            Gizmos.color = c;

            Vector3 endPoint = transform.position + rayDirections[i] * rayDistance;
            
            if(drawLines)
                Gizmos.DrawLine(transform.position, endPoint);

            if(drawDisks)
                DrawDisk(endPoint, rayDirections[i], diskRadius, c, 24);
        }
    }

    private void DrawDisk(Vector3 center, Vector3 normal, float radius, Color color, int segments = 24)
    {
        Gizmos.color = color;

        Vector3 v1 = Vector3.Cross(normal, Vector3.up);
        if (v1.sqrMagnitude < 0.001f)
            v1 = Vector3.Cross(normal, Vector3.right);

        v1.Normalize();
        Vector3 v2 = Vector3.Cross(normal, v1);

        float angleStep = 360f / segments;

        Vector3 prevPoint = center + v1 * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + (v1 * Mathf.Cos(angle) + v2 * Mathf.Sin(angle)) * radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
    #endregion
}
