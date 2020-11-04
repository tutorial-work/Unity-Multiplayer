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
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();

        Debug.Log(gameObject.name + " has died");
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
