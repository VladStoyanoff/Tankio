using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
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

    [Server]
    void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }

}
