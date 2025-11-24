using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float acceleration = 5f;

    [Header("Rotación")]
    public float mouseSensitivity = 3f;
    public bool invertY = false;

    [Header("Cursor")]
    public KeyCode unlockCursorKey = KeyCode.Space;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        HandleCursor();
        if (Cursor.lockState == CursorLockMode.Locked)
            HandleRotation();
        HandleMovement();
    }

    private void HandleCursor()
    {
        if (Input.GetKeyDown(unlockCursorKey))
            LockCursor(Cursor.lockState == CursorLockMode.None);
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1 : -1);

        yaw += mouseX;
        pitch += mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Subir y bajar
        if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
        if (Input.GetKey(KeyCode.E)) input.y += 1f;

        // Velocidad y sprint
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        // Transformar a espacio mundial
        Vector3 desiredMove = transform.TransformDirection(input) * speed;

        // Suavizar movimiento
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + desiredMove, ref currentVelocity, 1f / acceleration);
    }
}
