using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] Building building = null;
    [SerializeField] Image iconImage = null;
    [SerializeField] TMP_Text priceText = null;
    [SerializeField] LayerMask floorMask = new LayerMask();

    private Camera mainCamera;
    private RTSPlayer player;
    private GameObject buildingPreviewInstance;
    private Renderer buildingRendererInstance;

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    /// <summary>
    /// Unity Method; Start() is called before the first frame update
    /// </summary>
    private void Start()
    {
        mainCamera = Camera.main;

        iconImage.sprite = building.Icon;

        priceText.text = building.Price.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        buildingPreviewInstance = Instantiate(building.BuildingPreview);
        buildingRendererInstance = buildingPreviewInstance.GetComponentInChildren<Renderer>();

        buildingPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buildingPreviewInstance == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            player.CmdTryPlaceBuilding(building.Id, hit.point);
        }

        Destroy(buildingPreviewInstance);
    }

    /// <summary>
    /// Unity Method; Update() is called once per frame
    /// </summary>
    private void Update()
    {
        bool isSet = TempSetPlayer(); // delete this after lobby is created
        if (!isSet) return;

        if (buildingPreviewInstance == null) return;

        UpdateBuildingPreview();
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private bool TempSetPlayer()
    {
        if (player == null)
        {
            if (NetworkClient.connection == null) return false;
            if (NetworkClient.connection.identity == null) return false;

            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

            Debug.Log("Setting Player:" + player.name);
        }

        return true;
    }

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) return;

        buildingPreviewInstance.transform.position = hit.point;

        if (!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(true);
        }
    }

    #endregion
}
