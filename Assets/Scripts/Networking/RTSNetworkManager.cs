using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using Steamworks;

public class RTSNetworkManager : NetworkManager
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] GameObject unitBasePrefab = null;
    [SerializeField] GameObject smallCarPrefab = null;
    [SerializeField] GameOverHandler gameOverHandlerPrefab = null;
    [SerializeField] [Range(1, 4)] int minPlayersToStartGame = 1; // HACK: increase max num of players

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    bool isGameInProgress = false;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public static ulong LobbyId { get; set; }

    public List<RTSPlayer> Players { get; } = new List<RTSPlayer>();

    public int MinPlayersToStartGame
    {
        get
        {
            return minPlayersToStartGame;
        }
    }

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!isGameInProgress) return;

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }

    public void StartGame()
    {
        if (Players.Count < MinPlayersToStartGame) return;

        isGameInProgress = true;

        ServerChangeScene("Scene_Map_01"); // HACK: calling scene by string reference
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Players.Add(conn.identity.GetComponent<RTSPlayer>());

        RTSPlayerInfo playerInfo = conn.identity.GetComponent<RTSPlayerInfo>();

        if (MainMenu.UseSteam)
        {
            CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex(
                new CSteamID(LobbyId),
                numPlayers - 1
            );
            playerInfo.SteamId = steamId.m_SteamID; // this sets up all the steam info, name, picture
        }
        else
        {
            playerInfo.DisplayName = $"Player {Players.Count}";
        }

        playerInfo.TeamColor = new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)
        );

        playerInfo.IsPartyOwner = (Players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map")) // HACK: string reference
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);

            foreach (RTSPlayer player in Players)
            {
                // spawning unit base on server
                Vector3 pos = GetStartPosition().position;
                Quaternion rot = Quaternion.identity;
                GameObject unitBaseInstance = Instantiate(unitBasePrefab, pos, rot);

                // spawning the small car on server
                pos = unitBaseInstance.GetComponent<UnitBase>().SpawnPoint.position;
                GameObject smallCarInstance = Instantiate(smallCarPrefab, pos, rot);

                // server tells all clients to spawn instance
                NetworkServer.Spawn(unitBaseInstance, player.connectionToClient);
                NetworkServer.Spawn(smallCarInstance, player.connectionToClient);
            }
        }
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map")) // HACK: string reference
        {
            foreach (RTSPlayer player in Players)
            {
                RTSPlayerInfo playerInfo = player.GetComponent<RTSPlayerInfo>();

                foreach (Building building in player.MyBuildings)
                {
                    if (building.TryGetComponent<UnitBase>(out UnitBase unitBase))
                    {
                        unitBase.SetPlayerSteamImage(playerInfo);
                        break;
                    }
                }
                Debug.Log("uh");
            }
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion
}