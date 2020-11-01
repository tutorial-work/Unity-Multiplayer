using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    List<Unit> myUnits = new List<Unit>();

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public List<Unit> MyUnits
    {
        get
        {
            return myUnits;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Remove(unit);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartClient()
    {
        // if this is the server, return... i.e. add unit to list only if client
        if (!isClientOnly) return; 

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        // if this is the server, return... i.e. add unit to list only if client
        if (!isClientOnly) return;

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        // only add a unit to your list if you own the player and the unit
        if (!hasAuthority) return;

        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        // only add a unit to your list if you own the player and the unit
        if (!hasAuthority) return;

        myUnits.Remove(unit);
    }

    #endregion
}
