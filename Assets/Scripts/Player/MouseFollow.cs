using System;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private InputSystem_Actions inputSystem;

    private void Awake() {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable() {
        inputSystem.Enable();
    }
    private void OnDisable() {
        inputSystem.Disable();
    }

    private void Update() {
        FaceMouse();
    }

    private void FaceMouse() {
        Vector2 mousePosition = inputSystem.UI.Point.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (Vector2)transform.position - mousePosition;
        transform.right = -direction;
    }
}
