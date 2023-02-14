using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class PlayerObjectControllerTemp : NetworkBehaviour
{
    //Player Synced Data
    public override void OnStartAuthority()
    {
        // CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        // LobbyController.instance.FindLocalPlayer();
        // LobbyController.instance.UpdateLobbyName();

    }
    public override void OnStartClient()
    {
        // Manager.GamePlayers.Add(this);

        if (!isOwned) gameObject.name = "Client";
        // LobbyController.instance.UpdateLobbyName();
        // LobbyController.instance.UpdatePlayerList();

    }
    public override void OnStopClient()
    {
        // Manager.GamePlayers.Remove(this);
        // LobbyController.instance.UpdatePlayerList();

    }
}
