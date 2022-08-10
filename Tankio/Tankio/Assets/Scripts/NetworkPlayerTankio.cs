using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NetworkPlayerTankio : NetworkBehaviour
{
    [SerializeField] TMP_Text displayNameText = null;
    [SerializeField] Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField] string displayName = "Missing Name";
     
    [SyncVar(hook = nameof(HandleDisplayColorUpdated))] 
    Color displayColor = Color.black;

#region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetRandomColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    void CmdSetDisplayName(string newDisplayName)
    {
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

#endregion

#region Client

    void HandleDisplayColorUpdated(Color oldColor, Color newDisplayColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newDisplayColor);
    }

    void HandleDisplayNameUpdated(string oldName, string newDisplayName)
    {
        displayNameText.text = newDisplayName;
    }

    [ContextMenu("Set My Name")]
    void SetMyName()
    {
        CmdSetDisplayName("NewNAme");
    }

    [ClientRpc]
    void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}
