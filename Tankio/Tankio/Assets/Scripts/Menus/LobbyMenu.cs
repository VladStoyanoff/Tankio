using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject lobbyUI;

    void Start()
    {
        NetworkManagerTankio.ClientOnConnected += NetworkManagerTankio_ClientOnConnected;
    }

    void OnDestroy()
    {
        NetworkManagerTankio.ClientOnConnected -= NetworkManagerTankio_ClientOnConnected;
    }

    void NetworkManagerTankio_ClientOnConnected()
    {
        lobbyUI.SetActive(true);
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
