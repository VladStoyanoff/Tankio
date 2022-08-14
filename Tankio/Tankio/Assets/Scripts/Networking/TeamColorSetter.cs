using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField] Renderer[] colorRenderers = new Renderer[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    Color teamColor = new Color();

    #region Server

    public override void OnStartServer()
    {
        var player = connectionToClient.identity.GetComponent<NetworkPlayerTankio>();

        teamColor = player.GetTeamColor();
    }

    #endregion

    #region Client

    void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach(Renderer renderer in colorRenderers)
        {
            renderer.material.SetColor("_BaseColor", newColor);
        }
    }

    #endregion
}
