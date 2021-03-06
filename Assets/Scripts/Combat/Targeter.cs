using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    private Targetable target = null;

    public Targetable GetTarget() => target;

    #region SERVER

    public override void OnStartServer() {
        GameOverHandler.ServerOnGameOver += HandleServerOnGameOver;
    }

    public override void OnStopServer() {
        GameOverHandler.ServerOnGameOver -= HandleServerOnGameOver;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject) {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) return;

        this.target = target;
    }

    [Server]
    public void ClearTarget() {
        this.target = null;
    }

    [Server]
    public void HandleServerOnGameOver() {
        ClearTarget();
    }

    #endregion
}
