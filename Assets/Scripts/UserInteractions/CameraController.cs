// Jone Sainz Egea
// 18/04/2026
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float minPitch = 5f;   // límite inferior (cúpula)
    [SerializeField] private float maxPitch = 80f;   // límite superior

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 30f;

    [Header("Keyboard")]
    [SerializeField] private float keyboardRotationSpeed = 60f;
    [SerializeField] private float keyboardZoomSpeed = 20f;

    [Header("Terrain Interaction")]
    [SerializeField] private float minCamTerrainHeight = 0.5f;

    private float correctedDistance;

    private float yaw = 0f;
    private float pitch = 30f;
    private float distance = 15f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
        HandleZoom();

        correctedDistance = distance;

        AdjustCameraToTerrain(ref correctedDistance);

        UpdateCameraPosition(correctedDistance);
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            int direction = Input.GetMouseButton(0) ? 1 : -1;

            yaw += mouseX * rotationSpeed * direction;
            pitch -= mouseY * rotationSpeed * direction;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private void HandleKeyboardInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        yaw -= horizontal * keyboardRotationSpeed * Time.deltaTime;
        pitch += vertical * keyboardRotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (Input.GetKey(KeyCode.Q))
            distance -= keyboardZoomSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            distance += keyboardZoomSpeed * Time.deltaTime;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            distance -= scroll * zoomSpeed * 100f * Time.deltaTime;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    private void UpdateCameraPosition(float currentDistance)
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 direction = rotation * Vector3.forward;

        transform.position = target.position - direction * currentDistance;
        transform.LookAt(target.position);
    }

    private void AdjustCameraToTerrain(ref float correctedDistance)
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.forward;

        Vector3 desiredPosition = target.position - direction * correctedDistance;

        RaycastHit hit;

        if (Physics.SphereCast(target.position, 0.5f, -direction, out hit, correctedDistance))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                correctedDistance = hit.distance - minCamTerrainHeight;
            }
        }
    }
}
