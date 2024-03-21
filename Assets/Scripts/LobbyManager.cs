using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] public Button StartGameBtn;
    [SerializeField] public Button ReadyBtn;
    [SerializeField] public Button LeaveGameBtn;
    [SerializeField] private GameObject PanelPrefab;
    [SerializeField] private GameObject ContentGO;

    public NetworkList<PlayerInfo> allNetPlayers = new NetworkList<PlayerInfo>();
    private List<GameObject> playerPanels = new List<GameObject>();

    private ulong myLocalClientId;

    private Color[] playerColors = new Color[]
    {
        Color.blue,
        Color.magenta,
        Color.cyan,
        Color.yellow
    };

    private void Start()
    {
        if (IsHost)
        {
            foreach (NetworkClient nc in NetworkManager.ConnectedClientsList)
            {
                AddPlayerToList(nc.ClientId);
            }        
            RefreshPlayerPanels();
        }
        myLocalClientId = NetworkManager.LocalClientId;
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        }
        // must be after host connects to signals
        base.OnNetworkSpawn();
        if (IsClient)
        {
            allNetPlayers.OnListChanged += ClientOnAllPlayersChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback += ClientDissconnected;
        }
    }

    private void ClientDissconnected(ulong clientId)
    {
        
    }

    private void ClientOnAllPlayersChanged(NetworkListEvent<PlayerInfo> changeEvent)
    {
        RefreshPlayerPanels();
    }

    private void HostOnClientConnected(ulong clientId)
    {
        AddPlayerToList(clientId);
        RefreshPlayerPanels();
    }

    //  private NetworkList<PlayerInfo> allPlayers = new NetworkList<PlayerInfo>();
    // Start is called before the first frame update

    // Add Players panel phystically triger each prefab
    private void AddPlayerToList(ulong clientId)
    {
        allNetPlayers.Add(new PlayerInfo(clientId));
    }

    private void AddPlayerPanel(PlayerInfo info)
    {

        GameObject newPanel = Instantiate(PanelPrefab, ContentGO.transform);
        LobbyPlayerLabel LPL = newPanel.GetComponent<LobbyPlayerLabel>();
        LPL.setPlayerName(info.clientId);
        
        if(IsServer)
        {
            LPL.onKickClicked += kickUserBtn;

            //assume server is always ready and set it to true
            info.isPlayerReady = true;
            ReadyBtn.gameObject.SetActive(false);
        }

        if (IsClient && !IsHost || info.clientId == myLocalClientId)
        {
            LPL.enableKick(false);
        }

        LPL.SetReady(info.isPlayerReady);
        LPL.SetIconColor(playerColors[findPlayerIndex(info.clientId)]);
        playerPanels.Add(newPanel);
    }

    private int findPlayerIndex(ulong clientId)
    { 
        int index = 0;
        int myMatch = 0;

        foreach (NetworkClient nc in NetworkManager.ConnectedClientsList)
        {
            if(nc.ClientId == clientId)
            {
                //match found
                myMatch = index;        
            }
            index++;
        }
        return myMatch;
    }

    private void RefreshPlayerPanels()
    {
        foreach (GameObject panel in playerPanels)
        {
            Destroy(panel);

        }
        playerPanels.Clear();

        foreach (PlayerInfo pi in allNetPlayers)
        {
            AddPlayerPanel(pi);
        }
    }
    private void kickUserBtn(ulong kickTarget)
    {
        if (!IsServer || !IsHost) return;
        foreach(PlayerInfo pi in allNetPlayers)
        {
            if(pi.clientId == kickTarget)
            {
                allNetPlayers.Remove(pi);
                //send RPC to targetClient to disconnect/scene
                DisconnectClient(kickTarget);
            }
        }
        RefreshPlayerPanels();
    }

    public void DisconnectClient(ulong kickTarget)
    {
        ClientRpcParams clientRpcParams = default;
        clientRpcParams.Send.TargetClientIds = new ulong[1] { kickTarget };
        DisconnectClientRPC(clientRpcParams);
        NetworkManager.Singleton.DisconnectClient(kickTarget);
    }

    [ClientRpc]
    public void DisconnectClientRPC(ClientRpcParams clientRpcParams)
    {
        SceneManager.LoadScene(0);
    }
}
