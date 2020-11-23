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

    [Command]
    public void CmdSetPlayerSteamImage()
    {
        RTSNetworkManager manager = (RTSNetworkManager)NetworkManager.singleton;

        List<RTSPlayer> players = manager.Players;

        foreach (RTSPlayer player in players)
        {
            RTSPlayerInfo playerInfo = player.GetComponent<RTSPlayerInfo>();

            foreach (Building building in player.MyBuildings)
            {
                if (building.GetComponent<UnitBase>() == this)
                {
                    //playerSteamImage.texture = playerInfo.DisplayTexture;
                }
            }
        }
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions
        

    #endregion
}
