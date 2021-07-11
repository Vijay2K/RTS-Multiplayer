using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab = null;
    [SerializeField] private GameOverHandler gameOverHandlerPrefab = null;

    public override void OnServerAddPlayer(NetworkConnection conn) {
        base.OnServerAddPlayer(conn);
        Debug.Log($"There are {numPlayers} players are connected...");

        GameObject unitSpawnerPrefabInstance = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerPrefabInstance, conn);
    }

    public override void OnServerSceneChanged(string sceneName) {
        if(SceneManager.GetActiveScene().name.StartsWith("Game")) {
            GameOverHandler gameOverHandleInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandleInstance.gameObject);
        }
    }
}
