// Jone Sainz Egea
// 21/04/2026

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SolarModule : MonoBehaviour
{
    private Renderer rend;

    [Header("Debug")]
    public bool isInShadow;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void CheckShadow(Vector3 sunDirection, float maxDistance, LayerMask mask)
    {
        Ray ray = new Ray(transform.position, sunDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, mask))
        {
            isInShadow = true;
            SetColor(Color.red);

            Debug.DrawRay(transform.position, sunDirection * hit.distance, Color.red);
        }
        else
        {
            isInShadow = false;
            SetColor(Color.green);

            Debug.DrawRay(transform.position, sunDirection * maxDistance, Color.green);
        }
    }

    void SetColor(Color color)
    {
        rend.material.color = color;
    }
}
