using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] RectTransform unitSelectionArea = null;

    [SerializeField] LayerMask layerMask = new LayerMask();

    Vector2 startPosition;

    RTSPlayer player;
    Camera mainCamera;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public List<Unit> SelectedUnits { get; } = new List<Unit>();

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    /// <summary>
    /// Unity Method; Start() is called before the first frame update
    /// </summary>
    protected void Start()
    {
        mainCamera = Camera.main;

        //player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        TempSetPlayer();
    }

    /// <summary>
    /// Unity Method; Update() is called once per frame
    /// </summary>
    protected void Update()
    {
        bool isSet = TempSetPlayer(); // delete this after lobby is created
        if (!isSet) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Start selection area; deselects units
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }
        
        unitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

            if (!unit.hasAuthority) return;

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }

            return;
        }
         
        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (Unit unit in player.MyUnits)
        {
            if (SelectedUnits.Contains(unit)) continue;

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x &&
                screenPosition.x < max.x &&
                screenPosition.y > min.y &&
                screenPosition.y < max.y)
            {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = 
            startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    #endregion

    /********** MARK: Debug **********/
    #region Debug

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

    #endregion
}
