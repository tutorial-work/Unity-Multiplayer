using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.SetDisplayName($"Player {numPlayers}");

        Color color = new Color();
        color.r = Random.Range(0, 1f);
        color.b = Random.Range(0, 1f);
        color.g = Random.Range(0, 1f);
        player.SetDisplayColor(color);
    }
}
