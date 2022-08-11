using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] Targeter targeter;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float fireRange;
    [SerializeField] float fireRate;
    [SerializeField] float rotationSpeed;

    float lastFireTime;

    [ServerCallback]
    void Update()
    {
        var target = targeter.GetTarget();

        if (target == null) return;

        if (!CanFireAtTarget()) return;

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if(Time.time > (1 / fireRate) + lastFireTime)
        {
            var projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projectileSpawnPoint.position);

            var projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);
            
            lastFireTime = Time.time;
        }
    }

    [Server]
    bool CanFireAtTarget()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude 
            <= fireRange * fireRange;
    }
}
