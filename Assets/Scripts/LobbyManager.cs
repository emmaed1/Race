using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
            AddPlayerToList(NetworkManager.LocalClientId);
            RefreshPlayerPanels();
        }
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
        foreach (PlayerInfo pi in allNetPlayers)
        {
            if (pi.clientId == clientId)
            {
                allNetPlayers.Remove(pi);
            }

        }
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
        newPanel.GetComponent<LobbyPlayerLabel>().setPlayerName(info.clientId);
        newPanel.GetComponent<LobbyPlayerLabel>().onKickClicked += kickUserBtn;

        if(IsClient && !IsHost) newPanel.GetComponent<Button>().gameObject.SetActive(false);
        playerPanels.Add(newPanel);
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
    private void kickUserBtn(ulong KickTarget)
    {
        if (!IsServer || !IsHost) return;
        foreach(PlayerInfo pi in allNetPlayers)
        {
            if(pi.clientId == KickTarget)
            {
                allNetPlayers.Remove(pi);
                NetworkManager.Singleton.DisconnectClient(KickTarget);
            }
        }
    }

}
