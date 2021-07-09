using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> GetMyUnits() {
        return myUnits;
    }

    #region SERVER

    public override void OnStartServer() {
        Unit.ServerOnUnitSpawned += ServerHandleOnUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleOnUnitDespawned;
    }

    public override void OnStopServer() {
        Unit.ServerOnUnitSpawned -= ServerHandleOnUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleOnUnitSpawned;
    }


    private void ServerHandleOnUnitSpawned(Unit unit) {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myUnits.Add(unit);
    }

    private void ServerHandleOnUnitDespawned(Unit unit) {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
        myUnits.Remove(unit);
    }

    #endregion

    #region CLIENT

    public override void OnStartAuthority() {
        if (NetworkServer.active) return;

        Unit.AuthorityOnUnitSpawned += HandleAuthorityOnUnitSpawned;
        Unit.AuthorityOnUnitDespawed += HandleAuthorityOnUnitDespawned;
    }

    public override void OnStopClient() {
        if (!isClientOnly || !hasAuthority) return;

        Unit.AuthorityOnUnitDespawed -= HandleAuthorityOnUnitSpawned;
        Unit.AuthorityOnUnitDespawed -= HandleAuthorityOnUnitDespawned;
    }


    private void HandleAuthorityOnUnitSpawned(Unit unit) {
        myUnits.Add(unit);
    }

    private void HandleAuthorityOnUnitDespawned(Unit unit) {
        myUnits.Remove(unit);
    }

    #endregion
}
