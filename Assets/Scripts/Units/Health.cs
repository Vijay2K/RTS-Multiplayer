using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;
    public static event Action ServerOnDie;

    #region SERVER

    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damage) {
        if (currentHealth == 0) return;
        
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        
        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();

        Debug.Log("Unit Died");
    }

    #endregion

    #region CLIENT


    #endregion
}
