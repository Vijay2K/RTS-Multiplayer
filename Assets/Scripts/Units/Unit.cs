using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private Health health = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawed;


    public Targeter GetTargeter() => targeter;
    public UnitMovement GetUnitMovement() => unitMovement;


    #region SERVER

    public override void OnStartServer() {
        health.ServerOnDie += ServerHandleOnUnitDie;
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer() {
        health.ServerOnDie -= ServerHandleOnUnitDie;
        ServerOnUnitDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleOnUnitDie() {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region CLIENT

    public override void OnStartAuthority() { 
        if (!hasAuthority) return;
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient() {
        if (!hasAuthority) return;
        AuthorityOnUnitDespawed?.Invoke(this);
    }

    [Client]
    public void Select() {
        if (!hasAuthority) return;
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect() {
        if (!hasAuthority) return;
        onDeselected?.Invoke();
    }

    #endregion
}
