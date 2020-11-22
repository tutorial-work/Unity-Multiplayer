using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;

public class RTSPlayerInfo : NetworkBehaviour
{
    /********** MARK: Variables **********/
    #region Variables
        
    ulong steamId;
    
    string displayName;

    RawImage profileImage = null;

    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

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

    public override void OnStartClient()
    {
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }

    private void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        CSteamID steamId = new CSteamID(newSteamId);

        displayName = SteamFriends.GetFriendPersonaName(steamId);

        int imageId = SteamFriends.GetLargeFriendAvatar(steamId);

        if (imageId == -1) return;

        profileImage.texture = GetSteamImageAsTexture(imageId);
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID != steamId) return;

        profileImage.texture = GetSteamImageAsTexture(callback.m_iImage);
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
}
