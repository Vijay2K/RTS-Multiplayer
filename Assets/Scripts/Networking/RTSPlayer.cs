using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();
    [SerializeField] private List<Building> myBuildings = new List<Building>();

    public List<Unit> GetMyUnits() => myUnits;
    public List<Building> GetMyBuildings() => myBuildings;

    #region SERVER

    public override void OnStartServer() {
        //UNITS
        Unit.ServerOnUnitSpawned += ServerHandleOnUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleOnUnitDespawned;

        //BUILDINGS
        Building.ServerOnBuildingSpawned += ServerHandleOnBuildingSpawned;
        Building.ServerOnBuildingDespawned += ServerHandleOnBuildingDespawned;
    }

    public override void OnStopServer() {
        //UNITS
        Unit.ServerOnUnitSpawned -= ServerHandleOnUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleOnUnitSpawned;

        //BUILDINGS
        Building.ServerOnBuildingSpawned -= ServerHandleOnBuildingSpawned;
        Building.ServerOnBuildingDespawned -= ServerHandleOnBuildingDespawned;
    }

    #region SERVER UNITS

    private void ServerHandleOnUnitSpawned(Unit unit) {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myUnits.Add(unit);
    }

    private void ServerHandleOnUnitDespawned(Unit unit) {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myUnits.Remove(unit);
    }

    #endregion

    #region SERVER BUILDINGS

    private void ServerHandleOnBuildingSpawned(Building building) {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myBuildings.Add(building);
    }

    private void ServerHandleOnBuildingDespawned(Building building) {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myBuildings.Remove(building);
    }

    #endregion

    #endregion

    #region CLIENT

    public override void OnStartAuthority() {
        if (NetworkServer.active) return;

        Unit.AuthorityOnUnitSpawned += HandleAuthorityOnUnitSpawned;
        Unit.AuthorityOnUnitDespawed += HandleAuthorityOnUnitDespawned;

        Building.AuthorityOnBuildingSpawned += HandleAuthorityOnBuildingSpawned;
        Building.AuthorityOnBuildingDespawned += HandleAuthorityOnBuildingDespawned;
    }

    public override void OnStopClient() {
        if (!isClientOnly || !hasAuthority) return;

        Unit.AuthorityOnUnitDespawed -= HandleAuthorityOnUnitSpawned;
        Unit.AuthorityOnUnitDespawed -= HandleAuthorityOnUnitDespawned;

        Building.AuthorityOnBuildingSpawned -= HandleAuthorityOnBuildingSpawned;
        Building.AuthorityOnBuildingDespawned -= HandleAuthorityOnBuildingDespawned;
    }


    private void HandleAuthorityOnUnitSpawned(Unit unit) {
        myUnits.Add(unit);
    }

    private void HandleAuthorityOnUnitDespawned(Unit unit) {
        myUnits.Remove(unit);
    }

    private void HandleAuthorityOnBuildingSpawned(Building building) {
        myBuildings.Add(building);
    }

    private void HandleAuthorityOnBuildingDespawned(Building building) {
        myBuildings.Remove(building);
    } 

    #endregion
}
