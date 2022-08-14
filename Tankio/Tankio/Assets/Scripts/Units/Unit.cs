using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Mirror;

public class Unit : NetworkBehaviour
{
    [SerializeField] int resourceCost = 10;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] Targeter targeter;
    [SerializeField] UnityEvent onSelected;
    [SerializeField] UnityEvent onDeselected;
    [SerializeField] Health health;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region Server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
        health.ServerOnDie += Health_ServerOnDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
        health.ServerOnDie -= Health_ServerOnDie;
    }

    void Health_ServerOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) return;
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;
        onDeselected?.Invoke();
    }

    #endregion

    public UnitMovement GetUnitMovement() => unitMovement;
    public Targeter GetTargeter() => targeter;
    public int GetResourceCost() => resourceCost;
}
