using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private float rotationSpeed = 0;
    [SerializeField] private float shootingRange = 0;
    [SerializeField] private float fireRate = 0;

    private float lastTimeFire;

    [ServerCallback]
    private void Update() {
        Targetable target = targeter.GetTarget();
        if (target == null) return;
        if (!CanAttackAtTarget()) return;

        Quaternion lookAtTarget = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtTarget, rotationSpeed * Time.deltaTime);

        if(Time.time > (1 / fireRate) + lastTimeFire) {
            Quaternion projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projectileSpawnPoint.position);
            GameObject projectilePrefabInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectilePrefabInstance, connectionToClient);
            lastTimeFire = Time.time;
        }
    }

    [Server]
    private bool CanAttackAtTarget() {
        Targetable target = targeter.GetTarget();
        return (target.transform.position - transform.position).sqrMagnitude <= shootingRange * shootingRange;
    }
}
