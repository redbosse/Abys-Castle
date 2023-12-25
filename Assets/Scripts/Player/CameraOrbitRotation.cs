using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbitRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeedHorizontal = 10;

    [SerializeField]
    private float rotationSpeedVertical = 1;

    [SerializeField]
    private Transform cameraTransform;

    private float pi = (Mathf.PI / 360);

    [SerializeField]
    private float horizontalRotate = 0;

    [SerializeField]
    private float verticalRotate = 0;

    private bool isGrabState = false;

    public bool IsGrabState { get => isGrabState; set => isGrabState = value; }

    private void Start()
    {
        if (cameraTransform is null)
        {
            throw new System.Exception("CameraTransform is null");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MouseDelta(InputAction.CallbackContext context)
    {
        if (isGrabState == false)
            transform.Rotate(Vector3.up, context.ReadValue<Vector2>().x * rotationSpeedHorizontal * Time.deltaTime);

        horizontalRotate = Mathf.Clamp(horizontalRotate + context.ReadValue<Vector2>().x * Time.deltaTime * rotationSpeedHorizontal / 2f, -10, 10);
        verticalRotate = Mathf.Clamp(verticalRotate + context.ReadValue<Vector2>().y * Time.deltaTime * rotationSpeedVertical, -40, 20);

        cameraTransform.localRotation = new Quaternion(-verticalRotate * pi, horizontalRotate * pi, cameraTransform.localRotation.z, cameraTransform.localRotation.w);
    }
}