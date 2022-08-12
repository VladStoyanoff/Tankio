using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NetworkManagerTankio : NetworkManager
{
    [SerializeField] GameObject unitSpawnerPrefab;
    [SerializeField] GameOverHandler gameOverHandlerPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerInstance, conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Map_01"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
        }
    }
}
