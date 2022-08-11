using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using TMPro;

public class NetworkPlayerTankio : NetworkBehaviour
{
    List<Unit> myUnits = new List<Unit>();

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += Unit_ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += Unit_ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= Unit_ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= Unit_ServerHandleUnitDespawned;
    }

    #region Server
    void Unit_ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Add(unit);
    }

    void Unit_ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Remove(unit);
    }
    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly) return;
        Unit.AuthorityOnUnitSpawned += Unit_AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += Unit_AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        Unit.AuthorityOnUnitSpawned -= Unit_AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= Unit_AuthorityHandleUnitDespawned;
    }

    void Unit_AuthorityHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Remove(unit);
    }

    void Unit_AuthorityHandleUnitDespawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Add(unit);
    }



    #endregion
}
