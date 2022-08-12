using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] Health health;

    public static event Action<int> ServerOnPlayerDie;
    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDespawned;

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += Health_ServerOnDie;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= Health_ServerOnDie;

        ServerOnBaseDespawned?.Invoke(this);
    }

    void Health_ServerOnDie()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId);

        NetworkServer.Destroy(gameObject);
    }

    #endregion
    #region Client
    #endregion
}
