using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerReference : MonoBehaviour
{
    public static CustomNetworkManager manager;

    private void Awake()
    {
        manager = GetComponent<CustomNetworkManager>();
    }
}

