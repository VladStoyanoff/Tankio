using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damageToDeal;
    [SerializeField] float destroyAfterSeconds;
    [SerializeField] float launchForce;

    void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(Destroy), destroyAfterSeconds);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient) return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damageToDeal);
        }

        Destroy();
    }

    [Server]
    void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }

}
