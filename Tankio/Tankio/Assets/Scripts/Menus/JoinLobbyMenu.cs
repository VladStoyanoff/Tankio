using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject landingPagePanel;
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] Button joinButton;

    void OnEnable()
    {
        NetworkManagerTankio.ClientOnConnected += NetworkManagerTankio_ClientOnConnected;
        NetworkManagerTankio.ClientOnDisconnected += NetworkManagerTankio_ClientOnDisconnected;
    }

    void OnDisable()
    {
        NetworkManagerTankio.ClientOnConnected -= NetworkManagerTankio_ClientOnConnected;
        NetworkManagerTankio.ClientOnDisconnected -= NetworkManagerTankio_ClientOnDisconnected;
    }

    public void Join()
    {
        var address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    void NetworkManagerTankio_ClientOnConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    void NetworkManagerTankio_ClientOnDisconnected()
    {
        joinButton.interactable = true;
    }

}

