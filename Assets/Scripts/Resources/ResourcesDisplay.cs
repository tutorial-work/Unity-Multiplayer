using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ResourcesDisplay : MonoBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SerializeField] private TMP_Text resourcesText = null;

    private RTSPlayer player;

    #endregion

    /********** MARK: Unity Functions **********/
    #region Unity Functions

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        ClientHandleResourcesUpdated(player.Resources);
        player.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
    }

    private void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
    }

    #endregion

    /********** MARK: Class Functions **********/
    #region Class Functions

    private void ClientHandleResourcesUpdated(int resources)
    {
        resourcesText.text = $"Resources: {resources}";
    }

    #endregion
}