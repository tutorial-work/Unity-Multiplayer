using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using Steamworks;

public class UnitBase : NetworkBehaviour
{
    /********** MARK: Class Variables **********/
    #region Class Variables

    [SerializeField] RawImage playerSteamImage = null; // HACK: hardcoded

    [SerializeField] Health health = null;

    [SerializeField] Transform unitSpawnPoint = null;

    public static event Action<int> ServerOnPlayerDie;
    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDespawned;
    
    [SyncVar(hook = nameof(ClientHandleSteamIdUpdated))]
    ulong steamId;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public Transform SpawnPoint
    {
        get
        {
            return unitSpawnPoint;
        }
    }

    public ulong SteamId
    {
        get
        {
            return steamId;
        }

        [Server]
        set
        {
            steamId = value;
        }
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
        
    public void SetPlayerSteamImage(Texture2D displayTexture)
    {
        playerSteamImage.texture = displayTexture;
    }

    private void ClientHandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        CSteamID steamId = new CSteamID(newSteamId);

        int imageId = SteamFriends.GetLargeFriendAvatar(steamId);

        if (imageId == -1) return;

        SetPlayerSteamImage(RTSPlayerInfo.GetSteamImageAsTexture(imageId));
    }

    #endregion
}
