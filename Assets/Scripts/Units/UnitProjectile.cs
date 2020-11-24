using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitProjectile : NetworkBehaviour
{
    /********** MARK: Class Variables **********/

    [SerializeField] Rigidbody rb = null;
    [SerializeField] int damageToDeal = 20;
    [SerializeField] float destroyAfterSeconds = 5f;
    [SerializeField] float launchForce = 10f;

    /********** MARK: Properties **********/
    #region Properties

    public int DamageToDeal
    {
        get
        {
            return damageToDeal;
        }
    }

    public Transform OriginTransform { get; [Server] set; }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    private void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient) return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(this);
        }

        DestroySelf();
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
