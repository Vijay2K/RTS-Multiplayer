using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] private Health health = null;

    public static event Action<UnitBase> ServerOnUnitBaseSpawned;
    public static event Action<UnitBase> ServerOnUnitBaseDespawned;

    #region SERVER

    public override void OnStartServer() {
        health.ServerOnDie += HandleUnitOnserverDie;
        ServerOnUnitBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer() {
        ServerOnUnitBaseDespawned?.Invoke(this);
        health.ServerOnDie -= HandleUnitOnserverDie;
    }

    [Server]
    private void HandleUnitOnserverDie() {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region CLIENT


    #endregion
}
