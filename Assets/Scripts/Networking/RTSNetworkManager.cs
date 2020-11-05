using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    /********** MARK: Private Variables **********/
    #region Private Variables

    [SerializeField] GameObject unitSpawnerPrefab = null;
    [SerializeField] GameOverHandler gameOverHandlerPrefab = null;

    #endregion

    //private void Awake()
    //{
    //    GetComponent<NetworkManagerHUD>().offsetX = (Screen.width / 2) - 100;
    //}

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Vector3 pos = conn.identity.transform.position;

        Quaternion rot = conn.identity.transform.rotation;

        // spawns instance on server
        GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, pos, rot);

        // server tells all clients to spawn instance
        NetworkServer.Spawn(unitSpawnerInstance, conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
        }
    }
}
