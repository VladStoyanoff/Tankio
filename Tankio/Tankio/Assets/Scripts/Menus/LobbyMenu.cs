using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject lobbyUI;
    [SerializeField] Button startGameButton;
    [SerializeField] TMP_Text[] playerNameTexts = new TMP_Text[4]; 

    void Start()
    {
        NetworkManagerTankio.ClientOnConnected += NetworkManagerTankio_ClientOnConnected;
        NetworkPlayerTankio.AuthorityOnPartyOwnerStateUpdated += NetworkPlayerTankio_AuthorityOnPartyOwnerStateUpdated;
        NetworkPlayerTankio.ClientOnInfoUpdated += NetworkPlayerTankio_ClientOnInfoUpdated;
    }

    void OnDestroy()
    {
        NetworkManagerTankio.ClientOnConnected -= NetworkManagerTankio_ClientOnConnected;
        NetworkPlayerTankio.AuthorityOnPartyOwnerStateUpdated -= NetworkPlayerTankio_AuthorityOnPartyOwnerStateUpdated;
        NetworkPlayerTankio.ClientOnInfoUpdated -= NetworkPlayerTankio_ClientOnInfoUpdated;
    }

    void NetworkManagerTankio_ClientOnConnected()
    {
        lobbyUI.SetActive(true);
    }

    void NetworkPlayerTankio_ClientOnInfoUpdated()
    {
        List<NetworkPlayerTankio> players = ((NetworkManagerTankio)NetworkManager.singleton).Players;

        for(int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].GetDisplayName();
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
        }

        startGameButton.interactable = players.Count >= 1;
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
