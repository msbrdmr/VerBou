using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;
    //Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEnter;

    //Variables
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;

    //GameObject

    private void Start()
    {
        if (!SteamManager.Initialized) { return; }
        if (instance == null) { instance = this; }
        manager = GetComponent<CustomNetworkManager>();
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }


    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }
        Debug.Log("Lobby Created Successfully");
        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        //Here, the "HostAddress" key of lobby is set to the persons steamid (user steam id ).
        //whoever starts a host, the lobby's "HostAddress" data is set to that players steam id. We will use this data later to join to this lobby from another client.
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        // After starting host, I get the data from callback and then, set the lobby data to that data that came from callback.
        //Setlobbydata takes lobby id which is Csteamid data type(class) and key value pairs for example name string is a key and next arg is new value for name attribute.
        // there are other keys but those are not in our concern.
    }
    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request to join lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        //Say I am trying to join a lobby. Steam will run this func with that callback.
        // I need to run joinlobby with the steam lobby id (id of the owner) data from callback.

    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //For Everyone including host to enter lobby
        //only for clients
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        if (NetworkServer.active) { return; } 
        // This line checks if we are a host or not. If we are host, then our NetworkServer.active is true but if we are not, we are client. Then we can enter the lobby.
        //if we are client, we need to get the host address of the lobby we are trying to join
        string to_join_host_address = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),HostAddressKey);
        manager.networkAddress = to_join_host_address;// Here, I am setting the networkaddress of our networkmanager which is the address that i am going to connect to.
        manager.StartClient();
    }
}
