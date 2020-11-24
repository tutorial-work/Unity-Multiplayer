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

    [SerializeField] int resourceCost = 10;
    [SerializeField] Health health = null;
    [SerializeField] UnitMovement unitMovement = null;
    [SerializeField] Targeter targeter = null;
    [SerializeField] SpriteRenderer highlightMarker = null;
    [SerializeField] UnityEvent onSelected = null;
    [SerializeField] UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public int ResourceCost
    {
        get
        {
            return resourceCost;
        }
    }

    public UnitMovement Movement
    {
        get
        {
            return unitMovement;
        }
    }

    public Targeter MyTargeter
    {
        get
        {
            return targeter;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);

        health.ServerOnTakeDamage += ServerHandleTakeDamage;
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);

        health.ServerOnTakeDamage -= ServerHandleTakeDamage;
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    public void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    public void ServerHandleTakeDamage(Transform enemyTransform)
    {
        // if i don't have a task and i am not en route
        if (!targeter.Target && !unitMovement.HasWaypoint)
        {
            // then go towards the enemy to attack
            targeter.Target = enemyTransform.GetComponent<Targetable>();
        }
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);

        RTSPlayerInfo playerInfo = connectionToClient.identity.GetComponent<RTSPlayerInfo>();

        // change selection sprite color
        highlightMarker.color = playerInfo.TeamColor;

        // change health bar color
        GetComponent<HealthDisplay>().SetHealthBarColor(playerInfo.TeamColor);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) return;

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
