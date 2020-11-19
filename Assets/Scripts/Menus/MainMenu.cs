using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] TMP_Text debugText = null;
    [SerializeField] GameObject landingPagePanel = null;

    [SerializeField] bool useSteam = false;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void Start()
    {
        // Steam Callback Code setup if using steam
        if (useSteam) SetupSteamCallbacks();
    }

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        if (useSteam) // if using steam prepare the lobby for the steam callbacks
        {
            Debug.Log("starting SteamMatchmaking.CreateLobby");
            debugText.text += ">starting SteamMatchmaking.CreateLobby\n";

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4); // HACK: hard-coded number of players

            Debug.Log("completed SteamMatchmaking.CreateLobby");
            debugText.text += ">completed SteamMatchmaking.CreateLobby\n";
        }
        else // otherwise, just create the host lobby
        {
            NetworkManager.singleton.StartHost();
        }
    }

    #endregion

    /********** MARK: Steam Functions **********/
    #region Steam Functions

    private void SetupSteamCallbacks()
    {
        Debug.Log("starting SetupSteamCallbacks");
        debugText.text += ">starting SetupSteamCallbacks\n";

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        Debug.Log("completed SetupSteamCallbacks");
        debugText.text += ">completed SetupSteamCallbacks\n";
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        Debug.Log("starting OnLobbyCreated");
        debugText.text += ">starting OnLobbyCreated\n";

        // TODO: idk what this if statement does, he didn't go over it
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return;
        }

        NetworkManager.singleton.StartHost();

        // this sends a new client the lobby ID
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress", // this is the key to get the lobby ID
            SteamUser.GetSteamID().ToString()
        );

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

        if (NetworkServer.active) return; // do nothing if we're the server

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby), // Steam ID object with the lobby steam id
            "HostAddress"
        );

        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);

        Debug.Log("completed OnLobbyEntered");
        debugText.text += ">completed OnLobbyEntered\n";
    }

    #endregion
}