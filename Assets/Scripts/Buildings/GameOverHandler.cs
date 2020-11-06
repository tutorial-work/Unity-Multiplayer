using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameOverHandler : NetworkBehaviour
{
    /********** MARK: Class Variables **********/
    #region Class Variables

    public static event Action ServerOnGameOver;
        
    public static event Action<string> ClientOnGameOver;

    List<UnitBase> bases = new List<UnitBase>(); 

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawned += ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDespawned += ServerHandleBaseDespawned;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDespawned -= ServerHandleBaseDespawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        bases.Add(unitBase);
    }

    [Server]
    private void ServerHandleBaseDespawned(UnitBase unitBase)
    {
        bases.Remove(unitBase);

        if (bases.Count != 1) return;

        int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver($"Player {playerId} Wins!");

        ServerOnGameOver?.Invoke();
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    [ClientRpc]
    private void RpcGameOver(string winner)
    {
        //Debug.Log("Game Over");
        ClientOnGameOver?.Invoke(winner);
    }

    #endregion
}
