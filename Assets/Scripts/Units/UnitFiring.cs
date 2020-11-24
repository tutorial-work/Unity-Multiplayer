using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
    /********** MARK: Class Variables **********/

    [SerializeField] Targeter targeter = null;
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawnPoint = null;
    [SerializeField] float fireRange = 5f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float rotationSpeed = 20f;

    float lastFireTime;

    /********** MARK: Properties **********/
    #region Properties


    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.Target;

        if (target == null) return;

        if (!CanFireAtTarget()) return;

        Quaternion targetRotation = Quaternion.LookRotation(
            target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / fireRate) + lastFireTime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(
                targeter.Target.GetAimAtPoint().position - projectileSpawnPoint.position);

            GameObject projectileInstance =
                Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            UnitProjectile projectile = projectileInstance.GetComponent<UnitProjectile>();
            projectile.OriginTransform = transform;

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.Target.transform.position - transform.position).sqrMagnitude
            <= fireRange * fireRange;
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
