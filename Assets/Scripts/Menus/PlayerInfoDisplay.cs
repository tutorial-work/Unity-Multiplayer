using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;

public class PlayerInfoDisplay : NetworkBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    ulong steamId;

    //[SerializeField] RawImage profileImage = null;
    string displayName;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public ulong SteamId
    {
        get
        {
            return steamId;
        }
        set
        {
            steamId = value;
        }
    }

    public string DisplayName
    {
        get
        {
            return displayName;
        }
    }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    private void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        CSteamID steamId = new CSteamID(newSteamId);

        displayName = SteamFriends.GetFriendPersonaName(steamId);
    }

    #endregion
}
