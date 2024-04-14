using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] Button StartGameBtn;
    [SerializeField] Button ReadyBtn;
    [SerializeField] Button LeaveGameBtn;
    [SerializeField] private GameObject PanelPrefab;
    [SerializeField] private GameObject ContentGO;
    [SerializeField] private TMP_Text rdyTxt;

    public NetworkPlayers _NetworkedPlayers;

    private List<GameObject> playerPanels = new List<GameObject>();

    private ulong myLocalClientId;
    private bool isReady = false;

    private void Start()
    {
        myLocalClientId = NetworkManager.LocalClientId;

        if (IsHost)
        {
            _NetworkedPlayers.allNetPlayers.OnListChanged += ServerOnNetPlayersChanged;
            ServerPopulateLabels();
            rdyTxt.text = "waiting for ready players";
        }
        else
        {
            ClientPopulateLabels();
            _NetworkedPlayers.allNetPlayers.OnListChanged += ClientOnAllPlayersChanged;
            
            ReadyBtn.onClick.AddListener(ClientRdyBttnToggled);
            rdyTxt.text = "not ready";
        }
        LeaveGameBtn.onClick.AddListener(LeaveGameBtnClick);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RdyBttnToggleServerRpc(bool readyStatus, ServerRpcParams serverRpcParams = default)
    {
        NetworkLog.LogInfo("Ready Btn ServerRPC");
        _NetworkedPlayers.UpdateReadyClient(serverRpcParams.Receive.SenderClientId, readyStatus);
        ServerPopulateLabels();
        UpdatePlayerLabelsClientRpc();
    }

    [ClientRpc]
    public void UpdatePlayerLabelsClientRpc()
    {
        NetworkLog.LogInfo("Ready Btn clientRPC");
        if(!IsHost){
            ClientPopulateLabels();
        }
    }

    public void ClientRdyBttnToggled()
    {
        isReady = !isReady;
        if (isReady)
        {
            rdyTxt.text = "Ready!";
        }
        else
        {
            rdyTxt.text = "Not Ready!";
        }        
        RdyBttnToggleServerRpc(isReady);
    }

    private void ServerPopulateLabels()
    {
        ClearPlayerPanels();
        if(IsHost){
            foreach (PlayerInfo pi in _NetworkedPlayers.allNetPlayers)
            {
                GameObject newPanel = Instantiate(PanelPrefab, ContentGO.transform);
                LobbyPlayerLabel LPL = newPanel.GetComponent<LobbyPlayerLabel>();
                LPL.onKickClicked += KickUserBttn;
                // make sure we only the host or server displays kick button,
                if (pi.clientId == NetworkManager.LocalClientId)
                {
                    LPL.setKickActive(false);
                }
                else
                {
                    LPL.setKickActive(true);
                }
               //Display info and status status
                LPL.setPlayerName(pi.clientId);             
                LPL.SetReady(pi.isPlayerReady);
                Debug.Log("Player ready status: " + pi.isPlayerReady);
                LPL.SetIconColor(pi.colorId);
                playerPanels.Add(newPanel);
            } 
            //hides ready button
            ReadyBtn.GameObject().SetActive(false);
        }
    }

    private void ClientPopulateLabels()
    {
        ClearPlayerPanels();
        if(!IsHost){
            foreach (PlayerInfo pi in _NetworkedPlayers.allNetPlayers)
            {
                GameObject newPanel = Instantiate(PanelPrefab, ContentGO.transform);
                LobbyPlayerLabel LPL = newPanel.GetComponent<LobbyPlayerLabel>();
                LPL.onKickClicked += KickUserBttn;
               //Turn off kick button for client.
                LPL.setKickActive(false);
            
                //Display info and status status
                LPL.setPlayerName(pi.clientId);
                LPL.SetReady(pi.isPlayerReady);
           
                LPL.SetIconColor(pi.colorId);
                playerPanels.Add(newPanel);
            }
            ReadyBtn.GameObject().SetActive(true);
        }
    }

    private void ClearPlayerPanels()
    {
        foreach (GameObject panel in playerPanels)
        {
            Destroy(panel);
        }
        playerPanels.Clear();
    }

    public void KickUserBttn(ulong kickTarget)
    {
        if (!IsServer || !IsHost) return;
        foreach (PlayerInfo pi in _NetworkedPlayers.allNetPlayers)
        {
            if (pi.clientId == kickTarget)
            {
                _NetworkedPlayers.allNetPlayers.Remove(pi);
                
                // send RPC to target client to discconnect/scene
                DisconnectClient(kickTarget);            
            }
        }
      }

    private void LeaveGameBtnClick()
    {
        if (!IsServer)
        {
            QuitLobbyServerRpc();
        }
        else
        {
            NetworkManager.Shutdown();  
        }
    }

    public void fixedupdate()
    {
        if (NetworkManager.ShutdownInProgress)
        {
            SceneManager.LoadScene(0);
        }
    }
    private void ServerOnNetPlayersChanged(NetworkListEvent<PlayerInfo> changeevent)
    {
        ServerPopulateLabels();
    }

    [ServerRpc(RequireOwnership = false)]
    public void QuitLobbyServerRpc(ServerRpcParams serverRpcParams=default)
    {
        KickUserBttn(serverRpcParams.Receive.SenderClientId);
    }

    public void DisconnectClient(ulong kickTarget)
    {
        ClientRpcParams clientRpcParams = default;
        clientRpcParams.Send.TargetClientIds = new ulong[1] { kickTarget };
        DisconnectionClientRPC(clientRpcParams);
        NetworkManager.Singleton.DisconnectClient(kickTarget);     
    }
    private void ClientOnAllPlayersChanged(NetworkListEvent<PlayerInfo> changeEvent)
    {
        ClientPopulateLabels();
    }

    [ClientRpc]
    public void DisconnectionClientRPC(ClientRpcParams clientRpcParams)
    {
        SceneManager.LoadScene(0); 
    }

    public void OnStartGame(){
        NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
