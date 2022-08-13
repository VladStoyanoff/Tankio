using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using TMPro;

public class NetworkPlayerTankio : NetworkBehaviour
{
    [SerializeField] Building[] buildings = new Building[0];

    List<Unit> myUnits = new List<Unit>();
    List<Building> myBuildings = new List<Building>();

    [SyncVar(hook = nameof(ClientHandleResourcesUpdated))]
    int resources = 500;

    public event Action<int> ClientOnResourcesUpdated;

    public int GetResources() => resources;
    public List<Unit> GetMyUnits() => myUnits;
    public List<Building> GetMyBuildings() => myBuildings;

    [Server]
    public void SetResources(int newResources)
    {
        resources = newResources;
    }

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += Unit_ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += Unit_ServerHandleUnitDespawned;
        Building.ServerOnBuildingSpawned += Building_ServerOnBuildingSpawned;
        Building.ServerOnBuildingDespawned += Building_ServerOnBuildingDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= Unit_ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= Unit_ServerHandleUnitDespawned;
        Building.ServerOnBuildingSpawned -= Building_ServerOnBuildingSpawned;
        Building.ServerOnBuildingDespawned -= Building_ServerOnBuildingDespawned;
    }

    #region Server
    void Unit_ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Add(unit);
    }

    void Unit_ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Remove(unit);
    }

    void Building_ServerOnBuildingSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myBuildings.Add(building);
    }

    void Building_ServerOnBuildingDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myBuildings.Remove(building);
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 position)
    {
        Building buildingToPlace = null;

        foreach(Building building in buildings)
        {
            if(building.GetId() == buildingId)
            {
                buildingToPlace = building;
                break;
            }
        }

        if (buildingToPlace == null) return;

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, position, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) return;
        Unit.AuthorityOnUnitSpawned += Unit_AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += Unit_AuthorityHandleUnitDespawned;
        Building.AuthorityOnBuildingSpawned += Building_AuthorityOnBuildingSpawned;
        Building.AuthorityOnBuildingDespawned += Building_AuthorityOnBuildingDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) return;
        Unit.AuthorityOnUnitSpawned -= Unit_AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= Unit_AuthorityHandleUnitDespawned;
        Building.AuthorityOnBuildingSpawned -= Building_AuthorityOnBuildingSpawned;
        Building.AuthorityOnBuildingDespawned -= Building_AuthorityOnBuildingDespawned;
    }

    void Unit_AuthorityHandleUnitSpawned(Unit unit)
    {
        myUnits.Remove(unit);
    }

    void Unit_AuthorityHandleUnitDespawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    void Building_AuthorityOnBuildingSpawned(Building building)
    {
        myBuildings.Remove(building);
    }

    void Building_AuthorityOnBuildingDespawned(Building building)
    {
        myBuildings.Add(building);
    }

    void ClientHandleResourcesUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources);
    }

    #endregion

}

