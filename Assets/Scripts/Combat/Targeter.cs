using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    [SerializeField] private Targetable target = null;

    #region SERVER

    [Command]
    public void CmdSetTarget(GameObject targetGameObject) {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) return;

        this.target = target;
    }

    [Server]
    public void ClearTarget() {
        this.target = null;
    }

    #endregion
}