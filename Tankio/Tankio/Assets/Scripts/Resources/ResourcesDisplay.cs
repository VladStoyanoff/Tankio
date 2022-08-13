using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text resourcesText;

    NetworkPlayerTankio player;

    void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<NetworkPlayerTankio>();
            if(player != null)
            {
                Player_ClientOnResourcesUpdated(player.GetResources());
                player.ClientOnResourcesUpdated += Player_ClientOnResourcesUpdated;
            }
        } 
    }

    void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= Player_ClientOnResourcesUpdated;
    }

    void Player_ClientOnResourcesUpdated(int resources)
    {
        resourcesText.text = $"Resources: {resources}";
    }
}
