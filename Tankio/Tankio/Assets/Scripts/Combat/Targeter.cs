using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    Targetable target;

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += GameOverHandler_ServerOnGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= GameOverHandler_ServerOnGameOver;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)) return;

        target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    [Server]
    void GameOverHandler_ServerOnGameOver()
    {
        ClearTarget();
    }


    public Targetable GetTarget() => target;  
}
