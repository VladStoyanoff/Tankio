using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] int maxHealth;

    [SyncVar(hook = nameof(HandleHealthUpdating))]
    int currentHealth;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
        UnitBase.ServerOnPlayerDie += Unit_ServerOnPlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= Unit_ServerOnPlayerDie;
    }

    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();

        Debug.Log("Died");
    }

    void Unit_ServerOnPlayerDie(int connectionId)
    {
        if (connectionToClient.connectionId != connectionId) return;
        DealDamage(currentHealth);
    }

    #endregion

    #region Client

    void HandleHealthUpdating(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion


}
