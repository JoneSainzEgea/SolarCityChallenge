using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayerMask;
    public event Action OnMousePressed, OnMouseReleased, OnCancel, OnUndo;
    public event Action<int> OnRotate;
    public event Action<bool> OnToggleDelete;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if(Physics.Raycast(ray, out hit, 100, placementLayerMask))
            lastPosition = hit.point;

        return lastPosition;
    }

    public bool IsInteractingWithUI() => EventSystem.current.IsPointerOverGameObject();

    public bool GetPlacementInput() => Input.GetMouseButtonDown(0);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel?.Invoke();
        }
    }
}
