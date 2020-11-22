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

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void Awake()
    {
        RTSPlayerInfo info = NetworkClient.connection.identity.GetComponent<RTSPlayerInfo>();

        playerSteamImage.texture = info.DisplayTexture;
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;

        ServerOnBaseSpawned?.Invoke(this);
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
