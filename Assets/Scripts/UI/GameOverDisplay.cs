using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject gameOverDisplayPanel = null;
    [SerializeField] private TMP_Text winnerDisplayText = null;

    private void Start() {
        GameOverHandler.ClientOnGameOver += HandleClientOnGameOver;
    }

    private void OnDestroy() {
        GameOverHandler.ClientOnGameOver -= HandleClientOnGameOver;
    }

    public void LeaveGame() { 
        
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }

    private void HandleClientOnGameOver(string winnerID) {
        winnerDisplayText.text = $"{winnerID} has won!!";
        gameOverDisplayPanel.SetActive(true);
    }
}
