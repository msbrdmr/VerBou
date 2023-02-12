using UnityEngine;
using Steamworks;
using TMPro;

public class LobbyDataEntry : MonoBehaviour
{
    //Data
    public CSteamID LobbyID;
    public string LobbyName;
    public TextMeshProUGUI LobbyNameText;


    public void setLobbyData()
    {
        //set data for lobby
        if (LobbyName == "") { LobbyNameText.text = "Empty"; }
        else { LobbyNameText.text = LobbyName; }

    }

    public void joinLobby()
    {
        //I will assign this to button
        //need to access the function that allows me to join the lobby

        //that function is in  SteamLobby script.
        SteamLobby.instance.JoinLobby(LobbyID);

    }



}
