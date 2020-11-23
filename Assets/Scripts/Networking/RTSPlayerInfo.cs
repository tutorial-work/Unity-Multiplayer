using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;
using Steamworks;

public class RTSPlayerInfo : NetworkBehaviour
{
    /********** MARK: Variables **********/
    #region Variables

    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    ulong steamId;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    string displayName;

    //[SyncVar(hook = nameof(ClientHandleDisplayTextureUpdated))]
    Texture2D displayTexture;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    bool isPartyOwner = false;

    //RawImage profileImage = null;

    Color teamColor = new Color();

    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    #endregion

    /********** MARK: Properties **********/
    #region Properties

    public ulong SteamId
    {
        get
        {
            return steamId;
        }

        [Server]
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

        [Server]
        set
        {
            displayName = value;
        }
    }

    public Texture2D DisplayTexture
    {
        get
        {
            return displayTexture;
        }
    }

    public Color TeamColor
    {
        get
        {
            return teamColor;
        }

        [Server]
        set
        {
            teamColor = value;
        }
    }

    public bool IsPartyOwner
    {
        get
        {
            return isPartyOwner;
        }

        [Server]
        set
        {
            isPartyOwner = value;
        }
    }

    public UnitBase MyUnitBase { get; [Server] set; }

    #endregion

    /********** MARK: Server Functions **********/
    #region Server Functions
        

    #endregion

    /********** MARK: Client Functions **********/
    #region Client Functions

    public override void OnStartClient()
    {
        Debug.Log("Adding Client");

        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);

        UnitBase.ServerOnBaseSpawned += ClientHandleOnBaseSpawned;
    }

    public override void OnStopClient()
    {
        UnitBase.ServerOnBaseSpawned -= ClientHandleOnBaseSpawned;
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID != steamId) return;

        displayTexture = GetSteamImageAsTexture(callback.m_iImage);
        ClientOnInfoUpdated?.Invoke();
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int) (width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);

                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

    #endregion

    /********** MARK: Handle Functions **********/
    #region Handle Functions

    private void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        CSteamID steamId = new CSteamID(newSteamId);

        displayName = SteamFriends.GetFriendPersonaName(steamId);

        int imageId = SteamFriends.GetLargeFriendAvatar(steamId);

        if (imageId == -1) return;

        displayTexture = GetSteamImageAsTexture(imageId);
        ClientOnInfoUpdated?.Invoke();
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    //private void ClientHandleDisplayTextureUpdated(Texture2D oldTexture, Texture2D newTexture)
    //{
    //    ClientOnInfoUpdated?.Invoke();
    //}

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) return;

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    public void AuthorityHandleOnStopClient()
    {
        //if (!hasAuthority) return; // IDK ahhhh

        ClientOnInfoUpdated?.Invoke();
    }

    public void ClientHandleOnBaseSpawned(UnitBase unitBase)
    {
        unitBase.CmdSetPlayerSteamImage();
    }

    #endregion
}
