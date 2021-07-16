using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Building : NetworkBehaviour
{
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = -1;
    [SerializeField] private int price = 0;

    public static event Action<Building> ServerOnBuildingSpawned;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnBuildingSpawned;
    public static event Action<Building> AuthorityOnBuildingDespawned;

    public Sprite GetSprite() => icon;
    public int GetID() => id;
    public int GetPrice() => price;

    #region SERVER

    public override void OnStartServer() {
        ServerOnBuildingSpawned?.Invoke(this);
    }


    public override void OnStopServer() {
        ServerOnBuildingDespawned?.Invoke(this);
    }

    #endregion

    #region CLIENT

    public override void OnStartAuthority() {
        if (!hasAuthority) return;
        AuthorityOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient() {
        if (!hasAuthority) return;
        AuthorityOnBuildingDespawned?.Invoke(this);
    }

    #endregion
}
