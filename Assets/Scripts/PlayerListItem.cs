using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;
using Mirror;
using System;

public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionId;
    public ulong PlayerSteamID;

    private bool AvatarReceived;

    public TextMeshProUGUI PlayerNameText;
    public RawImage PlayerImage;

    public TextMeshProUGUI PlayerReadyText;
    public bool isReady;

    protected Callback<AvatarImageLoaded_t> AvatarLoaded;

    private void Start()
    {
        AvatarLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    public void setReadyStatus()
    {
        if (isReady) { PlayerReadyText.text = "Ready"; PlayerReadyText.color = Color.green; }
        if (!isReady) { PlayerReadyText.text = "Unready"; PlayerReadyText.color = Color.red; }
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {

        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        { //us
            PlayerImage.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else// another player
        {
            return;
        }
    }

    private Texture GetSteamImageAsTexture(int iImage)
    {
        Texture2D Texture = null;
        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
            if (isValid)
            {
                Texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                Texture.LoadRawTextureData(image);
                Texture.Apply();
            }
        }
        AvatarReceived = true;
        return Texture;

    }

    public void SetPlayerVaIues()
    {
        PlayerNameText.text = PlayerName;
        setReadyStatus();
        if (!AvatarReceived) { GetPIayerIcon(); }
    }
    void GetPIayerIcon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if (ImageID == -1) { return; }
        PlayerImage.texture = GetSteamImageAsTexture(ImageID);
    }

}
