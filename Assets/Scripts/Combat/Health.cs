using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Health : NetworkBehaviour
{
    /********** MARK: Class Variables **********/

    [SerializeField] int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    int currentHealth;

    public event Action<Transform> ServerOnTakeDamage;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    /********** MARK: Properties **********/
    #region Properties

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if (connectionToClient.connectionId != connectionId) return;

        // this just auto kills the owner by dealing their current damage to themself
        DealDamage(currentHealth); 
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();

        Debug.LogWarning(gameObject.name + " has died");
    }

    [Server]
    public void DealDamage(UnitProjectile projectile)
    {
        if (projectile.OriginTransform) ServerOnTakeDamage?.Invoke(projectile.OriginTransform);

        DealDamage(projectile.DamageToDeal);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
