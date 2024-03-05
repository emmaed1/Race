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
        newPanel.GetComponent<LobbyPlayerLabel>().setPlayerName("Player " + info.clientId.ToString());

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
}
