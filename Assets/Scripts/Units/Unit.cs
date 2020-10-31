using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] UnitMovement unitMovement = null;
    [SerializeField] UnityEvent onSelected = null;
    [SerializeField] UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public UnitMovement Movement
    {
        get
        {
            return unitMovement;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) return;

        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) return;

        AuthorityOnUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        onSelected?.Invoke(); // ? mark checks if event is null "it's a safety check"
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;

        onDeselected?.Invoke(); // ? mark checks if event is null "it's a safety check"
    }

    #endregion

}
