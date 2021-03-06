﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class LobbyMenu : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] GameObject lobbyUI = null;
    [SerializeField] Button startGameButton = null;
    [SerializeField] TMP_Text[] playerNameTexts = new TMP_Text[4]; // HACK: hardcoded
    [SerializeField] RawImage[] playerSteamImages = new RawImage[4];

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void Start()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSPlayerInfo.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        RTSPlayerInfo.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    private void OnDestroy()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSPlayerInfo.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        RTSPlayerInfo.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    private void ClientHandleInfoUpdated()
    {
        RTSNetworkManager rtsNetworkManager = (RTSNetworkManager)NetworkManager.singleton;
        List<RTSPlayer> players = rtsNetworkManager.Players;

        for (int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].GetComponent<RTSPlayerInfo>().DisplayName;
            playerSteamImages[i].texture = players[i].GetComponent<RTSPlayerInfo>().DisplayTexture;
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerSteamImages[i].texture = null;
        }

        startGameButton.interactable = (players.Count >= rtsNetworkManager.MinPlayersToStartGame);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<RTSPlayer>().CmdStartGame();
    }

    public void LeaveLobby()
    {

        if (NetworkServer.active && NetworkClient.isConnected) // are you a host?
        {
            NetworkManager.singleton.StopHost();
        }
        else // you must be a client
        {
            NetworkManager.singleton.StopClient();

            // this reloads the start menu, it's the lazy way rather than turning on/off various UI
            SceneManager.LoadScene(0);
        }
    }

    #endregion
}