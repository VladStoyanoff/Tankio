using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject lobbyUI;
    [SerializeField] Button startGameButton;

    void Start()
    {
        NetworkManagerTankio.ClientOnConnected += NetworkManagerTankio_ClientOnConnected;
        NetworkPlayerTankio.AuthorityOnPartyOwnerStateUpdated += NetworkPlayerTankio_AuthorityOnPartyOwnerStateUpdated;
    }

    void OnDestroy()
    {
        NetworkManagerTankio.ClientOnConnected -= NetworkManagerTankio_ClientOnConnected;
        NetworkPlayerTankio.AuthorityOnPartyOwnerStateUpdated -= NetworkPlayerTankio_AuthorityOnPartyOwnerStateUpdated;
    }

    void NetworkManagerTankio_ClientOnConnected()
    {
        lobbyUI.SetActive(true);
    }

    void NetworkPlayerTankio_AuthorityOnPartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<NetworkPlayerTankio>().CmdStartGame();
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();

            SceneManager.LoadScene(0);
        }
    }
}
