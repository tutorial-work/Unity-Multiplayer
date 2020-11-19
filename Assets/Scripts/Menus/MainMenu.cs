using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

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
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4); // HACK: hard-coded number of players
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
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
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
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return; // do nothing if we're the server

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby), // Steam ID object with the lobby steam id
            "HostAddress"
        );

        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);
    }

    #endregion
}
