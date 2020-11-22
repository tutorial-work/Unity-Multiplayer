using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;

public class UnitBase : NetworkBehaviour
{
    /********** MARK: Class Variables **********/
    #region Class Variables

    [SerializeField] RawImage playerSteamImage = null; // HACK: hardcoded

    [SerializeField] Health health = null;

    public static event Action<int> ServerOnPlayerDie;
    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDespawned;


    #endregion

    /********** MARK: Properties **********/
    #region Properties

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;

        ServerOnBaseSpawned?.Invoke(this);

        // i have no idea what im doing

        if (!hasAuthority) return;

        RTSNetworkManager manager = (RTSNetworkManager)NetworkManager.singleton;

        int playerId = connectionToClient.connectionId;

        RTSPlayerInfo info = manager.Players[playerId].GetComponent<RTSPlayerInfo>();

        playerSteamImage.texture = info.DisplayTexture;
    }

    public override void OnStopServer()
    {
        ServerOnBaseDespawned?.Invoke(this);

        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId);

        NetworkServer.Destroy(gameObject);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
