﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] TMP_Text debugText = null;

    [SerializeField] private GameObject landingPagePanel = null;

    [SerializeField] bool useSteam = false;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public static bool UseSteam { get; private set; }

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void OnValidate()
    {
        if (useSteam) Debug.LogWarning("This build is using Steam");
        else Debug.LogWarning("This build is NOT using Steam");

        UseSteam = useSteam;
    }

    private void Start()
    {
        if (!useSteam) { return; }

        Debug.Log("starting SetupSteamCallbacks");
        debugText.text += ">starting SetupSteamCallbacks\n";

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        Debug.Log("completed SetupSteamCallbacks");
        debugText.text += ">completed SetupSteamCallbacks\n";
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        if (useSteam)
        {
            Debug.Log("starting SteamMatchmaking.CreateLobby");
            debugText.text += ">starting SteamMatchmaking.CreateLobby\n";


            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);

            Debug.Log("completed SteamMatchmaking.CreateLobby");
            debugText.text += ">completed SteamMatchmaking.CreateLobby\n";

            return;
        }

        NetworkManager.singleton.StartHost();
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        Debug.Log("starting OnLobbyCreated");
        debugText.text += ">starting OnLobbyCreated\n";

        if (callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return;
        }

        RTSNetworkManager.LobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        NetworkManager.singleton.StartHost();

        SteamMatchmaking.SetLobbyData(
            RTSNetworkManager.LobbyId,
            "HostAddress",
            SteamUser.GetSteamID().ToString());

        Debug.Log("completed OnLobbyCreated");
        debugText.text += ">completed OnLobbyCreated\n";

    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("starting OnGameLobbyJoinRequested");
        debugText.text += ">starting OnGameLobbyJoinRequested\n";

        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);

        Debug.Log("completed OnGameLobbyJoinRequested");
        debugText.text += ">completed OnGameLobbyJoinRequested\n";
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log("starting OnLobbyEntered");
        debugText.text += ">starting OnLobbyEntered\n";

        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            RTSNetworkManager.LobbyId,
            "HostAddress");

        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);

        Debug.Log("completed OnLobbyEntered");
        debugText.text += ">completed OnLobbyEntered\n";
    }

    #endregion
}
