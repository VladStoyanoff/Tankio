using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] GameObject gameOverDisplayParent;
    [SerializeField] TMP_Text winnerNameText;

    void Start()
    {
        GameOverHandler.ClientOnGameOver += GameOverHandler_ClientOnGameOver;
    }

    void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= GameOverHandler_ClientOnGameOver;
    }

    public void LeaveGame()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    void GameOverHandler_ClientOnGameOver(string winner)
    {
        winnerNameText.text = $"{winner} Has Won!";

        gameOverDisplayParent.SetActive(true);
    }
}
