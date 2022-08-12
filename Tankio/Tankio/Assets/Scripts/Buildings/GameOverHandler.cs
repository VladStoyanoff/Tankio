using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameOverHandler : NetworkBehaviour
{
    public static event Action ServerOnGameOver;

    public static event Action<string> ClientOnGameOver;

    List<UnitBase> bases = new List<UnitBase>();

    #region Server

    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawned += Unit_ServerOnUnitSpawned;
        UnitBase.ServerOnBaseDespawned += Unit_ServerOnUnitDespawned;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpawned -= Unit_ServerOnUnitSpawned;
        UnitBase.ServerOnBaseDespawned -= Unit_ServerOnUnitDespawned;
    }

    [Server]
    void Unit_ServerOnUnitSpawned(UnitBase unitBase)
    {
        bases.Add(unitBase);
    }

    [Server]
    void Unit_ServerOnUnitDespawned(UnitBase unitBase)
    {
        bases.Remove(unitBase);

        if (bases.Count != 1) return;

        int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver($"Player {playerId}");

        ServerOnGameOver?.Invoke();
    }

    #endregion
    #region Client

    [ClientRpc]
    void RpcGameOver(string winner)
    {
        ClientOnGameOver?.Invoke(winner);
    }

    #endregion
}
