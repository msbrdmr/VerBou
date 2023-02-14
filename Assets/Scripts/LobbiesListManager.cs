using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{

    public static LobbiesListManager instance;
    public List<GameObject> LobbiesMenuItems;
    public List<GameObject> MainMenuElements;
    public GameObject LobbyDataItemPrefab;
    public GameObject LobbyListContent; //this will be the parent of lobby objects


    public List<GameObject> ListOfLobbies = new List<GameObject>();//will hold all the lobbies that are created by particular user's all of the friends
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void DestroyLobbies()
    {
        foreach (GameObject item in ListOfLobbies)
        {
            Destroy(item);
        }
        ListOfLobbies.Clear();
    }
    public void DisplayLobbies(List<CSteamID> alllobbyIDs, LobbyDataUpdate_t result)
    {
        //result is a callback after fetching all the lobbies.
        for (int i = 0; i < alllobbyIDs.Count; i++)
        {
            if (alllobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                //this checks if the lobby exists.
                //then we create the lobby item to show in the list.
                GameObject CreatedItem = Instantiate(LobbyDataItemPrefab);

                CreatedItem.GetComponent<LobbyDataEntry>().LobbyID = (CSteamID)alllobbyIDs[i].m_SteamID;

                CreatedItem.GetComponent<LobbyDataEntry>().LobbyName = SteamMatchmaking.GetLobbyData((CSteamID)alllobbyIDs[i].m_SteamID, "name");

                CreatedItem.GetComponent<LobbyDataEntry>().setLobbyData();


                CreatedItem.transform.SetParent(LobbyListContent.transform);

                CreatedItem.transform.localScale = Vector3.one;

                ListOfLobbies.Add(CreatedItem);

            }
        }
    }

    public void ListLobbies()
    {
        foreach (GameObject element in MainMenuElements)
        {
            element.SetActive(false);
        }
        foreach (GameObject element in LobbiesMenuItems)
        {
            element.SetActive(true);
        }

        SteamLobby.instance.GetLobbiesList();

    }

    

}
