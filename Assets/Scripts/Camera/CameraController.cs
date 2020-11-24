using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] Transform playerCameraTransform = null;
    [SerializeField] float speed = 25f;
    [SerializeField] float screenBorderThickness = 10f;
    [SerializeField] Vector2 screenXLimits = Vector2.zero;
    [SerializeField] Vector2 screenZLimits = Vector2.zero;

    Vector2 previousInput;

    Controls controls;

    Vector3 resetPosition;

    bool isCameraResetPositionInitialized = false;

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);

        controls = new Controls();

        controls.Player.MoveCamera.performed += SetPreviousInput;
        controls.Player.MoveCamera.canceled += SetPreviousInput;

        controls.Player.ResetCamera.performed += ResetCameraPosition;

        controls.Enable();

        UnitBase.AuthorityOnBaseSpawned += InitializeCameraResetPosition; 
    }

    public override void OnStopClient()
    {
        if (hasAuthority) UnitBase.AuthorityOnBaseSpawned -= InitializeCameraResetPosition;
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority || !Application.isFocused) return;

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 pos = playerCameraTransform.position;

        if (previousInput == Vector2.zero)
        {
            Vector3 cursorMovement = Vector3.zero;

            Vector2 cursorPosition = Mouse.current.position.ReadValue();

            if (cursorPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.z += 1;
            }
            else if (cursorPosition.y <= screenBorderThickness)
            {
                cursorMovement.z -= 1;
            }

            if (cursorPosition.x >= Screen.width - screenBorderThickness)
            {
                cursorMovement.x += 1;
            }
            else if (cursorPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }

            pos += cursorMovement.normalized * speed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, 0f, previousInput.y) * speed * Time.deltaTime;
        }

        // these x & y's are really the min and maxes, we just stored them as a Vector2
        pos.x = Mathf.Clamp(pos.x, screenXLimits.x, screenXLimits.y); 
        pos.z = Mathf.Clamp(pos.z, screenZLimits.x, screenZLimits.y);

        playerCameraTransform.position = pos;
    }

    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }

    private void InitializeCameraResetPosition(UnitBase unitBase)
    {
        float cameraHeight = playerCameraTransform.position.y;
        resetPosition = unitBase.transform.position;
        resetPosition.z -= 10f; // HACK: hardcoded offset
        resetPosition.y = cameraHeight;

        playerCameraTransform.position = resetPosition;

        isCameraResetPositionInitialized = true;
    }

    private void ResetCameraPosition(InputAction.CallbackContext ctx)
    {
        if (isCameraResetPositionInitialized) playerCameraTransform.position = resetPosition;
    }

    #endregion
}
