using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] Health health;
    [SerializeField] GameObject unitPrefab;
    [SerializeField] Transform unitSpawnPoint;

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += Health_ServerOnDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= Health_ServerOnDie;
    }

    [Server]
    void Health_ServerOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!hasAuthority) return;
        CmdSpawnUnit();
    }
    #endregion
}