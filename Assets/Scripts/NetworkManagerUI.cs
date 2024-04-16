using System;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Start()
    {
        playBtn.onClick.AddListener(startClick);
        serverBtn.onClick.AddListener(serverClick);
        hostBtn.onClick.AddListener(hostClick);
        clientBtn.onClick.AddListener(clientClick);

        NetworkManager.OnServerStarted += OnServerStarted;
        NetworkManager.OnClientStarted += OnClientStarted;

        playBtn.gameObject.SetActive(false);
    }
    
    private void OnServerStarted()
    {
        playBtn.gameObject.SetActive(true);
    }
    private void OnClientStarted()
    {
        if (!IsHost)
        {
            Console.WriteLine("Wait for game to start");
        }
    }

    private void hostClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void clientClick()
    {
        NetworkManager.Singleton.StartClient();
    }
    private void serverClick()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void startClick()
    {
        NetworkManager.SceneManager.LoadScene("PlayerLobby", LoadSceneMode.Single);
    }

    private void Update()
    {
        playersCountText.text = "Players: " + playersNum.Value.ToString();
        if (!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
