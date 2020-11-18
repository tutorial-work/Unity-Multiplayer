using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class LobbyMenu : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] GameObject lobbyUI = null;
    [SerializeField] Button startGameButton = null;

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void Start()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
    }

    private void OnDestroy()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
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
