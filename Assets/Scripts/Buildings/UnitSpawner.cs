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
    [SerializeField] Transform unitSpawnPoint = null;

    [SerializeField] TMP_Text remainingUnitsText = null;
    [SerializeField] Image unitProgressImage = null;

    [SerializeField] int maxUnitQueue = 5;
    [SerializeField] float spawnMoveRange = 7f;
    [SerializeField] float unitSpawnDuration = 5f;

    [SyncVar(hook = nameof(ClientHandleQueuedUnitsUpdated))]
    int queuedUnits;
    [SyncVar]
    float unitTimer;

    float progressImageVelocity;

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void Update()
    {
        if (isServer)
        {
            ServerProduceUnits();
        }

        if (isClient)
        {
            UpdateTimerDisplay();
        }
    }

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
    private void ServerProduceUnits()
    {
        if (queuedUnits == 0) return;

        unitTimer += Time.deltaTime;

        if (unitTimer < unitSpawnDuration) return;

        GameObject instance = Instantiate(unitPrefab.gameObject, unitSpawnPoint.position, unitSpawnPoint.rotation);

        // spawns the instance across the network and gives authority to the client that spawned it
        NetworkServer.Spawn(instance, connectionToClient);

        Vector3 spawnOffset = Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnPoint.position.y;

        UnitMovement unitMovement = instance.GetComponent<UnitMovement>();
        unitMovement.ServerMove(unitSpawnPoint.position + spawnOffset);

        queuedUnits--;
        unitTimer = 0;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        if (queuedUnits == maxUnitQueue) return;

        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        if (player.CurrentResources < unitPrefab.ResourceCost) return;

        queuedUnits++;

        //player.Resources = player.Resources - unitPrefab.ResourceCost;
        player.CurrentResources -= unitPrefab.ResourceCost; // TODO: can you actually do this with getters and setters?
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    private void UpdateTimerDisplay()
    {
        float newProgress = unitTimer / unitSpawnDuration;

        if (newProgress < unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgress;
        }
        else
        {
            unitProgressImage.fillAmount = Mathf.SmoothDamp(
                unitProgressImage.fillAmount,
                newProgress,
                ref progressImageVelocity,
                0.1f
           );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DrawLine();

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (!hasAuthority) return;

        CmdSpawnUnit();
    }

    private void ClientHandleQueuedUnitsUpdated(int oldQueuedUnits, int newQueuedUnits)
    {
        remainingUnitsText.text = $"{newQueuedUnits}";
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
