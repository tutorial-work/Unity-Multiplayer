using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] NavMeshAgent agent = null;

    Camera mainCamera;

    float speed = 0f;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [Command]
    private void CmdMove(Vector3 position)
    {
        float leeway = 1f; // distance away from navmesh that still counts

        // exit if invalid position
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, leeway, NavMesh.AllAreas))
            return;

        agent.SetDestination(position);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Unity Method; Update() is called once per frame
    /// </summary>
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;

        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;

        CmdMove(hit.point);
    }

    #endregion
}
