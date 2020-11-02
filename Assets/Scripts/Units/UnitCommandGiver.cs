using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{

    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] LayerMask layerMask = new LayerMask();

    [SerializeField] UnitSelectionHandler unitSelectionHandler = null;

    Camera mainCamera;
    
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
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }
            TryTarget(target);
            return;
        }

        TryMove(hit.point);
    }

    private void TryMove(Vector3 position)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.Movement.CmdMove(position);
        }
    }

    private void TryTarget(Targetable target)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.Targeter.CmdSetTarget(target.gameObject);
        }
    }

    #endregion
}
