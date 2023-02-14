using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostButtonHandler : MonoBehaviour
{
    public GameObject NetworkMan;
    public Button HostButton;
    void Start()
    {
        NetworkMan = GameObject.Find("NetworkManager");
        HostButton.onClick.AddListener(delegate () { NetworkMan.GetComponent<SteamLobby>().HostLobby(); });
    }

}
