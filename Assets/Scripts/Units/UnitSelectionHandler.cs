using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] LayerMask layerMask = new LayerMask();

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
    }

    /// <summary>
    /// Unity Method; Update() is called once per frame
    /// </summary>
    protected void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Start selection area
            Deselect();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if (!unit.hasAuthority) return;

        SelectedUnits.Add(unit);

        foreach(Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
    }

    private void Deselect()
    {
        foreach (Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Deselect();
        }

        SelectedUnits.Clear();
    }

    #endregion
}
