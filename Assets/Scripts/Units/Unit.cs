using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawed;

    public UnitMovement GetUnitMovement() {
        return unitMovement;
    }

    #region SERVER

    public override void OnStartServer() {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer() {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    #region CLIENT

    public override void OnStartClient() {
        if (!isClientOnly || !hasAuthority) return;
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient() {
        if (!isClientOnly || !hasAuthority) return;
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
