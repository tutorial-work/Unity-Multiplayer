using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameOverDisplay : MonoBehaviour
{
    /********** MARK: Class Variables **********/
    #region Class Variables

    [SerializeField] GameObject gameOverDisplayParent = null;
    [SerializeField] TMP_Text winnerNameText = null;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void Start()
    {
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            // stop hosting
            NetworkManager.singleton.StopHost();
        }
        else
        {
            // stop client
            NetworkManager.singleton.StopClient();
        }
    }

    private void ClientHandleGameOver(string winner)
    {
        winnerNameText.text = $"{winner} Has Won!";

        gameOverDisplayParent.SetActive(true);
    }

    #endregion
}
