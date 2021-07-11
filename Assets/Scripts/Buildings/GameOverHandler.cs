using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameOverHandler : NetworkBehaviour
{

    private List<UnitBase> bases = new List<UnitBase>();

    #region SERVER

    public override void OnStartServer() {
        UnitBase.ServerOnUnitBaseSpawned += HandleServerOnUnitBaseSpawned;
        UnitBase.ServerOnUnitBaseDespawned += HandleServerOnUnitBaseDespawned;
    }

    public override void OnStopServer() {
        UnitBase.ServerOnUnitBaseSpawned -= HandleServerOnUnitBaseSpawned;
        UnitBase.ServerOnUnitBaseDespawned -= HandleServerOnUnitBaseDespawned;
    }
    
    [Server]
    private void HandleServerOnUnitBaseSpawned(UnitBase unitBase) {
        bases.Add(unitBase);
    }

    [Server]
    private void HandleServerOnUnitBaseDespawned(UnitBase unitBase) {
        bases.Remove(unitBase);
        if (bases.Count != 1) return;
        Debug.Log("Game Over");
    }

    #endregion

    #region CLIENT


    #endregion
}
