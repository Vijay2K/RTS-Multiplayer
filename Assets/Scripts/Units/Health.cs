using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthBarUpdate))]
    private int currentHealth;

    public event Action ServerOnDie;
    public event Action<int, int> ClientOnHealthUpdated;


    #region SERVER

    public override void OnStartServer() {
        currentHealth = maxHealth;
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer() {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    public void DealDamage(int damage) {
        if (currentHealth == 0) return;
        
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        
        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionID) {
        if (connectionToClient.connectionId != connectionID) return;
        DealDamage(currentHealth);
    }

    #endregion

    #region CLIENT

    private void HandleHealthBarUpdate(int oldHealth, int newHealth) {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
