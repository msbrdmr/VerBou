using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uimanager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI healthtext;
    private void Start()
    {
        player = GameObject.Find("LocalGamePlayer");
        healthtext.text = player.ToString();
    }
    private void Update()
    {
        healthtext.text = player.GetComponent<PlayerHealthScript>().Health.ToString();
    }
}
