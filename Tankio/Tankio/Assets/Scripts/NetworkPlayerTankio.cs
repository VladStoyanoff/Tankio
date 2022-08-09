using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerTankio : NetworkBehaviour
{
    [SyncVar][SerializeField] string displayName = "Missing Name";
    [SyncVar] Color color = Color.black;

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetRandomColor(Color newColor)
    {
        color = newColor;
    }
}
