using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class RTSPlayer : NetworkBehaviour
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] Transform cameraTransform = null;
    [SerializeField] LayerMask buildingBlockLayer = new LayerMask();
    [SerializeField] Building[] buildings = new Building[0];
    [SerializeField] float buildingRangeLimit = 5f;

    [SyncVar(hook = nameof(ClientHandleCurrentResourcesUpdate))]
    [SerializeField] int currentResources = 30;

    [SyncVar(hook = nameof(ClientHandleMaxResourcesUpdate))]
    [SerializeField] int maxResources = 50;

    public event Action<int, int> ClientOnResourcesUpdated;

    List<Unit> myUnits = new List<Unit>();

    List<Building> myBuildings = new List<Building>();

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public Transform CameraTransform
    {
        get
        {
            return cameraTransform;
        }
    }

    public int CurrentResources
    {
        get
        {
            return currentResources;
        }

        [Server] // TODO: im not really sure if this works? validate?
        set
        {
            currentResources = Mathf.Clamp(value, 0, maxResources);
        }
    }

    public int MaxResources
    {
        get
        {
            return maxResources;
        }

        [Server]
        set
        {
            maxResources = value;
        }
    }

    public List<Unit> MyUnits
    {
        get
        {
            return myUnits;
        }
    }

    public List<Building> MyBuildings
    {
        get
        {
            return myBuildings;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;

        Building.ServerOnBuildingSpawned += ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned += ServerHandleBuildingDespawned;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;

        Building.ServerOnBuildingSpawned -= ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned -= ServerHandleBuildingDespawned;
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

    [Server] // TODO: Verify this line
    private void ServerHandleBuildingSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myBuildings.Add(building);

        // HACK: these lines should probably be moved into resource Generator
        if (!building.TryGetComponent<ResourceStorage>(out ResourceStorage resourceStorage)) return;
        maxResources += resourceStorage.ResourceCapacity; 
    }

    [Server] // TODO: Verify this line
    private void ServerHandleBuildingDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myBuildings.Remove(building);

        // HACK: these lines should probably be moved into resource Generator
        if (!building.TryGetComponent<ResourceStorage>(out ResourceStorage resourceStorage)) return;
        maxResources -= resourceStorage.ResourceCapacity;
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingID, Vector3 point)
    {
        Building buildingToPlace = null;

        foreach (Building building in buildings)
        {
            if (building.Id == buildingID)
            {
                buildingToPlace = building;
                break;
            }
        }

        if (buildingToPlace == null) return;

        if (currentResources < buildingToPlace.Price) return;

        BoxCollider buildingCollider = buildingToPlace.GetComponentInChildren<BoxCollider>();

        if (!CanPlaceBuilding(buildingCollider, point)) return;

        GameObject buildingInstance =
            Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);

        CurrentResources = currentResources - buildingToPlace.Price;
    }

    public bool CanPlaceBuilding(BoxCollider buildingCollider, Vector3 point)
    {
        Collider[] colliders = Physics.OverlapBox(
            point + buildingCollider.center, 
            buildingCollider.size / 2, Quaternion.identity, 
            buildingBlockLayer
        );

        if (Physics.CheckBox(
            point + buildingCollider.center,
            buildingCollider.size / 2,
            Quaternion.identity,
            buildingBlockLayer))
        {
            return false;
        }

        foreach (Unit unit in myUnits)
        {
            if (unit.GetComponent<UnitBuilder>())
            {
                if ((point - unit.transform.position).sqrMagnitude <=
                buildingRangeLimit * buildingRangeLimit)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void CmdStartGame()
    {
        if (!GetComponent<RTSPlayerInfo>().IsPartyOwner) return;

        ((RTSNetworkManager)NetworkManager.singleton).StartGame();
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartAuthority()
    {
        // if this is the server, return... i.e. add unit to list only if client
        if (NetworkServer.active) return;

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;

        Building.AuthorityOnBuildingSpawned += AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingDespawned += AuthorityHandleBuildingDespawned;
    }

    public override void OnStartClient()
    {
        if (NetworkServer.active) return;

        DontDestroyOnLoad(gameObject);

        ((RTSNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        Debug.LogWarning("RTSPlayer Stopping Client");
        GetComponent<RTSPlayerInfo>().AuthorityHandleOnStopClient();

        if (!isClientOnly) return; // this helps the client get a list of the players

        ((RTSNetworkManager)NetworkManager.singleton).Players.Remove(this);

        if (!hasAuthority) return; // if this is the server, return... i.e. add unit to list only if client

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;

        Building.AuthorityOnBuildingSpawned -= AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingDespawned -= AuthorityHandleBuildingDespawned;
    }

    private void ClientHandleCurrentResourcesUpdate(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources, maxResources);
    }

    private void ClientHandleMaxResourcesUpdate(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(currentResources, newResources);
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        myUnits.Remove(unit);
    }

    private void AuthorityHandleBuildingSpawned(Building building)
    {
        myBuildings.Add(building);
    }

    private void AuthorityHandleBuildingDespawned(Building building)
    {
        myBuildings.Remove(building);
    }

    #endregion
}