using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.EventSystems;

public class Minimap : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] RectTransform minimapRect = null;
    [SerializeField] float mapScale = 20f;
    [SerializeField] float offset = -6f;

    Transform playerCameraTransform = null;

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveCamera();
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveCamera();
    }

    private void Update()
    {
        if (playerCameraTransform != null) return;

        if (NetworkClient.connection == null) return;
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        if (networkIdentity == null) return;

        playerCameraTransform = networkIdentity.GetComponent<RTSPlayer>().CameraTransform;
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void MoveCamera()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimapRect,
            mousePos,
            null,
            out Vector2 localPoint
        )) return;

        Vector2 lerp = new Vector2(
            (localPoint.x - minimapRect.rect.x) / minimapRect.rect.width,
            (localPoint.y - minimapRect.rect.y) / minimapRect.rect.height
        );

        Vector3 newCameraPos = new Vector3(
            Mathf.Lerp(-mapScale, mapScale, lerp.x),
            playerCameraTransform.position.y,
            Mathf.Lerp(-mapScale, mapScale, lerp.y)
        );

        playerCameraTransform.position = newCameraPos + new Vector3(0, 0, offset);
    }

    #endregion
}
