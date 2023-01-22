using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Synced Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;// whenever this var changes , a func is called this means this var is hook
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool isReady;



    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();

    }
    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();

    }
    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();

    }

    [Command]
    private void CmdSetPlayerName(string PlayeName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayeName);
    }
    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer)
        {
            this.PlayerName = newValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            this.isReady = newValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();// this method of Lobby controller, loops over all the list items using foreach, then calls SetPlayerValues method of PlayerListItem script
        }
    }

    [Command]
    private void CmdSetReadyValue()
    {
        this.PlayerReadyUpdate(this.isReady, !this.isReady);//this code(function) is running on server.

    }

    public void changeReady()
    {
        if (isOwned)//if this is us changing the status
        {
            CmdSetReadyValue();
        }
    }
}
