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

    Controls controls;

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

        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>(); // TODO: just breath this line of code in

        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;

        controls = new Controls();

        controls.Player.SelectAllUnits.performed += SelectAllUnits;

        controls.Enable();
    }

    private void OnDestroy()
    {
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    /// <summary>
    /// Unity Method; Update() is called once per frame
    /// </summary>
    protected void Update()
    {
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
    
    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        SelectedUnits.Remove(unit);
    }

    private void ClientHandleGameOver(string winnerName)
    {
        enabled = false;
    }

    private void SelectAllUnits(InputAction.CallbackContext ctx)
    {
        SelectedUnits.Clear();
        foreach (Unit unit in player.MyUnits)
        {
            SelectedUnits.Add(unit);
            unit.Select();
        }
    }
    
    #endregion
}
