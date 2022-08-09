using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerTankio : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        NetworkPlayerTankio player = conn.identity.GetComponent<NetworkPlayerTankio>();
        player.SetDisplayName($"Player {numPlayers}");
        Color displayColor = new Color(Random.value, Random.value, Random.value);
        player.SetRandomColor(displayColor);
    }
}
