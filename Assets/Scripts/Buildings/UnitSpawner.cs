using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] Health health = null;
    [SerializeField] Unit unitPrefab = null;
    [SerializeField] Transform spawnPoint = null;

    [SerializeField] TMP_Text remainingUnitsText = null;
    [SerializeField] Image unitProgressImage = null;

    [SerializeField] int maxUnitQueue = 5;
    [SerializeField] float spawnMoveRange = 7f;
    [SerializeField] float unitSpawnDuration = 5f;

    [SyncVar]
    private int queuedUnits;
    [SyncVar]
    private float unitTimer;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }


    [Command]
    private void CmdSpawnUnit()
    {
        GameObject instance = Instantiate(unitPrefab, spawnPoint.position, spawnPoint.rotation);

        // spawns the instance across the network and gives authority to the client that spawned it
        NetworkServer.Spawn(instance, connectionToClient); 
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public void OnPointerClick(PointerEventData eventData)
    {
        DrawLine();

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (!hasAuthority) return;

        CmdSpawnUnit();
    }

    #endregion

    /********** MARK: Debug **********/
    #region Debug

    private void DrawLine()
    {
        Camera mainCamera = FindObjectOfType<Camera>(); // "there can be only one"
        Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.white, 3f);
        }
    }

    #endregion
}
