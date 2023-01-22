using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using TMPro;
using System.Linq;
using System;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;
    //UI elements
    public TextMeshProUGUI LobbyNameText;
    //player data
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;
    //other data
    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public PlayerObjectController LocalPlayerController;

    // Ready system
    public Button startGameButton;
    public TextMeshProUGUI ReadyButtonText;


    //Manager
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }
    public void ReadyPlayer()
    {
        LocalPlayerController.changeReady();
    }
    public void updateButton()
    {
        if (LocalPlayerController.isReady) { ReadyButtonText.text = "Unready"; }
        else { ReadyButtonText.text = "Ready"; }
    }
    public void checkIfAllReady()
    {
        bool AllReady = false;
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (player.isReady) { AllReady = true; }
            else { AllReady = false; break; }
        }

        if (AllReady)
        {
            // only host can start the game (interactable for host only)
            if (LocalPlayerController.PlayerIDNumber == 1)
            {
                startGameButton.interactable = true;
            }
            else
            {
                startGameButton.interactable = false;
            }
        }
        else { startGameButton.interactable = false; }
    }
    public void UpdateLobbyName()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }
    public void UpdatePlayerList()
    {
        if (!PlayerItemCreated) { CreateHostPlayerItem(); }
        if (PlayerListItems.Count < Manager.GamePlayers.Count) { CreateClientPlayerItem(); }
        if (PlayerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem(); }
        if (PlayerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem(); }
    }
    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }
    private void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)

        {
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();
            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionId = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.isReady = player.isReady;
            NewPlayerItemScript.SetPlayerVaIues();

            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;
            PlayerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }
    private void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)

        {
            if (!PlayerListItems.Any(b => b.ConnectionId == player.ConnectionID))
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();
                NewPlayerItemScript.PlayerName = player.PlayerName;
                NewPlayerItemScript.ConnectionId = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.isReady = player.isReady;
                NewPlayerItemScript.SetPlayerVaIues();

                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;
                PlayerListItems.Add(NewPlayerItemScript);
            }
        }
    }
    private void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in PlayerListItems)
            {
                if (playerListItemScript.ConnectionId == player.ConnectionID)
                {
                    playerListItemScript.PlayerName = player.PlayerName;
                    playerListItemScript.isReady = player.isReady;
                    playerListItemScript.SetPlayerVaIues();
                    if (player == LocalPlayerController)
                    {
                        //if this is us
                        updateButton();
                        // we want to update buttons individually. not for every single person.
                    }
                }
            }
        }

        checkIfAllReady();
    }
    private void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();
        foreach (PlayerListItem playerlistItem in PlayerListItems)
        {
            if (!Manager.GamePlayers.Any(b => b.ConnectionID == playerlistItem.ConnectionId))
            {
                playerListItemToRemove.Add(playerlistItem);
            }
        }

        if (playerListItemToRemove.Count > 0)
        {
            foreach (PlayerListItem playerlistItemtoRemove in playerListItemToRemove)
            {
                GameObject objtoremove = playerlistItemtoRemove.gameObject;
                PlayerListItems.Remove(playerlistItemtoRemove);
                Destroy(objtoremove);
                objtoremove = null;
            }
        }
    }


}