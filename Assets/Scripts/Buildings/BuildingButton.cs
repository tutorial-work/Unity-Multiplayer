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

    Camera mainCamera;
    BoxCollider buildingCollider;
    RTSPlayer player;
    GameObject buildingPreviewInstance;
    Renderer buildingRendererInstance;

    bool canAffordBuilding = true;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    private bool CanAffordBuilding
    {
        get
        {
            return canAffordBuilding;
        }
        set
        {
            if (canAffordBuilding == value) return;

            iconImage.color = (value) ? new Color(1f, 1f, 1f) : new Color(0.25f, 0.25f, 0.25f);
            canAffordBuilding = value;
        }
    }

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

        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        buildingCollider = building.GetComponent<BoxCollider>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (player.CurrentResources < building.Price) return;

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
        CanAffordBuilding = (building.Price <= player.CurrentResources);

        if (buildingPreviewInstance == null) return;

        UpdateBuildingPreview();
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) return;

        buildingPreviewInstance.transform.position = hit.point;

        if (!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(true);
        }

        Color color = player.CanPlaceBuilding(buildingCollider, hit.point) ? Color.green : Color.red;

        //foreach(Renderer renderer in buildingRendererInstance.GetComponents<Renderer>())
        buildingRendererInstance.material.SetColor("_BaseColor", color);
    }

    #endregion
}