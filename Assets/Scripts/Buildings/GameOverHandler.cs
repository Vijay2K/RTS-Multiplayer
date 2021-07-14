using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameOverHandler : NetworkBehaviour
{
    public static event Action ServerOnGameOver;
    public static event Action<string> ClientOnGameOver;

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

        int playerID = bases[0].connectionToClient.connectionId;

        RpcHandleClientOnGameOver($"Player {playerID}");
        Debug.Log("Game Over");

        ServerOnGameOver?.Invoke();
    }

    #endregion

    #region CLIENT

    [ClientRpc]
    private void RpcHandleClientOnGameOver(string playerID) {
        ClientOnGameOver?.Invoke(playerID);
    }

    #endregion
}
