﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceGenerator : NetworkBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] private Health health = null;

    [SerializeField] private int resourcesPerInterval = 10;

    [SerializeField] private float interval = 2f;

    float timer;
    RTSPlayer player;

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<RTSPlayer>();

        health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            player.Resources = player.Resources + resourcesPerInterval;

            timer += interval;
        }
    }

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void ServerHandleGameOver()
    {
        enabled = false;
    }

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    #endregion
}
