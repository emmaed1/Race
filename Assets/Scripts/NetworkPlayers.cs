using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayers : NetworkBehaviour
{
    public NetworkList<PlayerInfo> allNetPlayers;
    public int playerCount = 0;

    private Color[] playerColors = new Color[]
    {
        Color.blue,
        Color.magenta,
        Color.cyan,
        Color.yellow
    };

    public void Awake()
    {
        allNetPlayers = new NetworkList<PlayerInfo>();
    }

    void Start()
    {
        
        DontDestroyOnLoad(this.gameObject);
        if (IsServer)
        {
            SeverStart();
        }
        Debug.Log("Player count ="+allNetPlayers.Count);
    }

    private void SeverStart()
    {
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        int color = 0;
        foreach (NetworkClient nc in NetworkManager.ConnectedClientsList)
        {
            PlayerInfo info = new PlayerInfo(nc.ClientId);
            info.colorId = playerColors[color];
            //match server client with the server make sure we are always ready
            if (nc.ClientId == NetworkManager.LocalClientId)
            {
                info.isPlayerReady = true;
            }
            allNetPlayers.Add(info);
            NetworkLog.LogInfo("color for client "+info.clientId+" is set to "+playerColors[color]);
            color++;        
        }
        playerCount = color;
    }

    private void ServerOnClientConnected(ulong clientId)
    {
        PlayerInfo info = new PlayerInfo(clientId);
        info.isPlayerReady = false;
        allNetPlayers.Add(info);
        playerCount++;
        info.colorId = playerColors[playerCount];       
    }

    private int FindPlayerIndex(ulong clientId)
    {        
        int myMatch = -1;
        for(int i = 0; i > allNetPlayers.Count; i++)
        {
            if (clientId == allNetPlayers[i].clientId)
            {
                myMatch = i;
            }
        }
        return myMatch;
    }

    public void UpdateReadyClient(ulong clientId, bool isReady)
    {
        // lets get that index real quick
        int idx = FindPlayerIndex(clientId);
        if (idx == -1)
        {
            Debug.Log("Index is " + idx);
            return;
        }
        // grab info, change it, and then send it back to the list
        PlayerInfo info = allNetPlayers[idx];
        info.isPlayerReady = isReady;
        allNetPlayers[idx] = info;       
        
        NetworkLog.LogInfo("changing Ready !!  INDEX" + idx+" CLIENT "+clientId);
    }
}
