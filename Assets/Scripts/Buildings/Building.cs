using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Building : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] GameObject buildingPreview = null;
    [SerializeField] Sprite icon = null;
    [SerializeField] int id = -1;
    [SerializeField] int price = 100;

    public static event Action<Building> ServerOnBuildingSpawned;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnBuildingSpawned;
    public static event Action<Building> AuthorityOnBuildingDespawned;
    
    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public GameObject BuildingPreview
    {
        get
        {
            return buildingPreview;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        ServerOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBuildingDespawned?.Invoke(this);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartAuthority()
    {
        AuthorityOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) return;

        AuthorityOnBuildingDespawned?.Invoke(this);
    }

    #endregion
    
}
